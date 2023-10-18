﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.Service.Contracts;

namespace YogaCenter.BackEnd.Service.Implementations
{
    public class JwtService :IJwtService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly TokenDto _tokenDto;


        public JwtService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _configuration = configuration;
            _tokenDto = new TokenDto();
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        public async Task<string> GenerateAccessToken(LoginRequestDto loginRequest)
        {
            var user = await _unitOfWork.GetRepository<ApplicationUser>().GetByExpression(u => u.Email.ToLower() == loginRequest.Email.ToLower());

            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles != null)
                {
                    var claims = new List<Claim>
                    {
                       new Claim (ClaimTypes.Email, loginRequest.Email),
                       new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                       new Claim("AccountId", user.Id)
                    };
                    claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));


                    var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:Issuer"],
                        audience: _configuration["JWT:Audience"],
                        expires: DateTime.Now.AddDays(1),
                        claims: claims,
                        signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
                        );
                    return new JwtSecurityTokenHandler().WriteToken(token);
                }
            }

            return string.Empty;

        }
        public async Task<TokenDto> GetNewToken(string refreshToken, string accountId)
        {
            var user = await _unitOfWork.GetRepository<ApplicationUser>().GetByExpression(u => u.Id.ToLower() == accountId);
            if (user.RefreshToken == refreshToken)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var claims = new List<Claim>
            {
            new Claim (ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("AccountId", user.Id)
               };
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
                var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
                var token = new JwtSecurityToken
                    (
                    issuer: _configuration["JWT:Issuer"],
                    audience: _configuration["JWT:Audience"],
                    expires: DateTime.Now.AddDays(1),
                    claims: claims,
                    signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
                    );

                _tokenDto.Token = new JwtSecurityTokenHandler().WriteToken(token);
                if (user.RefreshTokenExpiryTime <= DateTime.Now)
                {
                    user.RefreshToken = GenerateRefreshToken();
                    user.RefreshTokenExpiryTime = DateTime.Now.AddDays(1);
                    _unitOfWork.SaveChange();
                    _tokenDto.RefreshToken = user.RefreshToken;
                }
                else
                {
                    _tokenDto.RefreshToken = refreshToken;
                }
            }
            return _tokenDto;

        }


    }
}
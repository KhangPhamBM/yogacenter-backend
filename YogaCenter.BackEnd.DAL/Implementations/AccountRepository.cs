using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
using YogaCenter.BackEnd.DAL.Data;
using YogaCenter.BackEnd.DAL.Models;

namespace YogaCenter.BackEnd.DAL.Implementations
{
    public class AccountRepository : Repository<ApplicationUser>, IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly TokenDto _tokenDto;
        private readonly YogaCenterContext _db;

        public AccountRepository(
            YogaCenterContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
            : base(context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _tokenDto = new TokenDto();
            _db = context;
        }

        public async Task<TokenDto> Login(LoginRequestDto loginRequest)
        {
            var result = await _signInManager.PasswordSignInAsync(loginRequest.Email, loginRequest.Password, false, false);
            if (!result.Succeeded)
            {
                return null;

            }
            else
            {
                string token = await GenerateAccessToken(loginRequest);
                var user = await _db.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == loginRequest.Email.ToLower());
                if (user != null)
                {
                    if (user.RefreshToken == null || user.RefreshTokenExpiryTime <= DateTime.Now)
                    {
                        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(1);
                        user.RefreshToken = GenerateRefreshToken();
                        await _db.SaveChangesAsync();

                    }
                    _tokenDto.Token = token;
                    _tokenDto.RefreshToken = user.RefreshToken;
                }
            }



            return _tokenDto;

        }

        private async Task<string> GenerateAccessToken(LoginRequestDto loginRequest)
        {
            var user = await _db.Users.SingleOrDefaultAsync(u => u.Email.ToLower() == loginRequest.Email.ToLower());

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
            var user = await _db.Users.SingleOrDefaultAsync(u => u.Id.ToLower() == accountId);
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
                    await _db.SaveChangesAsync();
                    _tokenDto.RefreshToken = user.RefreshToken;
                }
                else
                {
                    _tokenDto.RefreshToken = refreshToken;

                }

            }

            return _tokenDto;

        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        public async Task<ApplicationUser>  SignUp(SignUpRequestDto dto)
        {
            var user = new ApplicationUser
            {
                Email = dto.Email,
                UserName = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                Gender = dto.Gender

            };
            await _userManager.CreateAsync(user, dto.Password);
            return user;
        }

        public async Task AssignRole(string userId, string roleName)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id.ToLower() == userId);
            var roleDb = await _db.Roles.SingleOrDefaultAsync(r => r.Name == roleName);
            if (roleDb == null)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));

            }
            if (user != null)
            {
                await _userManager.AddToRoleAsync(user, roleName);
            }
        }
    }
}

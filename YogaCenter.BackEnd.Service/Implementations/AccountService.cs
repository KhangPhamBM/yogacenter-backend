using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.Service.Contracts;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using YogaCenter.BackEnd.DAL.Util;
using YogaCenter.BackEnd.DAL.Common;

namespace YogaCenter.BackEnd.Service.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppActionResult _result;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly TokenDto _tokenDto;
        private readonly IJwtService _jwtService;

        public AccountService(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtService jwtService
            )
        {
            _unitOfWork = unitOfWork;
            _result = new AppActionResult();
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }


        public async Task<AppActionResult> Login(LoginRequestDto loginRequest)
        {
            bool isValid = true;
            try
            {
                if (await _unitOfWork.GetRepository<ApplicationUser>().GetByExpression(u => u.Email.ToLower() == loginRequest.Email.ToLower()) != null)
                {
                    isValid = false;
                    _result.Message.Add($"The user with username {loginRequest.Email} not found");
                }
                var result = await _signInManager.PasswordSignInAsync(loginRequest.Email, loginRequest.Password, false, false);
                if (!result.Succeeded)
                {
                    isValid = false;
                    _result.Message.Add(SD.ResponseMessage.LOGIN_FAILED);
                }

                if (isValid)
                {
                    string token = await _jwtService.GenerateAccessToken(loginRequest);

                    var user = await _unitOfWork.GetRepository<ApplicationUser>().GetByExpression(u => u.Email.ToLower() == loginRequest.Email.ToLower());
                    if (user.RefreshToken == null)
                    {
                        user.RefreshToken = _jwtService.GenerateRefreshToken();
                        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(1);

                    }
                    if (user.RefreshTokenExpiryTime <= DateTime.Now)
                    {
                        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(1);
                        user.RefreshToken = _jwtService.GenerateRefreshToken();
                    }

                    _unitOfWork.SaveChange();
                    _tokenDto.Token = token;
                    _tokenDto.RefreshToken = user.RefreshToken;
                    _result.Data = _tokenDto;
                }
            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }

            return _result;

        }
        public async Task<AppActionResult> CreateAccount(SignUpRequestDto signUpRequest)
        {
            try
            {
                if (await _unitOfWork.GetRepository<IdentityRole>().GetByExpression(r => r.Name == signUpRequest.RoleName) == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole(signUpRequest.RoleName));

                }
                var user = new ApplicationUser
                {
                    Email = signUpRequest.Email,
                    UserName = signUpRequest.Email,
                    FirstName = signUpRequest.FirstName,
                    LastName = signUpRequest.LastName,
                    PhoneNumber = signUpRequest.PhoneNumber,
                    Gender = signUpRequest.Gender

                };
                await _userManager.CreateAsync(user, signUpRequest.Password);
                await _userManager.AddToRoleAsync(user, signUpRequest.RoleName);

                _result.Message.Add(SD.ResponseMessage.CREATE_SUCCESS);
            }
            catch (Exception ex)
            {
                _result.Message.Add(ex.Message);
                _result.isSuccess = false;
            }

            return _result;



        }
        
        public async Task<AppActionResult> UpdateAccount(ApplicationUser applicationUser)
        {
            bool isValid = true;
            try
            {

                if (await _unitOfWork.GetRepository<ApplicationUser>().GetById(applicationUser) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The user with id {applicationUser.Id} not found");
                }
                if (isValid)
                {
                    await _unitOfWork.GetRepository<ApplicationUser>().Update(applicationUser);
                    _unitOfWork.SaveChange();
                    _result.Message.Add(SD.ResponseMessage.UPDATE_SUCCESS);
                }
            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);

            }

            return _result;
        }

        public async Task<AppActionResult> GetAccountByUserId(string id)
        {
            bool isValid = true;
            try
            {

                if (await _unitOfWork.GetRepository<ApplicationUser>().GetById(id) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The user with id {id} not found");

                }
                if (isValid)
                {
                    _result.Data = await _unitOfWork.GetRepository<ApplicationUser>().GetById(id);

                }
            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> GetAllAccount()
        {
            try
            {
                List<AccountResponse> accounts = new List<AccountResponse>();
               var list =  await _unitOfWork.GetRepository<ApplicationUser>().GetAll();
                foreach (var account in list)
                {
                    var userRole = await _unitOfWork.GetRepository<IdentityUserRole<string>>().GetByExpression(s=> s.UserId== account.Id);
                    var role = await _unitOfWork.GetRepository<IdentityRole>().GetById(userRole.RoleId);
                    accounts.Add(new AccountResponse { User = account, Role = role });
                }
                _result.Data = accounts;

            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);

            }
            return _result;
        }
    }
}

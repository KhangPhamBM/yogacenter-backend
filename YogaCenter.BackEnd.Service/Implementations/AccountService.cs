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
using System.Web.WebPages;

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
            _tokenDto = new();
        }

        public async Task<AppActionResult> Login(LoginRequestDto loginRequest)
        {
            bool isValid = true;
            try
            {
                if (await _unitOfWork.GetRepository<ApplicationUser>().GetByExpression(u => u.Email.ToLower() == loginRequest.Email.ToLower() && u.isDeleted == false) == null)
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
            bool isValid = true;
            try
            {
                if (await _unitOfWork.GetRepository<ApplicationUser>().GetByExpression(r => r.UserName == signUpRequest.Email) != null)
                {
                    _result.Message.Add("The email or username is existed");
                    isValid = false;

                }
                if (isValid)
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
                    var resultCreateUser = await _userManager.CreateAsync(user, signUpRequest.Password);
                    if (resultCreateUser.Succeeded)
                    {
                        _result.Message.Add($"{SD.ResponseMessage.CREATE_SUCCESSFUL} USER");

                    }
                    else
                    {
                        _result.Message.Add($"{SD.ResponseMessage.CREATE_FAILED} USER");

                    }
                    var resultCreateRole = await _userManager.AddToRoleAsync(user, signUpRequest.RoleName);
                    if (resultCreateRole.Succeeded)
                    {
                        _result.Message.Add($"ASSIGN ROLE SUCCESSFUL");

                    }
                    else
                    {
                        _result.Message.Add($"ASSIGN ROLE FAILED");

                    }
                }

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

                if (await _unitOfWork.GetRepository<ApplicationUser>().GetByExpression(a => a.Id == applicationUser.Id) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The user with id {applicationUser.Id} not found");
                }
                if (isValid)
                {
                    await _unitOfWork.GetRepository<ApplicationUser>().Update(applicationUser);
                    _unitOfWork.SaveChange();
                    _result.Message.Add(SD.ResponseMessage.UPDATE_SUCCESSFUL);
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
        public async Task<AppActionResult> GetAllAccount(int pageIndex, int pageSize, IList<SortInfo> sortInfos)
        {
            try
            {
                List<AccountResponse> accounts = new List<AccountResponse>();
                var list = await _unitOfWork.GetRepository<ApplicationUser>().GetAll();
                foreach (var account in list)
                {
                    var userRole = await _unitOfWork.GetRepository<IdentityUserRole<string>>().GetListByExpression(s => s.UserId == account.Id, null);
                    var listRole = new List<IdentityRole>();
                    foreach (var role in userRole)
                    {
                        var item = await _unitOfWork.GetRepository<IdentityRole>().GetById(role.RoleId);
                        listRole.Add(item);
                    }
                    accounts.Add(new AccountResponse { User = account, Role = listRole });
                }
                var data = accounts.AsQueryable().OrderBy(x => x.User.Id);
                if (sortInfos != null)
                {
                    data = DataPresentationHelper.ApplySorting(data, sortInfos);
                }
                if (pageIndex > 0 && pageSize > 0)
                {
                    data = DataPresentationHelper.ApplyPaging(data, pageIndex, pageSize);
                }
                _result.Data = data;
            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }
        public async Task<AppActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            bool isValid = true;
            try
            {
                if (await _unitOfWork.GetRepository<ApplicationUser>().GetByExpression(c => c.Email == changePasswordDto.Email && c.isDeleted == false) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The user with email {changePasswordDto.Email} not found");
                }
                if (isValid)
                {
                    var user = await _unitOfWork.GetRepository<ApplicationUser>().GetByExpression(c => c.Email == changePasswordDto.Email && c.isDeleted == false);
                    var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.OldPassword, changePasswordDto.NewPassword);
                    if (result.Succeeded)
                    {
                        _result.Message.Add(SD.ResponseMessage.UPDATE_SUCCESSFUL);
                    }
                    else
                    {
                        _result.Message.Add(SD.ResponseMessage.CREATE_FAILED);
                    }
                }
            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }
        public async Task<AppActionResult> SearchApplyingSortingAndFiltering(BaseFilterRequest filterRequest)
        {
            try
            {
                var source = (IOrderedQueryable<ApplicationUser>)await _unitOfWork.GetRepository<ApplicationUser>().GetByExpression(a => (bool)a.isDeleted, null);
                if (filterRequest != null)
                {
                    if (filterRequest.pageIndex <= 0 || filterRequest.pageSize <= 0)
                    {
                        _result.Message.Add($"Invalid value of pageIndex or pageSize");
                        _result.isSuccess = false;
                    }
                    else
                    {
                        if (!filterRequest.keyword.IsEmpty())
                        {
                            source = (IOrderedQueryable<ApplicationUser>)await _unitOfWork.GetRepository<ApplicationUser>().GetByExpression(c => (bool)!c.isDeleted && c.UserName.Contains(filterRequest.keyword), null);
                        }
                        if (filterRequest.filterInfoList != null)
                        {
                            source = DataPresentationHelper.ApplyFiltering(source, filterRequest.filterInfoList);
                        }

                        if (filterRequest.sortInfoList != null)
                        {
                            source = DataPresentationHelper.ApplySorting(source, filterRequest.sortInfoList);
                        }
                        source = DataPresentationHelper.ApplyPaging(source, filterRequest.pageIndex, filterRequest.pageSize);
                        _result.Data = source;
                    }
                }
                else
                {
                    _result.Data = source;
                }
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Models;

namespace YogaCenter.BackEnd.Service.Contracts
{
    public interface IAccountService : ISearching<ApplicationUser>
    {
        Task<AppActionResult> Login(LoginRequestDto loginRequest);
        public Task<AppActionResult> VerifyLoginGoogle(string email, string verifyCode);

        Task<AppActionResult> CreateAccount(SignUpRequestDto signUpRequest);
        Task<AppActionResult> UpdateAccount(ApplicationUser applicationUser);
        Task<AppActionResult> ChangePassword(ChangePasswordDto changePasswordDto);
        Task<AppActionResult> GetAccountByUserId(string id);
        Task<AppActionResult> GetAllAccount(int pageIndex, int pageSize, IList<SortInfo> sortInfos);
        Task<AppActionResult> SearchApplyingSortingAndFiltering(BaseFilterRequest filterRequest);
        Task<AppActionResult> AssignRoleForUserId(string userId, IList<string> roleId);
        Task<AppActionResult> RemoveRoleForUserId(string userId, IList<string> roleId);
        Task<AppActionResult> GetNewToken(string refreshToken, string userId);
        Task<AppActionResult> ForgotPassword(ForgotPasswordDto dto);
        Task<AppActionResult> ActiveAccount(string email, string verifyCode);
        Task<AppActionResult> SendEmailForgotPassword(string email);
        Task<string> GenerateVerifyCode(string email);
        Task<string> GenerateVerifyCodeGoogle(string email);


    }
}

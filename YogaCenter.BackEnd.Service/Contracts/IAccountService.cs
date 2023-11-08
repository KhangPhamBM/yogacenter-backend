using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Models;

namespace YogaCenter.BackEnd.Service.Contracts
{
    public interface IAccountService: ISearching<ApplicationUser>
    {
        Task<AppActionResult> Login(LoginRequestDto loginRequest);
        Task<AppActionResult> CreateAccount(SignUpRequestDto signUpRequest);
        Task<AppActionResult> UpdateAccount(ApplicationUser applicationUser);
        Task<AppActionResult> ChangePassword(ChangePasswordDto changePasswordDto);
        Task<AppActionResult> GetAccountByUserId(string id);
        Task<AppActionResult> GetAllAccount(int pageIndex, int pageSize, IList<SortInfo> sortInfos);
        Task<AppActionResult> SearchApplyingSortingAndFiltering(BaseFilterRequest filterRequest);

        Task<AppActionResult> AssignRoleForUserId(string userId, IList<string> roleId);
        Task<AppActionResult> RemoveRoleForUserId(string userId, IList<string> roleId);

        Task<AppActionResult> GetAllRole();

    }
}

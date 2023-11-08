using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.DAL.Util;
using YogaCenter.BackEnd.Service.Contracts;

namespace YogaCenter.BackEnd.API.Controllers
{
    [Route("account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("create-account")]

        public async Task<AppActionResult> CreateAccount(SignUpRequestDto request)
        {
            return await _accountService.CreateAccount(request);

        }

        [HttpPost("login")]
        public async Task<AppActionResult> Login(LoginRequestDto request)
        {
            return await _accountService.Login(request);

        }

        [HttpPut("update-account")]
        [Authorize(Roles = Permission.ALL)]

        public async Task<AppActionResult> UpdateAccount(ApplicationUser request)
        {
            return await _accountService.UpdateAccount(request);

        }
        [HttpGet("get-account-by-id/{id}")]
        [Authorize(Roles = Permission.ALL)]

        public async Task<AppActionResult> UpdateAccount(string id)
        {
            return await _accountService.GetAccountByUserId(id);

        }

        [HttpPost("get-all-account")]
        [Authorize(Roles = Permission.MANAGEMENT)]

        public async Task<AppActionResult> GetAllAccount(int pageIndex, int pageSize, IList<SortInfo> sortInfos)
        {
            return await _accountService.GetAllAccount(pageIndex, pageSize, sortInfos);
        }
        [HttpPut("change-password")]
        [Authorize(Roles = Permission.ALL)]

        public async Task<AppActionResult> ChangePassword(ChangePasswordDto dto)
        {
            return await _accountService.ChangePassword(dto);
        }

        [HttpPost("get-accounts-with-searching")]
        [Authorize(Roles = Permission.MANAGEMENT)]
        public async Task<AppActionResult> GetAccountWithSearching( BaseFilterRequest baseFilterRequest)
        {
            return await _accountService.SearchApplyingSortingAndFiltering(baseFilterRequest);
        }

        [HttpPut("assign-role-for-userId")]
        [Authorize(Roles = Permission.ADMIN)]

        public async Task<AppActionResult> AssignRoleForUserId(string userId, IList<string> roleId)
        {
            return await _accountService.AssignRoleForUserId(userId, roleId);
        }
        [HttpPut("remove-role-for-userId")]
        [Authorize(Roles = Permission.ADMIN)]

        public async Task<AppActionResult> RemoveRoleForUserId(string userId, IList<string> roleId)
        {
            return await _accountService.RemoveRoleForUserId(userId, roleId);
        }


    }
}

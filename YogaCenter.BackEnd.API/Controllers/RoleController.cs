using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Util;
using YogaCenter.BackEnd.Service.Contracts;
using static YogaCenter.BackEnd.DAL.Util.SD;

namespace YogaCenter.BackEnd.API.Controllers
{
    [Route("role")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

       
        [HttpGet("get-all-role")]
        public async Task<AppActionResult> GetAllRole()
        {
            return await _roleService.GetAllRole();
        }

        [HttpPost("assign-role-for-user")]
        [Authorize(Roles = Permission.STAFF)]

        public async Task<AppActionResult> AssignRoleForUser(string userId, string roleName)
        {
            return await _roleService.AssignRoleForUser(userId,roleName);
        }

        [HttpDelete("remove-role-for-user")]
        [Authorize(Roles = Permission.STAFF)]

        public async Task<AppActionResult> RemoveRoleForUser(string userId, string roleName)
        {
            return await _roleService.RemoveRoleForUser(userId, roleName);
        }


    }
}

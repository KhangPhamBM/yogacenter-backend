using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YogaCenter.BackEnd.Common.Dto;
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

        [HttpPost("create-role")]
        public async Task<AppActionResult> CreateRole(string roleName)
        {
            return await _roleService.CreateRole(roleName);
        }

        [HttpPut("update-role")]
        public async Task<AppActionResult> UpdateRole(IdentityRole role)
        {
            return await _roleService.UpdateRole(role);

        }
        [HttpPost("assign-role-for-user")]
        public async Task<AppActionResult> AssignRoleForUser(string userId, string roleName)
        {
            return await _roleService.AssignRoleForUser(userId,roleName);
        }

        [HttpDelete("remove-role-for-user")]
        public async Task<AppActionResult> RemoveRoleForUser(string userId, string roleName)
        {
            return await _roleService.RemoveRoleForUser(userId, roleName);
        }


    }
}

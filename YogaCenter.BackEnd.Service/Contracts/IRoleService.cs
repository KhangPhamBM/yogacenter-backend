using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;

namespace YogaCenter.BackEnd.Service.Contracts
{
    public interface IRoleService
    {
        Task<AppActionResult> CreateRole(string roleName);
        Task<AppActionResult> UpdateRole(IdentityRole role);
        Task<AppActionResult> AssignRoleForUser(string userId, string roleName);
        Task<AppActionResult> RemoveRoleForUser(string userId, string roleName);
        Task<AppActionResult> GetAllRole();

    }
}

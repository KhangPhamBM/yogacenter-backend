﻿




using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto.Request;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.DAL.Util;
using YogaCenter.BackEnd.Service.Contracts;

namespace YogaCenter.BackEnd.Service.Implementations
{
    public class RoleService : GenericBackendService,IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppActionResult _result;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private IIdentityRoleRepository _roleRepository;


        public RoleService(
            IIdentityRoleRepository identityRoleRepository,
            IUnitOfWork unitOfWork,
            RoleManager<IdentityRole> roleManager, 
            UserManager<ApplicationUser> userManager,
            IServiceProvider serviceProvider
            ):base(serviceProvider)
        {
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _result = new();
            _userManager = userManager;
            _roleRepository = identityRoleRepository;
        }

        public async Task<AppActionResult> AssignRoleForUser(string userId, string roleName)
        {
            bool isValid = true;
            try
            {
                var accountRepository = Resolve<IAccountRepository>();
                if (await accountRepository.GetByExpression(u => u.Id == userId && u.IsDeleted == false) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The user with id {userId} not found");
                }
                if (await _roleRepository.GetByExpression(r => r.Name.ToLower() == roleName.ToLower()) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The role with name {roleName} not found");
                }

                if (isValid)
                {
                    var user = await accountRepository.GetById(userId);
                    var result = await _userManager.AddToRoleAsync(user, roleName);
                    if (result.Succeeded)
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

                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> CreateRole(string roleName)
        {
            bool isValid = true;
            try
            {
                if (await _roleRepository.GetByExpression(r => r.Name.ToLower() == roleName.ToLower()) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The role with name {roleName} is existed");
                }
                if (isValid)
                {
                    var role = new IdentityRole();
                    role.Name = roleName;
                    var result = await _roleManager.CreateAsync(role);

                    if (result.Succeeded)
                    {
                        _result.Message.Add($"{SD.ResponseMessage.CREATE_SUCCESSFUL} ROLE");

                    }
                    else
                    {
                        _result.Message.Add($"{SD.ResponseMessage.CREATE_FAILED} ROLE");

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

        public async Task<AppActionResult> GetAllRole()
        {
            try
            {
                _result.Result.Data = await _roleRepository.GetAll();

            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> RemoveRoleForUser(string userId, string roleName)
        {
            bool isValid = true;
            var accountRepository = Resolve<IAccountRepository>();
            try
            {
                if (await accountRepository.GetByExpression(u => u.Id == userId && u.IsDeleted == false) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The user with id {userId} not found");
                }
                if (await _roleRepository.GetByExpression(r => r.Name.ToLower() == roleName.ToLower()) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The role with name {roleName} not found");
                }
                if (isValid)
                {
                    var user = await accountRepository.GetById(userId);
                    var result = await _userManager.RemoveFromRoleAsync(user, roleName);
                    if (result.Succeeded)
                    {
                        _result.Message.Add($"REMOVE ROLE SUCCESSFUL");
                    }
                    else
                    {
                        _result.Message.Add($"REMOVE ROLE FAILED");

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

        public async Task<AppActionResult> UpdateRole(IdentityRole role)
        {
            bool isValid = true;
            try
            {
                if (await _roleRepository.GetByExpression(r => r.Name.ToLower() == role.Name.ToLower()) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The role with name {role.Name} not found");
                }
                if (isValid)
                {
                    var result = await _roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        _result.Message.Add($"{SD.ResponseMessage.UPDATE_SUCCESSFUL} ROLE");

                    }
                    else
                    {
                        _result.Message.Add($"{SD.ResponseMessage.UPDATE_FAILED} ROLE");

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
    }
}

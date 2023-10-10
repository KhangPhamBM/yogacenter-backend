using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Models;

namespace YogaCenter.BackEnd.DAL.Contracts
{
    public interface IAccountRepository: IRepository<ApplicationUser>
    {
        Task <TokenDto> Login(LoginRequestDto loginRequest);
        Task<ApplicationUser>  SignUp(SignUpRequestDto user);
        Task AssignRole(string userId, string roleName);

    }
}

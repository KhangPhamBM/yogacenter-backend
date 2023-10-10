using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Models;

namespace YogaCenter.BackEnd.Service.Contracts
{
    public interface IAccountService
    {
        Task<TokenDto> Login(LoginRequestDto loginRequest);
        Task SignUp(SignUpRequestDto signUpRequest);
        Task UpdateAccount(ApplicationUser applicationUser);
       Task<ApplicationUser> GetAccountByUserId(string id);


    }
}

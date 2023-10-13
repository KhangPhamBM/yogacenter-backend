using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.Service.Contracts;

namespace YogaCenter.BackEnd.Service.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IAccountRepository accountRepository, IUnitOfWork unitOfWork)
        {
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TokenDto> Login(LoginRequestDto loginRequest)
        {
            return await _accountRepository.Login(loginRequest);
        }

        public async Task SignUp(SignUpRequestDto signUpRequest)
        {
            var user = await _accountRepository.SignUp(signUpRequest);
            if (user != null)
            {
                await _accountRepository.AssignRole(user.Id, signUpRequest.RoleName);
                _unitOfWork.SaveChange();

            }
        }
        public Task UpdateAccount(ApplicationUser applicationUser)
        {
            var user = _unitOfWork.GetRepository<ApplicationUser>().GetById(applicationUser);
            if (user != null)
            {
                _unitOfWork.GetRepository<ApplicationUser>().Update(applicationUser);
                _unitOfWork.SaveChange();
            }
            return Task.CompletedTask;
        }

        public Task<ApplicationUser> GetAccountByUserId(string id)
        {
          return _unitOfWork.GetRepository<ApplicationUser>().GetById(id);
        }
    }
}

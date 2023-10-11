using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.Service.Contracts;

namespace YogaCenter.BackEnd.API.Controllers
{
    [Route("account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ResponeDto _responeDto;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
            _responeDto = new ResponeDto();
        }

        [HttpPost("create-account")]
        public async Task<ResponeDto> SignUp(SignUpRequestDto request)
        {
            try
            {
                await _accountService.SignUp(request);
                _responeDto.Data = true;
            }
            catch (Exception ex)
            {
                _responeDto.isSuccess = false;
                _responeDto.Message = ex.Message;
            }
            return _responeDto;
        }

        [HttpPost("login")]
        public async Task<ResponeDto> Login(LoginRequestDto request)
        {
            try
            {
                _responeDto.Data = await _accountService.Login(request);
            }
            catch (Exception ex)
            {
                _responeDto.isSuccess = false;
                _responeDto.Message = ex.Message;
            }
            return _responeDto;
        }

        [HttpPut("update-account")]
        public async Task<ResponeDto> UpdateAccount(ApplicationUser request)
        {
            try
            {
                await _accountService.UpdateAccount(request);
                _responeDto.Data = true;
            }
            catch (Exception ex)
            {
                _responeDto.isSuccess = false;
                _responeDto.Message = ex.Message;
            }
            return _responeDto;
        }
        [HttpPost("get-account-by-id/{id}")]
        public ResponeDto UpdateAccount(string id)
        {
            try
            {
                _responeDto.Data = _accountService.GetAccountByUserId(id);

            }
            catch (Exception ex)
            {
                _responeDto.isSuccess = false;
                _responeDto.Message = ex.Message;
            }
            return _responeDto;
        }
    }
}

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
        private readonly AppActionResult _responeDto;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
            _responeDto = new AppActionResult();
        }

        [HttpPost("create-account")]
        public async Task<AppActionResult> SignUp(SignUpRequestDto request)
        {
            try
            {
                await _accountService.SignUp(request);
                _responeDto.Data = true;
            }
            catch (Exception ex)
            {
                _responeDto.isSuccess = false;
            }
            return _responeDto;
        }

        [HttpPost("login")]
        public async Task<AppActionResult> Login(LoginRequestDto request)
        {
            try
            {
                _responeDto.Data = await _accountService.Login(request);
            }
            catch (Exception ex)
            {
                _responeDto.isSuccess = false;
            }
            return _responeDto;
        }

        [HttpPut("update-account")]
        public async Task<AppActionResult> UpdateAccount(ApplicationUser request)
        {
            try
            {
                await _accountService.UpdateAccount(request);
                _responeDto.Data = true;
            }
            catch (Exception ex)
            {
                _responeDto.isSuccess = false;
            }
            return _responeDto;
        }
        [HttpPost("get-account-by-id/{id}")]
        public AppActionResult UpdateAccount(string id)
        {
            try
            {
                _responeDto.Data = _accountService.GetAccountByUserId(id);

            }
            catch (Exception ex)
            {
                _responeDto.isSuccess = false;
            }
            return _responeDto;
        }
    }
}

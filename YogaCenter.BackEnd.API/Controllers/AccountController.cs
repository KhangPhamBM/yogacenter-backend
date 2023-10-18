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
        public async Task<AppActionResult> UpdateAccount(ApplicationUser request)
        {
            return await _accountService.UpdateAccount(request);

        }
        [HttpGet("get-account-by-id/{id}")]
        public async Task<AppActionResult> UpdateAccount(string id)
        {
            return await _accountService.GetAccountByUserId(id);

        }

        [HttpGet("get-all-account")]
        public async Task<AppActionResult> GetAllAccount()
        {
            return await _accountService.GetAllAccount();
        }
        [HttpPut("change-password")]
        public async Task<AppActionResult> ChangePassword(ChangePasswordDto dto)
        {
            return await _accountService.ChangePassword(dto);
        }
    }
}

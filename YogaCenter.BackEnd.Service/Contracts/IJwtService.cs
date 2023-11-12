using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;

namespace YogaCenter.BackEnd.Service.Contracts
{
    public interface IJwtService
    {
        string GenerateRefreshToken();
        Task<string> GenerateAccessToken(LoginRequestDto loginRequest);
        Task<TokenDto> GetNewToken(string refreshToken, string accountId);


    }
}

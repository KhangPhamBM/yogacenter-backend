using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.Service.Contracts;
using YogaCenter.BackEnd.Service.Implementations;

namespace YogaCenter.BackEnd.API.Controllers
{
    [Route("class-detail")]
    [ApiController]
    public class ClassDetailController : ControllerBase
    {
        public ResponeDto _responeDto;
        public IClassDetailService _classDetail;

        public ClassDetailController(IClassDetailService classDetail)
        {
            _classDetail = classDetail;
            _responeDto = new ResponeDto();
        }
        [HttpPost("register-class")]
        public async Task<ResponeDto> RegisterClass(ClassDetailDto classDto)
        {
            try
            {
                await _classDetail.RegisterClass(classDto);
                _responeDto.Data = true;

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

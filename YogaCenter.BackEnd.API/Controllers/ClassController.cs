using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.Service.Contracts;

namespace YogaCenter.BackEnd.API.Controllers
{
    [Route("class")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        public readonly IClassService _classService;
        public ResponeDto _responeDto;
        public ClassController(IClassService classService)
        {
            _classService = classService;
            _responeDto = new ResponeDto();
        }

       
        [HttpPut("update-class")]
        public async Task<ResponeDto> UpdateClass(ClassDto classDto)
        {
            try
            {
                await _classService.UpdateClass(classDto);
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

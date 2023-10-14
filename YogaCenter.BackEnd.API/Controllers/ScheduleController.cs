using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.Service.Contracts;

namespace YogaCenter.BackEnd.API.Controllers
{
    [Route("schedule")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;
        private readonly ResponeDto _responeDto;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
            _responeDto = new ResponeDto();
        }
        [HttpGet("get-schedule-by-classId/{classId}")]
        public async Task<ResponeDto> GetScheduleByClassId(int classId)
        {
            try
            {

                _responeDto.Data = await _scheduleService.GetScheduleByClassId(classId);
            }
            catch (Exception ex)
            {
                _responeDto.Message = ex.Message;
                _responeDto.isSuccess = false;

            }
            return _responeDto;
        }
        [HttpPost("register-schedule-for-class")]
        public async Task<ResponeDto> RegisterSchedulesForClass(IEnumerable<ScheduleDto> scheduleListDto, int classId)
        {
            try
            {

                await _scheduleService.RegisterSchedulesForClass(scheduleListDto, classId);
                _responeDto.Data = true;
            }
            catch (Exception ex)
            {
                _responeDto.Message = ex.Message;
                _responeDto.isSuccess = false;

            }
            return _responeDto;
        }
        [HttpGet("get-schedule-by-userId/{userId}")]

        public async Task<ResponeDto> GetSchedulesByUserId(string UserId)
        {

            try
            {

                _responeDto.Data = await _scheduleService.GetSchedulesByUserId(UserId);

            }
            catch (Exception ex)
            {
                _responeDto.Message = ex.Message;
                _responeDto.isSuccess = false;

            }
            return _responeDto;
        }
    }
}

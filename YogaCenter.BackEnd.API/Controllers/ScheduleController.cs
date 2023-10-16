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
        private readonly AppActionResult _responeDto;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
            _responeDto = new AppActionResult();
        }
        [HttpGet("get-schedule-by-classId/{classId}")]
        public async Task<AppActionResult> GetScheduleByClassId(int classId)
        {
            try
            {

                _responeDto.Data = await _scheduleService.GetScheduleByClassId(classId);
            }
            catch (Exception ex)
            {
                _responeDto.isSuccess = false;

            }
            return _responeDto;
        }
        [HttpPost("register-schedule-for-class")]
        public async Task<AppActionResult> RegisterSchedulesForClass(IEnumerable<ScheduleDto> scheduleListDto, int classId)
        {
            try
            {

                await _scheduleService.RegisterSchedulesForClass(scheduleListDto, classId);
                _responeDto.Data = true;
            }
            catch (Exception ex)
            {
                _responeDto.isSuccess = false;

            }
            return _responeDto;
        }
        [HttpGet("get-schedule-by-userId/{userId}")]

        public async Task<AppActionResult> GetSchedulesByUserId(string UserId)
        {

            try
            {

                _responeDto.Data = await _scheduleService.GetSchedulesByUserId(UserId);

            }
            catch (Exception ex)
            {
                _responeDto.isSuccess = false;

            }
            return _responeDto;
        }
    }
}

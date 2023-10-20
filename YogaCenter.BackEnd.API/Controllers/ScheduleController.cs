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
           return await _scheduleService.GetScheduleByClassId(classId);    
        }
       
        [HttpGet("get-schedule-by-userId/{userId}")]

        public async Task<AppActionResult> GetSchedulesByUserId(string UserId)
        {
            return await _scheduleService.GetSchedulesByUserId(UserId);
        }

        [HttpPut("update-schedule")]

        public async Task<AppActionResult> UpdateSchedule(ScheduleDto scheduleDto)
        {
            return await _scheduleService.UpdateSchedule(scheduleDto);
        }

        [HttpPost("generate-schedule-for-class")]

        public async Task<AppActionResult> GenerateScheduleForClass(CreateScheduleRequest scheduleDto)
        {
            return await _scheduleService.GenerateScheduleForClass(scheduleDto);
        }
    }
}

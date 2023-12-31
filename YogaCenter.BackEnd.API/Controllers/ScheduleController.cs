﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YogaCenter.BackEnd.Common.Dto.Common;
using YogaCenter.BackEnd.Common.Dto.Request;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.DAL.Util;
using YogaCenter.BackEnd.Service.Contracts;

namespace YogaCenter.BackEnd.API.Controllers
{
    [Route("schedule")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;
        

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }
        [HttpGet("get-schedule-by-classId/{classId:int}")]
        public async Task<AppActionResult> GetScheduleByClassId(int classId)
        {
           return await _scheduleService.GetScheduleByClassId(classId);    
        }
       
        [HttpGet("get-schedule-by-userId/{userId}")]

        public async Task<AppActionResult> GetSchedulesByUserId(string userId)
        {
            return await _scheduleService.GetSchedulesByUserId(userId);
        }

        [HttpPut("update-schedule")]
        [Authorize(Roles = Permission.MANAGEMENT)]

        public async Task<AppActionResult> UpdateSchedule(ScheduleDto scheduleDto)
        {
            return await _scheduleService.UpdateSchedule(scheduleDto);
        }

        [HttpPost("generate-schedule-for-class")]
        //[Authorize(Roles = Permission.STAFF)]

        public async Task<AppActionResult> GenerateScheduleForClass(CreateScheduleRequest scheduleDto)
        {
            return await _scheduleService.GenerateScheduleForClass(scheduleDto);
        }


        [HttpGet("get-schedule-of-class-by-date")]
        public async Task<AppActionResult> GetScheduleOfClassByDate(int classId, DateTime date)
        {
            return await _scheduleService.GetScheduleOfClassByDate(classId, date);           
        }

        [HttpGet("get-schedule-of-class-by-week")]
        public async Task<AppActionResult> GetScheduleOfClassByWeek(int classId, int week, int year)
        {
            return await _scheduleService.GetScheduleOfClassByWeek(classId, week, year);
        }

        [HttpGet("get-schedule-of-class-by-month")]
        public async Task<AppActionResult> GetScheduleOfClassByMonth(int classId, int month, int year)
        {
            return await _scheduleService.GetScheduleOfClassByMonth(classId, month, year);
        }


        [HttpGet("get-schedules-by-date")]
        public async Task<AppActionResult> GetScheduleByDate(DateTime date)
        {
            return await _scheduleService.GetSchedulesByDate(date);
        }

        [HttpGet("get-schedules-by-week")]
        public async Task<AppActionResult> GetSchedulesByWeek(int week, int year)
        {
            return await _scheduleService.GetSchedulesByWeek( week, year);
        }

        [HttpGet("get-schedules-by-month")]
        public async Task<AppActionResult> GetSchedulesByMonth(int month, int year)
        {
            return await _scheduleService.GetSchedulesByMonth( month, year);
        }

        [HttpGet("get-week-by-year/{year:int}")]
        public async Task<AppActionResult> GetWeekByYear(int year)
        {
            AppActionResult actionResult = new AppActionResult();
            actionResult.Result.Data = SD.PrintWeeksForYear(year);
            return actionResult;
        }

    }
}

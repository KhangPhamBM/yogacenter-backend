﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.Service.Contracts;

namespace YogaCenter.BackEnd.API.Controllers
{
    [Route("attendance")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        public IAttendanceService _attendanceService;
        public AttendanceController(IAttendanceService attendanceService) {
            _attendanceService = attendanceService;
        }

       
        [HttpPost("add-list-attendance")]
        public async Task<AppActionResult> AddListAttendance([FromBody] IEnumerable<AttendanceDto> attendances)
        {
            return await _attendanceService.AddListAttendance(attendances);
        }

        [HttpPost("update-attendance")]
        public async Task<AppActionResult> UpdateAttendance([FromBody] IEnumerable<AttendanceDto> attendances)
        {
            return await _attendanceService.UpdateAttendance(attendances);
        }

      
        [HttpGet("get-attendances-by-classId/{classId:int}")]
        public async Task<AppActionResult> GetAttendancesByClassId(int classId, int pageIndex, int pageSize, IList<SortInfo> sortInfos)
        {
            return await _attendanceService.GetAttendancesByClassId(classId, pageIndex, pageSize, sortInfos);
        }

        [HttpGet("get-attendances-by-userId/{userId:int}")]
        public async Task<AppActionResult> GetAttendancesByUserId(string userId, int pageIndex, int pageSize, IList<SortInfo> sortInfos)
        {
            return await _attendanceService.GetAttendancesByUserId(userId, pageIndex, pageSize, sortInfos);
        }

        [HttpGet("get-attendances-by-scheduleId/{scheduleId:int}")]
        public async Task<AppActionResult> GetAttendancesByScheduleId(int schduleId, int pageIndex, int pageSize, IList<SortInfo> sortInfos)
        {
            return await _attendanceService.GetAttendancesByScheduleId(schduleId, pageIndex, pageSize, sortInfos);
        }
    }
}

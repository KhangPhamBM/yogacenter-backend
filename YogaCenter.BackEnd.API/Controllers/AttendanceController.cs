using Microsoft.AspNetCore.Http;
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

        /*
         Task<AppActionResult> AddListAttendance(IEnumerable<AttendanceDto> attendances); 
        Task<AppActionResult> UpdateAttendance(IEnumerable<AttendanceDto> attendances);
        Task<AppActionResult> GetAttendancesByScheduleId(int scheduleId);
        Task<AppActionResult> GetAttendancesByClassId(int classId);
        Task<AppActionResult> GetAttendancesByUserId(string userId);
         */
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

        [HttpGet("get-attendances-by-classId")]
        public async Task<AppActionResult> GetAttendancesByClassId(int classId)
        {
            return await _attendanceService.GetAttendancesByClassId(classId);
        }

        [HttpGet("get-attendances-by-userId")]
        public async Task<AppActionResult> GetAttendancesByUserId(string userId)
        {
            return await _attendanceService.GetAttendancesByUserId(userId);
        }

        [HttpGet("get-attendances-by-scheduleId")]
        public async Task<AppActionResult> GetAttendancesByScheduleId(int schduleId)
        {
            return await _attendanceService.GetAttendancesByScheduleId(schduleId);
        }
    }
}

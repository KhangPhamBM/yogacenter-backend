using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.DAL.Util;
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
        [Authorize(Roles = Permission.MANAGEMENT)]
        public async Task<AppActionResult> AddListAttendance([FromBody] IEnumerable<AttendanceDto> attendances)
        {
            return await _attendanceService.AddListAttendance(attendances);
        }

        [HttpPost("update-attendance")]
        [Authorize(Roles = Permission.MANAGEMENT)]

        public async Task<AppActionResult> UpdateAttendance([FromBody] IEnumerable<AttendanceDto> attendances)
        {
            return await _attendanceService.UpdateAttendance(attendances);
        }

      
        [HttpPost("get-attendances-by-classId/{classId:int}")]
        [Authorize(Roles = Permission.CLASS)]

        public async Task<AppActionResult> GetAttendancesByClassId(int classId, int pageIndex, int pageSize, IList<SortInfo> sortInfos)
        {
            return await _attendanceService.GetAttendancesByClassId(classId, pageIndex, pageSize, sortInfos);
        }

        [HttpPost("get-attendances-by-userId/{userId}")]
        [Authorize(Roles = Permission.ALL)]

        public async Task<AppActionResult> GetAttendancesByUserId(string userId, int pageIndex, int pageSize, IList<SortInfo> sortInfos)
        {
            return await _attendanceService.GetAttendancesByUserId(userId, pageIndex, pageSize, sortInfos);
        }

        [HttpPost("get-attendances-by-scheduleId/{scheduleId:int}")]
        [Authorize(Roles = Permission.CLASS)]

        public async Task<AppActionResult> GetAttendancesByScheduleId(int scheduleId, int pageIndex, int pageSize, IList<SortInfo> sortInfos)
        {
            return await _attendanceService.GetAttendancesByScheduleId(scheduleId, pageIndex, pageSize, sortInfos);
        }
        [HttpPost("export-template")]

        public async Task<IActionResult> ExportData()
        {
            return await _attendanceService.ExportData();
        }

    }
}

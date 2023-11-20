using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto.Common;
using YogaCenter.BackEnd.Common.Dto.Request;
using YogaCenter.BackEnd.DAL.Models;

namespace YogaCenter.BackEnd.Service.Contracts
{
    public interface IAttendanceService 
    {
        Task<AppActionResult> AddListAttendance(IEnumerable<AttendanceDto> attendances); 
        Task<AppActionResult> UpdateAttendance(IEnumerable<AttendanceDto> attendances);
        Task<AppActionResult> GetAttendancesByScheduleId(int scheduleId, int pageIndex, int pageSize, IList<SortInfo> sortInfos);
        Task<AppActionResult> GetAttendancesByClassId(int classId, int pageIndex, int pageSize, IList<SortInfo> sortInfos);
        Task<AppActionResult> GetAttendancesByUserId(string userId, int pageIndex, int pageSize, IList<SortInfo> sortInfos);
        public  Task<IActionResult> ExportData();


    }
}

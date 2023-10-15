using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Models;

namespace YogaCenter.BackEnd.Service.Contracts
{
    public interface IAttendanceService
    {
        Task AddListAttendance(IEnumerable<AttendanceDto> attendances); 
        Task UpdateAttend(IEnumerable<AttendanceDto> attendances);
        Task<IEnumerable<Attendance>> GetAttendancesByScheduleId(int scheduleId);
        Task<IEnumerable<Attendance>> GetAttendancesByClassId(int classId);
        Task<IEnumerable<Attendance>> GetAttendancesByUserId(string userId);

    }
}

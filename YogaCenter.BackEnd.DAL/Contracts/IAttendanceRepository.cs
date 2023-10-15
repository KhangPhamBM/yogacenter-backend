using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.DAL.Models;

namespace YogaCenter.BackEnd.DAL.Contracts
{
    public interface IAttendanceRepository:IRepository<Attendance>
    {
        Task<IEnumerable<Attendance>> GetAttendancesByScheduleId(int scheduleId);
        Task<Dictionary<int, IEnumerable<Attendance>>> GetAttendancesByClassId(int classId);
        Task<Dictionary<int, IEnumerable<Attendance>>> GetAttendancesByUserId(string userId);
    }
}

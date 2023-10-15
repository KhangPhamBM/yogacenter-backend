using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Data;
using YogaCenter.BackEnd.DAL.Models;

namespace YogaCenter.BackEnd.DAL.Implementations
{
    public class AttendanceRepository : Repository<Attendance>, IAttendanceRepository
    {
        private readonly YogaCenterContext _context;
        public AttendanceRepository(YogaCenterContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Dictionary<int, IEnumerable<Attendance>>> GetAttendancesByClassId(int classId)
        {
            var scheduleList = _context.Schedules.Where(s => s.ClassId == classId).ToList();
            if (scheduleList.Any())
            {
                var attandenceList = new Dictionary<int, IEnumerable<Attendance>>();
                foreach (var schedule in scheduleList)
                {
                    var attendanceOfEachSchedule = _context.Attendances.Where(a => a.ScheduleId == schedule.ScheduleId).ToList();
                    attandenceList.Add((int)schedule.ScheduleId, attendanceOfEachSchedule);
                }
                return attandenceList;
            }
            return new Dictionary<int, IEnumerable<Attendance>>();
        }

        public async Task<IEnumerable<Attendance>> GetAttendancesByScheduleId(int scheduleId)
        {
            return await _context.Attendances.Where(a => a.ScheduleId == scheduleId).ToListAsync();
        }

        public async Task<Dictionary<int, IEnumerable<Attendance>>> GetAttendancesByUserId(string userId)
        {
           var classDetails = _context.ClassDetails.Where(cd => cd.UserId == userId).ToList();
           if(classDetails.Any()) {
                var attandenceList = new Dictionary<int, IEnumerable<Attendance>>();
                foreach (var classDetail in classDetails)
                {
                    var attendanceOfEachClass = _context.Attendances.Where(a => a.ClassDetailId == a.ClassDetailId).ToList();
                    attandenceList.Add((int)classDetail.ClassId, attendanceOfEachClass);
                }
                return attandenceList;
           }
            return new Dictionary<int, IEnumerable<Attendance>>();
        }
    }
}

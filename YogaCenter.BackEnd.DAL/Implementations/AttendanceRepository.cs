using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
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
            var attendances = new Dictionary<int, IEnumerable<Attendance>>();
            if(attendances.Any())
            {
                foreach (var schedule in scheduleList)
                {
                    var attendance = _context.Attendances.Where(a => a.ScheduleId == schedule.ScheduleId).ToList();
                    attendances.Add((int)schedule.ClassId, attendance);
                }
            }
            return attendances;
        }
    }
}

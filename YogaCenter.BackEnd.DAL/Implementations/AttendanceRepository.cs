using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
        private IServiceProvider _serviceProvider;
        public AttendanceRepository(YogaCenterContext context,  IServiceProvider serviceProvider) : base(context)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<Dictionary<int, IEnumerable<Attendance>>> GetAttendancesByClassId(int classId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var scheduleRepository = scope.ServiceProvider.GetRequiredService<IScheduleRepository>();
                var scheduleList = await scheduleRepository.GetListByExpression(s => s.ClassId == classId);
                var attendances = new Dictionary<int, IEnumerable<Attendance>>();
                if (scheduleList.Any())
                {
                    foreach (var schedule in scheduleList)
                    {
                        var attendance = await this.GetListByExpression(a => a.ScheduleId == schedule.ScheduleId);
                        attendances.Add((int)schedule.ClassId, (IEnumerable<Attendance>)attendance);
                    }
                }
                return attendances;
            }

        }

        public async Task<IEnumerable<Attendance>> GetAttendancesByScheduleId(int scheduleId)
        {
            return await this.GetListByExpression(a => a.ScheduleId == scheduleId);
        }

        public async Task<Dictionary<int, IEnumerable<Attendance>>> GetAttendancesByUserId(string userId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var classRepository = scope.ServiceProvider.GetRequiredService<IClassDetailRepository>();
                var classDetails = await classRepository.GetListByExpression(cd => cd.UserId == userId);
                if (classDetails.Any())
                {
                    var attandenceList = new Dictionary<int, IEnumerable<Attendance>>();
                    foreach (var classDetail in classDetails)
                    {
                        var attendanceOfEachClass = await this.GetListByExpression(a => a.ClassDetailId == a.ClassDetailId);
                    }
                    return attandenceList;
                }
                return new Dictionary<int, IEnumerable<Attendance>>();
            }
        }
    }
}

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
        private readonly IUnitOfWork _unitOfWork;
        public AttendanceRepository(YogaCenterContext context, IUnitOfWork unitOfWork) : base(context)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Dictionary<int, IEnumerable<Attendance>>> GetAttendancesByClassId(int classId)
        {
            var scheduleList = _unitOfWork.GetRepository<Schedule>().GetListByExpression(s => s.ClassId == classId);
            var attendances = new Dictionary<int, IEnumerable<Attendance>>();
            if (scheduleList.Result.Any())
            {
                foreach (var schedule in scheduleList.Result)
                {
                    var attendance = _unitOfWork.GetRepository<Attendance>().GetListByExpression(a => a.ScheduleId == schedule.ScheduleId);
                    attendances.Add((int)schedule.ClassId, (IEnumerable<Attendance>)await attendance);
                }
            }
            return attendances;
        }

        public async Task<IEnumerable<Attendance>> GetAttendancesByScheduleId(int scheduleId)
        {
            return await _unitOfWork.GetRepository<Attendance>().GetListByExpression(a => a.ScheduleId == scheduleId);
        }

        public async Task<Dictionary<int, IEnumerable<Attendance>>> GetAttendancesByUserId(string userId)
        {
           var classDetails = await _unitOfWork.GetRepository<ClassDetail>().GetListByExpression(cd => cd.UserId == userId);
           if(classDetails.Any()) {
                var attandenceList = new Dictionary<int, IEnumerable<Attendance>>();
                foreach (var classDetail in classDetails)
                {
                    var attendanceOfEachClass = _unitOfWork.GetRepository<Attendance>().GetListByExpression(a => a.ClassDetailId == a.ClassDetailId);
                //    attandenceList.Add((int)classDetail.ClassId, attendanceOfEachClass);
                }
                return attandenceList;
           }
            return new Dictionary<int, IEnumerable<Attendance>>();
       }
    }
}

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
        private readonly IUnitOfWork _unitOfWork;
        public AttendanceRepository(YogaCenterContext context, IUnitOfWork unitOfWork) : base(context)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Dictionary<int, IEnumerable<Attendance>>> GetAttendancesByClassId(int classId)
        {
            var scheduleList = await _unitOfWork.GetRepository<Class>().GetListByExpression(s => s.ClassId == classId);
            if (scheduleList.Any())
            {
                var attandenceList = new Dictionary<int, IEnumerable<Attendance>>();
                foreach (var schedule in scheduleList)
                {
                    //var attendanceOfEachSchedule = await _unitOfWork.GetRepository<Attendance>().GetListByExpression(a => a.ScheduleId == schedule.ScheduleId;
                    //attandenceList.Add((int)schedule.ScheduleId, attendanceOfEachSchedule);
                }
                return attandenceList;
            }
            return new Dictionary<int, IEnumerable<Attendance>>();
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

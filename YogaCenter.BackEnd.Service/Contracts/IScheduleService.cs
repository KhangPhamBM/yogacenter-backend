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
    public interface IScheduleService
    {
        Task<AppActionResult> GetScheduleByClassId(int id);
        Task<AppActionResult> GetSchedulesByUserId(string UserId);
        Task<AppActionResult> UpdateSchedule(ScheduleDto scheduleDto);
        Task<AppActionResult> GenerateScheduleForClass(CreateScheduleRequest request);
        Task<AppActionResult> GetScheduleOfClassByDate(int classId,  DateTime date);
        Task<AppActionResult> GetScheduleOfClassByWeek(int classId, int week, int year);
        Task<AppActionResult> GetScheduleOfClassByMonth(int classId, int month, int year);


        Task<AppActionResult> GetSchedulesByDate( DateTime date);
        Task<AppActionResult> GetSchedulesByWeek(int week, int year);
        Task<AppActionResult> GetSchedulesByMonth( int month, int year);
    }
}

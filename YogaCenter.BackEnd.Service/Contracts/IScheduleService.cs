using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Models;

namespace YogaCenter.BackEnd.Service.Contracts
{
    public interface IScheduleService
    {
        Task<AppActionResult> GetScheduleByClassId(int id);
        Task<AppActionResult> GetSchedulesByUserId(string UserId);
        Task<AppActionResult> UpdateSchedule(ScheduleDto scheduleDto);

        Task<AppActionResult> GenerateScheduleForClass(CreateScheduleRequest request);
    }
}

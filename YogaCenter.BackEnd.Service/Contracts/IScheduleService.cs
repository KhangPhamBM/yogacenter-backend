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
        Task<IEnumerable<Schedule>> GetScheduleByClassId(int id);
        Task RegisterSchedulesForClass(IEnumerable<ScheduleDto> scheduleListDto, int classId);
        Task<IEnumerable<Schedule>> GetSchedulesByUserId(string UserId);


    }
}

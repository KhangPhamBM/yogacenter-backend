using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto.Response;
using YogaCenter.BackEnd.DAL.Models;

namespace YogaCenter.BackEnd.DAL.Contracts
{
    public interface IScheduleRepository: IRepository<Schedule>
    {
        Task<IEnumerable<Schedule>> GetSchedulesByClassId(int classId); 
        Task<Schedule> GetSchedule(ScheduleOfClassDto scheduleDto);
    }
}

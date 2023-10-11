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
    public class ScheduleRepository : Repository<Schedule>, IScheduleRepository
    {
        private readonly YogaCenterContext _db;
        public ScheduleRepository(YogaCenterContext context) : base(context)
        {
            _db = context;
        }

        public async Task<Schedule> GetSchedule(ScheduleDto scheduleDto)
        {
          return await _db.Schedules.SingleOrDefaultAsync(s=> s.Date == scheduleDto.Date && s.TimeFrameId == scheduleDto.TimeFrameId && s.ClassId == scheduleDto.ClassId && s.ClassId == scheduleDto.ClassId);
        }

       

        public async Task<IEnumerable<Schedule>> GetSchedulesByClassId(int classId)
        {
            return await _db.Schedules.Where(s=> s.ClassId == classId).ToListAsync();
        }

      
    }
}

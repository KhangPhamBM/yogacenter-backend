using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.DAL.Data;

namespace YogaCenter.BackEnd.DAL.Implementations
{
    public class ScheduleRepository : Repository<ScheduleRepository>
    {
        public ScheduleRepository(YogaCenterContext context) : base(context)
        {
        }
    }
}

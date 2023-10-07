using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.DAL.Data;

namespace YogaCenter.BackEnd.DAL.Implementations
{
    public class TimeFrameRepository : Repository<TimeFrameRepository>
    {
        public TimeFrameRepository(YogaCenterContext context) : base(context)
        {
        }
    }
}

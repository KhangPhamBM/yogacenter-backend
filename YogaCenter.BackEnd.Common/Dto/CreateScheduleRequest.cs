using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.Common.Dto
{
    public class CreateScheduleRequest
    {
        public int ClassId { get; set; }
        public IEnumerable<ScheduleRequest> Schedules { get; set; }

       
        public class ScheduleRequest
        {
            public int TimeFrameId { get; set; }
            public DayOfWeek DayOfWeek { get; set; }
            public int RoomId { get; set; }

        }
    }
}

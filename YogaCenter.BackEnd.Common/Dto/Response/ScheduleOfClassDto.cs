using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto.Common;

namespace YogaCenter.BackEnd.Common.Dto.Response
{
    public class ScheduleOfClassDto
    {
        public int ScheduleId { get; set; }
        public int? ClassId { get; set; }
        public int? TimeFrameId { get; set; }
        public TimeFrameDto? TimeFrameDto { get; set; }
        public int? RoomId { get; set; }
        public RoomDto? RoomDto { get; set; }
        public DateTime Date { get; set; }
    }

}

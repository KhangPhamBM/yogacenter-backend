using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.Common.Dto
{
    public class ScheduleDto
    {
        public int ScheduleId { get; set; }
        public int? ClassId { get; set; }
        public int? TimeFrameId { get; set; }

        public int? RoomId { get; set; }

        public DateTime Date { get; set; }
    }
}

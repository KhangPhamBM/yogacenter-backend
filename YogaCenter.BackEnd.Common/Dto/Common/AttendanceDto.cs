using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.Common.Dto.Common
{
    public class AttendanceDto
    {
        public int ClassDetailId { get; set; }

        public int ScheduleId { get; set; }
        public int AttendanceStatusId { get; set; } = 0;

    }
}

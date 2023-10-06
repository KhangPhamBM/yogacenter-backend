using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.DAL.Models
{
    public class Attendance
    {
        [Key, Column(Order = 1)]
        public int ClassDetailId { get; set; }
        [ForeignKey("ClassDetailId")]
        public ClassDetail ClassDetail { get; set; }
        [Key, Column(Order = 2)]

        public int ScheduleId { get; set; }
        [ForeignKey("ScheduleId")]
        public Schedule Schedule { get; set; }
        public bool isAttended { get; set; }

    }
}

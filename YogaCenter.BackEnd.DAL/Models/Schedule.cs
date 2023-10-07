using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.DAL.Models
{
    public class Schedule
    {
        [Key]
        public int ScheduleId { get; set; }
        public int? ClassId { get; set; }
        [ForeignKey("ClassId"), Column(Order = 1)]
        public Class? Class { get; set; }
        public int? TimeFrameId { get; set; }
        [ForeignKey("TimeFrameId"), Column(Order = 2)]
        public TimeFrame? TimeFrame { get; set; }

        public int? RoomId { get; set; }
        [ForeignKey("RoomId"),Column(Order =3)]
        public Room? Room { get; set; } 

        public DateTime Date { get; set; }
    }
}

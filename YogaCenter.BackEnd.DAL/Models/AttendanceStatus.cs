using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.DAL.Models
{
    public class AttendanceStatus
    {
        [Key]
        public int AttendanceStatusId { get; set; }
        public string AttendanceStatusName { get; set; }
    }
}

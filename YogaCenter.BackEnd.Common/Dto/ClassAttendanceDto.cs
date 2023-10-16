using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.Common.Dto
{
    public class ClassAttendanceDto
    {
        public ClassDto Class { get; set; }
        public List<AttendanceDto> Attendances { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto.Common;
using YogaCenter.BackEnd.Common.Dto.Request;

namespace YogaCenter.BackEnd.Common.Dto.Response
{
    public class ClassAttendanceResponseDto
    {
        public ClassRequest Class { get; set; }
        public List<AttendanceDto> Attendances { get; set; }
    }
}

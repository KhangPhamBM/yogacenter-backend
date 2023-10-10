using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.Common.Dto
{
    public class TimeFrameDto
    {
        public int TimeFrameId {  get; set; }
        public string? TimeFrameName { get; set; }
    }
}

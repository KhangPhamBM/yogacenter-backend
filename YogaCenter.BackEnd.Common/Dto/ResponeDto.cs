using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.Common.Dto
{
    public class ResponeDto
    {
        public object Data { get; set; }
        public int StatusCode { get; set; } = 200;
        public string Message { get; set; } = "";
    }
}

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
        public bool isSuccess { get; set; } = true;
        public string Message { get; set; } = "";
    }
}

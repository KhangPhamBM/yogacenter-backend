using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.Common.Dto
{
    public class AppActionResult
    {
        public object Data { get; set; }
        public bool isSuccess { get; set; } = true;
        public List<string?> Message { get; set; } = new List<string?>();
    }
}

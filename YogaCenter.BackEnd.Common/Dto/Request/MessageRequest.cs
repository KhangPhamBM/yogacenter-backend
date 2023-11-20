using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.Common.Dto.Request
{
    public class MessageRequest
    {
        public string UserId { get; set; }
        public int ClassId { get; set; }
        public string Message { get; set; }

    }
}

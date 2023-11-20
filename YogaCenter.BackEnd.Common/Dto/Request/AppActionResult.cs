using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto.Response;

namespace YogaCenter.BackEnd.Common.Dto.Request
{
    public class AppActionResult
    {
        public Result Result { get; set; } = new();

        public bool isSuccess { get; set; } = true;
        public List<string?> Message { get; set; } = new List<string?>();

    }
}

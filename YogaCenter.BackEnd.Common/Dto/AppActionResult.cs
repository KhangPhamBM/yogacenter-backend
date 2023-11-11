﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.Common.Dto
{
    public  class AppActionResult
    {
        public Result Result { get; set; } = new();

        public bool isSuccess { get; set; } = true;
        public List<string?> Message { get; set; } = new List<string?>();

    }
}

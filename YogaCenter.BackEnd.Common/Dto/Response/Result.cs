﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.Common.Dto.Response
{
    public class Result
    {
        public object Data { get; set; }
        public int TotalPage { get; set; } = 0;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.Common.Dto
{
    public class FilterInfoToRange : FilterInfo
    {
        public double minValue { get; set; }
        public double maxValue { get; set; }
    }
}

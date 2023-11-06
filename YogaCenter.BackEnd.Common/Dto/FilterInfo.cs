using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.Common.Dto
{
    public class FilterInfo
    {
        public string fieldName { get; set; }
        //public bool isValueFilter { get; set; }
        public double? min { get; set; }
        public double? max { get; set; }
        //public IList<SearchFieldDto>? values { get; set; }
    }
}
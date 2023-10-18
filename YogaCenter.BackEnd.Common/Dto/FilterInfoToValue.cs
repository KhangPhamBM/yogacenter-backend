using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.Common.Dto
{
    public class FilterInfoToValue : FilterInfo
    {
        public List<object> filterValues {  get; set; }
    }
}

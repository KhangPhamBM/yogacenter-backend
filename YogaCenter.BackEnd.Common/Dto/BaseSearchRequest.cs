using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.Common.Dto
{
    public class BaseSearchRequest
    {
        public string searchKeyWord { get; set; }
        public IList<SortInfo>? sortInfoList { get; set; }
        public IList<FilterInfo>? filterInfoList { get; set; }
    }
}

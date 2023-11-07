using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.Common.Dto
{
    public class SortInfo
    {
        public string fieldName { get; set; }
        public bool ascending { get; set; } = true;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto.Request;

namespace YogaCenter.BackEnd.Common.Dto.Response
{
    public class ReportDto
    {
        public int TotalCourse { get; set; }
        public int TotalClass { get; set; }
        public DateTime Date { get; set; }


        public List<ReportMonth> ReportMonths { get; set; } = new List<ReportMonth>();
        public double Total { get; set; }

        public class ReportMonth
        {
            public CourseRequestDto Course { get; set; }
            public IEnumerable<ClassDto> Classes { get; set; } = new List<ClassDto>();
            public double Total { get; set; }
        }

    }
}

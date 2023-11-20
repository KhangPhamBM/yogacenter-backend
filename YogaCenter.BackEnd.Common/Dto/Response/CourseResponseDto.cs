using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.Common.Dto.Response
{
    public class CourseResponseDto
    {
        public int? CourseId { get; set; }
        public string? CourseName { get; set; }
        public string? CourseDescription { get; set; }
        public string? CourseImageUrl { get; set; }
        public double? Price { get; set; }
        public double? Discount { get; set; }

        public bool? IsDeleted { get; set; } = false;
    }
}

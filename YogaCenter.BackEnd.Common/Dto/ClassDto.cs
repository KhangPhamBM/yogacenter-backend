using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.Common.Dto
{
    public class ClassDto
    {
        public int ClassId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? ClassName { get; set; }

        public int? CourseId { get; set; }
        public int MinOfTrainee { get; set; }
        public int MaxOfTrainer { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public IEnumerable<ScheduleOfClassDto > Schedules { get; set; }
    }
}

 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static YogaCenter.BackEnd.DAL.Util.SD;

namespace YogaCenter.BackEnd.DAL.Models
{
    public class Class: IEquatable<Class>
    {
        [Key]
        public int ClassId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? ClassName { get; set; }

        public int? CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course? Course { get; set; }
        public int MinOfTrainee { get; set; }
        public int MaxOfTrainee { get; set; }
        public bool? IsDeleted { get; set; }=false;
        public override bool Equals(object obj)
        {
            return Equals(obj as Class);
        }

        public bool Equals(Class classDto)
        {
            return this.ClassId == classDto.ClassId;
        }

        public override int GetHashCode()
        {
            return (int)(ClassId * CourseId * ClassId % int.MaxValue);
        }
    }
}

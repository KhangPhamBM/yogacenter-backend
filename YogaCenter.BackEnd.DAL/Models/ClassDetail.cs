using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.DAL.Models
{
    public class ClassDetail
    {
        [Key]
        public int ClassDetailId { get; set; }
        public int? ClassId { get; set; }
        [ForeignKey("ClassId"), Column(Order = 1)]
        public Class? Class { get; set; }
        public string? UserId { get; set; }
        [ForeignKey("UserId"), Column(Order = 2)]
        public ApplicationUser? User { get; set; }



    }
}

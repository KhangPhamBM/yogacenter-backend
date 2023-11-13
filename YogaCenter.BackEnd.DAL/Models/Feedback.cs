using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.DAL.Models
{
    public class Feedback
    {
        [Key]
        public int Id { get; set; }
        public int ClassDetailId { get; set; }
        [ForeignKey("ClassDetailId")]
        public ClassDetail ClassDetail { get; set; }
        public string Content { get; set; }
        public int Rating {  get; set; } 
        public int Status { get; set; }
    }
}

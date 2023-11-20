using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.DAL.Models
{
    public class Blog
    {
        [Key]
        public int Id { get; set; }
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string BlogImg { get; set; }= string.Empty;
    }
}

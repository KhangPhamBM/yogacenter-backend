using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.DAL.Models
{
    public class SubcriptionStatus
    {
        [Key]
        public int SubcriptionStatusId { get; set; }
        public string? SubcriptionStatusName { get; set;}
    }
}

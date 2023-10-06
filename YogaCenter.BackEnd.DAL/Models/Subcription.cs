using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.DAL.Models
{
    public class Subcription
    {
        [Key]
        public string SubcriptionId { get; set; }
        public DateTime? SubcriptionDate { get; set; }
        public double? Total { get; set; }
        public int? ClassId { get; set; }
        [ForeignKey("ClassId"), Column(Order = 1)]
        public Class? Class { get; set; }
        public int SubcriptionStatusId { get; set; }
        [ForeignKey("SubcriptionStatusId"), Column(Order = 2)]
        public SubcriptionStatus? SubcriptionStatus { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.Common.Dto
{
    public class SubcriptionDto
    {
        public string SubcriptionId { get; set; }
        public DateTime? SubcriptionDate { get; set; }
        public double? Total { get; set; }
        public int? ClassId { get; set; }
        public int SubcriptionStatusId { get; set; }


    }
}

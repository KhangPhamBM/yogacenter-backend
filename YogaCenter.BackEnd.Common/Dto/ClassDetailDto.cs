using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.Common.Dto
{
    public class ClassDetailDto
    {
        public int ClassDetailId { get; set; }
        public int? ClassId { get; set; }
        public string? UserId { get; set; }



    }
}

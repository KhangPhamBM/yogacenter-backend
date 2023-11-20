using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.Common.Dto.Common
{
    public class PaymentTypeDto
    {
        public int? PaymentTypeId { get; set; }
        public string? Type { get; set; }
    }
}

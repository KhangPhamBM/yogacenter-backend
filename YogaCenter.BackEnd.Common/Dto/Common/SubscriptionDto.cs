using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.Common.Dto.Common
{
    public class SubscriptionDto
    {
        public string SubscriptionId { get; set; }
        public DateTime? SubscriptionDate { get; set; }
        public double? Total { get; set; }
        public int? ClassId { get; set; }
        public string? UserId { get; set; }
        public int SubscriptionStatusId { get; set; }


    }
}

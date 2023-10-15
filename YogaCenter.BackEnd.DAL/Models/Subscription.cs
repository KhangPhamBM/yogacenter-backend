using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.DAL.Models
{
    public class Subscription
    {
        [Key]
        public string SubscriptionId { get; set; }
        public DateTime? SubscriptionDate { get; set; }
        public double? Total { get; set; }
        public string? UserId { get; set; }
        [ForeignKey("UserId"), Column(Order = 3)]
        public int? ClassId { get; set; }
        [ForeignKey("ClassId"), Column(Order = 1)]
        public Class? Class { get; set; }
        public int SubscriptionStatusId { get; set; }
        [ForeignKey("SubscriptionStatusId"), Column(Order = 2)]
        public SubscriptionStatus? SubscriptionStatus { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.DAL.Models
{
    public class SubscriptionStatus
    {
        [Key]
        public int SubscriptionStatusId { get; set; }
        public string? SubscriptionStatusName { get; set;}
    }
}

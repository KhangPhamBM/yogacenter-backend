using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.Common.Dto
{
    public class SubscriptionRequest
    {
        [Required]
        public SubscriptionDto Subscription { get; set; }
        [Required]
        public int PaymentChoice { get; set; }
    }
}

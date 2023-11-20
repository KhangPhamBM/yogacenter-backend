using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto.Common;

namespace YogaCenter.BackEnd.Common.Dto.Request
{
    public class SubscriptionRequest
    {
        [Required]
        public SubscriptionDto Subscription { get; set; }
        [Required]
        public int PaymentChoice { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.Common.Dto.Common
{
    public class SubscriptionStatusDto
    {
        public int SubscriptionStatusId { get; set; }
        public string? SubscriptionStatusName { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.DAL.Models
{
    public class PaymentRespone
    {
        [Key]
        public string PaymentResponseId { get; set; } = null!;
        public string? SubscriptionId { get; set; } = null!;
        [ForeignKey("SubscriptionId"), Column(Order = 1)]

        public Subscription? Subscription { get; set; } = null!;

        public string? Amount { get; set; }
        public string? OrderInfo { get; set; }
        public bool? Success { get; set; }

        public int? PaymentTypeId { get; set; }
        [ForeignKey("PaymentTypeId"), Column(Order = 2)]
        public PaymentType? PaymentType { get; set; } = null!;

    }
}

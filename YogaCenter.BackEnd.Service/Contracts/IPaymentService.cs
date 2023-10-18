using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.Service.Payment.PaymentRequest;

namespace YogaCenter.BackEnd.Service.Contracts
{
    public interface IPaymentService
    {
        public  string CreatePaymentUrlMomo(SubscriptionDto subscriptionDto);
        public string CreatePaymentUrlVNPay(SubscriptionDto subscriptionDto, HttpContext context);
    }
}

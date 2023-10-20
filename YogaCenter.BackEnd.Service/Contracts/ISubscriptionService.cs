using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;

namespace YogaCenter.BackEnd.Service.Contracts
{
    public interface ISubscriptionService
    {
        Task<AppActionResult> CreateSubscription(SubscriptionRequest Subscription, HttpContext context);
        Task<AppActionResult> GetPaymentUrl(SubscriptionRequest Subscription, HttpContext context);

        Task<AppActionResult> UpdateSubscription(SubscriptionDto Subscription);

        Task<AppActionResult> GetSubscriptionByIdWithPendingStatus(string subcriptionId);
        Task<AppActionResult> UpdateStatusSubscription(string subcriptionId, int status);
    }
}

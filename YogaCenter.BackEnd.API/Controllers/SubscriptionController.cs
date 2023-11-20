using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YogaCenter.BackEnd.Common.Dto.Common;
using YogaCenter.BackEnd.Common.Dto.Request;
using YogaCenter.BackEnd.Service.Contracts;
using YogaCenter.BackEnd.Service.Implementations;

namespace YogaCenter.BackEnd.API.Controllers
{
    [Route("subscription")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpPost("create-subscription")]
        public async Task<AppActionResult> CreateSubscription(SubscriptionRequest request)
        {
            return await _subscriptionService.CreateSubscription(request, HttpContext);

        }
        [HttpPut("update-subscription")]
        public async Task<AppActionResult> UpdateSubscription(SubscriptionDto request)
        {
            return await _subscriptionService.UpdateSubscription(request);
        }

        [HttpPost("get-payment-url")]
        public async Task<AppActionResult> GetPaymentUrl(string subscriptionId, int choice)
        {
            return await _subscriptionService.GetPaymentUrl(subscriptionId,choice, HttpContext);
        }

        [HttpPost]
        [Route("get-subscription-with-searching")]
        public async Task<AppActionResult> GetSubscriptionWithSearching(BaseFilterRequest baseFilterRequest)
        {
            return await _subscriptionService.SearchApplyingSortingAndFiltering(baseFilterRequest);
        }

    }
}

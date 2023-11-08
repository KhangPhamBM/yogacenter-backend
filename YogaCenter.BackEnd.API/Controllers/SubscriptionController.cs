using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.Service.Contracts;
using YogaCenter.BackEnd.Service.Implementations;

namespace YogaCenter.BackEnd.API.Controllers
{
    [Route("subscription")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly AppActionResult _responeDto;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
            _responeDto = new AppActionResult();
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
        public async Task<AppActionResult> GetPaymentUrl(SubscriptionRequest request)
        {
            return await _subscriptionService.GetPaymentUrl(request, HttpContext);
        }

        [HttpPost]
        [Route("get-subscription-with-searching")]
        public async Task<AppActionResult> GetSubscriptionWithSearching(BaseFilterRequest baseFilterRequest)
        {
            return await _subscriptionService.SearchApplyingSortingAndFiltering(baseFilterRequest);
        }

    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.Service.Contracts;

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

        [HttpPost("get-payment-url")]
        public async Task<AppActionResult> GetPaymentUrl(SubscriptionRequest request)
        {
            return await _subscriptionService.GetPaymentUrl(request, HttpContext);

        }


    }
}

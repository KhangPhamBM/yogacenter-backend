using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using YogaCenter.BackEnd.Common.Dto.Common;
using YogaCenter.BackEnd.Common.Dto.Request;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.DAL.Util;
using YogaCenter.BackEnd.Service.Contracts;
using YogaCenter.BackEnd.Service.Payment.PaymentRespone;

namespace YogaCenter.BackEnd.API.Controllers
{
    [Route("payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly IPaymentResponeService _paymentResponeService;
        public PaymentController(ISubscriptionService subscriptionService, IPaymentResponeService paymentResponeService)
        {
            _subscriptionService = subscriptionService;
            _paymentResponeService = paymentResponeService;
        }

        [HttpPost]
        [Route("MomoIpn")]
        public async Task<IActionResult> MomoIPN(MomoResponseDto momo)
        {
            try
            {
                AppActionResult subscription = await _subscriptionService.GetSubscriptionByIdWithPendingStatus(momo.extraData);
                if(subscription.Result != null)
                {
                    if (momo.resultCode == 0 )
                    {
                        PaymentResponseDto dto = new PaymentResponseDto
                        {
                            PaymentResponseId = Convert.ToString(momo.transId),
                            SubscriptionId = momo.extraData,
                            Amount = momo.amount.ToString(),
                            OrderInfo = momo.orderInfo,
                            Success = true,
                            PaymentTypeId = SD.PaymentType.MOMO
                        };
                        await _paymentResponeService.AddPaymentRespone(dto);

                        await _subscriptionService.UpdateStatusSubscription(momo.extraData, SD.Subscription.SUCCESSFUL);

                    }

                }




                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("VNPayIpn")]
        public async Task<IActionResult> VNPayIPN()
        {
            try
            {
                var response = new VNPayResponseDto
                {
                    PaymentMethod = Request.Query["vnp_BankCode"],
                    OrderDescription = Request.Query["vnp_OrderInfo"],
                    OrderId = Request.Query["vnp_TxnRef"],
                    PaymentId = Request.Query["vnp_TransactionNo"],
                    TransactionId = Request.Query["vnp_TransactionNo"],
                    Token = Request.Query["vnp_SecureHash"],
                    VnPayResponseCode = Request.Query["vnp_ResponseCode"],
                    PayDate = Request.Query["vnp_PayDate"],
                    Amount = Request.Query["vnp_Amount"],
                    Success = true
                };

                AppActionResult subscription = await _subscriptionService.GetSubscriptionByIdWithPendingStatus(response.OrderId);


                if (response.VnPayResponseCode == "00" )
                {
                    PaymentResponseDto dto = new PaymentResponseDto
                    {
                        PaymentResponseId = Convert.ToString(response.PaymentId),
                        SubscriptionId = response.OrderId,
                        Amount = response.Amount.ToString(),
                        OrderInfo = response.OrderDescription,
                        Success = true,
                        PaymentTypeId = SD.PaymentType.VNPAY



                    };
                    await _paymentResponeService.AddPaymentRespone(dto);
                    await _subscriptionService.UpdateStatusSubscription(dto.SubscriptionId, SD.Subscription.SUCCESSFUL);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);

            }


        }
    }
}

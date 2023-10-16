using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.DAL.Util;
using YogaCenter.BackEnd.Service.Contracts;
using YogaCenter.BackEnd.Service.Payment.PaymentLibrary;
using YogaCenter.BackEnd.Service.Payment.PaymentRequest;
using Utility = YogaCenter.BackEnd.DAL.Util.Utility;

namespace YogaCenter.BackEnd.Service.Implementations
{
    public class PaymentService : IPaymentService
    {

        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        public PaymentService(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

     

        public async Task<string> CreatePaymentUrlMomo(SubscriptionDto subscriptionDto)
        {
            string connection = "";
            var courseId = _unitOfWork.GetRepository<Class>().GetByExpression(c => c.ClassId == subscriptionDto.ClassId).Result.CourseId;
            if (courseId != null)
            {
                var course = await _unitOfWork.GetRepository<Course>().GetById(courseId);

                PaymentInformationRequest momo = new PaymentInformationRequest
                {
                    AccountID = subscriptionDto.UserId,
                    Amount = (double)(course.Price * (100 - course.Discount) / 100),
                    CustomerName = _unitOfWork.GetRepository<ApplicationUser>().GetById(subscriptionDto.UserId).Result.FirstName,
                    OrderID = subscriptionDto.SubscriptionId
                };


                string endpoint = "https://test-payment.momo.vn/v2/gateway/api/create";
                string partnerCode = "MOMO";
                string accessKey = "F8BBA842ECF85";
                string serectkey = "K951B6PE1waDMi640xX08PD3vg6EkVlz";
                string orderInfo = $"Khach hang: {momo.CustomerName} thanh toan hoa don {momo.OrderID}";
                string redirectUrl = $"{_configuration["Momo:RedirectUrl"]}/{momo.OrderID}";
                string ipnUrl = _configuration["Momo:IPNUrl"];
                //  string ipnUrl = "https://webhook.site/3399b42a-eee3-4e2d-8925-c2f893737de9";

                string requestType = "captureWallet";

                string amount = momo.Amount.ToString();
                string orderId = Guid.NewGuid().ToString();
                string requestId = Guid.NewGuid().ToString();
                string extraData = momo.OrderID.ToString();

                //Before sign HMAC SHA256 signature
                string rawHash = "accessKey=" + accessKey +
                    "&amount=" + amount +
                    "&extraData=" + extraData +
                    "&ipnUrl=" + ipnUrl +
                    "&orderId=" + orderId +
                    "&orderInfo=" + orderInfo +
                    "&partnerCode=" + partnerCode +
                    "&redirectUrl=" + redirectUrl +
                    "&requestId=" + requestId +
                    "&requestType=" + requestType
                    ;

                MomoSecurity crypto = new MomoSecurity();
                //sign signature SHA256
                string signature = crypto.signSHA256(rawHash, serectkey);

                //build body json request
                JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "partnerName", "Test" },
                { "storeId", "MomoTestStore" },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderId },
                { "orderInfo", orderInfo },
                { "redirectUrl", redirectUrl },
                { "ipnUrl", ipnUrl },
                { "lang", "en" },
                { "extraData", extraData },
                { "requestType", requestType },
                { "signature", signature }
                };

                var client = new RestClient();

                var request = new RestRequest(endpoint, Method.Post);
                request.AddJsonBody(message.ToString());
                RestResponse response = client.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    JObject jmessage = JObject.Parse(response.Content);
                    connection = jmessage.GetValue("payUrl").ToString();
                }
            }

            return connection;
        }


        public async Task<string> CreatePaymentUrlVNPay(SubscriptionDto subscriptionDto, HttpContext context)
        {
            var paymentUrl = "";
            var courseId = _unitOfWork.GetRepository<Class>().GetByExpression(c => c.ClassId == subscriptionDto.ClassId).Result.CourseId;
            if (courseId != null)
            {
                var course = await _unitOfWork.GetRepository<Course>().GetById(courseId);
                PaymentInformationRequest model = new PaymentInformationRequest
                {
                    AccountID = subscriptionDto.UserId,
                    Amount = (double)(course.Price *(100 - course.Discount)/100),
                    CustomerName = _unitOfWork.GetRepository<ApplicationUser>().GetById(subscriptionDto.UserId).Result.FirstName,
                    OrderID = subscriptionDto.SubscriptionId
                };
                var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
                var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
                var pay = new VNPayLibrary();
                var urlCallBack = $"{_configuration["Vnpay:ReturnUrl"]}/{model.OrderID}";

                pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
                pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
                pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
                pay.AddRequestData("vnp_Amount", ((int)model.Amount * 100).ToString());
                pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));

                pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
                pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
                pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
                pay.AddRequestData("vnp_OrderInfo", $"Khach hang: {model.CustomerName} thanh toan hoa don {model.OrderID}");
                pay.AddRequestData("vnp_OrderType", "other");

                pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
                pay.AddRequestData("vnp_TxnRef", model.OrderID.ToString());

                 paymentUrl = pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);
            }
          

            return paymentUrl;
        }
    }
}

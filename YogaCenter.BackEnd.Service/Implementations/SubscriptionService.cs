using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Implementations;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.DAL.Util;
using YogaCenter.BackEnd.Service.Contracts;

namespace YogaCenter.BackEnd.Service.Implementations
{
    public class SubscriptionService : ISubscriptionService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private ISubscriptionRepository _subscriptionRepository;
        private IPaymentService _paymentService;
        private readonly AppActionResult _result;
        public SubscriptionService(IUnitOfWork unitOfWork, IMapper mapper, ISubscriptionRepository subscriptionRepository, IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _subscriptionRepository = subscriptionRepository;
            _paymentService = paymentService;
            _result = new AppActionResult();
        }

        public async Task<AppActionResult> CreateSubscription(SubscriptionRequest Subscription, HttpContext context)
        {
            try
            {
                bool isValid = true;
                if (Subscription.PaymentChoice != SD.PaymentType.VNPAY && Subscription.PaymentChoice != SD.PaymentType.MOMO && Subscription.PaymentChoice != 3)
                {
                    _result.Message.Add("The API only support for VNPAY (1) and Momo (2) or add subcription with no payment (3)");
                    isValid = false;
                }

                if (await _unitOfWork.GetRepository<Class>().GetById(Subscription.Subscription.ClassId) == null)
                {

                    _result.Message.Add("The class not found");
                    isValid = false;
                }
                else
                {
                    if (DateTime.Now < _unitOfWork.GetRepository<Class>()
                    .GetById(Subscription.Subscription.ClassId).Result.EndDate
                    &&
                    await _unitOfWork.GetRepository<ClassDetail>()
                    .GetByExpression(c => c.UserId == Subscription.Subscription.UserId && c.ClassId == Subscription.Subscription.ClassId) != null)
                    {
                        _result.Message.Add("This action was blocked because the trainee is studying a class which hasn't ended ");
                        isValid = false;
                    }
                }
                if (await _unitOfWork.GetRepository<ApplicationUser>().GetById(Subscription.Subscription.UserId) == null)
                {

                    _result.Message.Add("The user not found");
                    isValid = false;
                }
                if (await _unitOfWork.GetRepository<SubscriptionStatus>().GetById(Subscription.Subscription.SubscriptionStatusId) == null)
                {

                    _result.Message.Add("The subscription status not found");
                    isValid = false;
                }

                if (isValid)
                {
                    Subscription.Subscription.SubscriptionId = Guid.NewGuid().ToString();
                    var classDb = await _unitOfWork.GetRepository<Class>().GetById(Subscription.Subscription.ClassId);
                    var course = await _unitOfWork.GetRepository<Course>().GetById(classDb.CourseId);
                    double total = 0;
                    total = (double)(course.Price * (100 - course.Discount) / 100);
                    Subscription.Subscription.Total = total;

                    var subscription = await _unitOfWork.GetRepository<Subscription>().Insert(_mapper.Map<Subscription>(Subscription.Subscription));
                    _unitOfWork.SaveChange();

                    switch (Subscription.PaymentChoice)
                    {
                        case 1:
                            try
                            {
                                _result.Data = await _paymentService.CreatePaymentUrlVNPay(_mapper.Map<SubscriptionDto>(subscription), context);

                            }
                            catch (Exception ex)
                            {
                                _result.Message.Add($"{ex.Message}");
                            }
                            break;
                        case 2:
                            try
                            {
                                _result.Data = await _paymentService.CreatePaymentUrlMomo(_mapper.Map<SubscriptionDto>(subscription));

                            }
                            catch (Exception ex)
                            {
                                _result.Message.Add($"{ex.Message}");
                            }
                            break;

                        default:
                            _result.Data = "";
                            break;
                    }
                }
                else
                {
                    _result.isSuccess = false;

                }
            }
            catch (Exception ex)
            {

                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }

            return _result;

        }

        public async Task<AppActionResult> GetPaymentUrl(SubscriptionRequest Subscription, HttpContext context)
        {
            try
            {
                bool isValid = true;
                if (Subscription.PaymentChoice != SD.PaymentType.VNPAY && Subscription.PaymentChoice != SD.PaymentType.MOMO && Subscription.PaymentChoice != 3)
                {
                    _result.Message.Add("The API only support for VNPAY (1) and Momo (2) or add subcription with no payment (3)");
                    isValid = false;
                }

                if (await _unitOfWork.GetRepository<Class>().GetById(Subscription.Subscription.ClassId) == null)
                {

                    _result.Message.Add("The class not found");
                    isValid = false;
                }
                else
                {
                    if (DateTime.Now < _unitOfWork.GetRepository<Class>()
                    .GetById(Subscription.Subscription.ClassId).Result.EndDate
                    &&
                    await _unitOfWork.GetRepository<ClassDetail>()
                    .GetByExpression(c => c.UserId == Subscription.Subscription.UserId && c.ClassId == Subscription.Subscription.ClassId) != null)
                    {
                        _result.Message.Add("This action was blocked because the trainee is studying a class which hasn't ended ");
                        isValid = false;
                    }
                }
                if (await _unitOfWork.GetRepository<ApplicationUser>().GetById(Subscription.Subscription.UserId) == null)
                {

                    _result.Message.Add("The user not found");
                    isValid = false;
                }
                if (await _unitOfWork.GetRepository<SubscriptionStatus>().GetById(Subscription.Subscription.SubscriptionStatusId) == null)
                {

                    _result.Message.Add("The subscription status not found");
                    isValid = false;
                }
                if (await _unitOfWork.GetRepository<Subscription>().GetById(Subscription.Subscription.SubscriptionId) == null)
                {

                    _result.Message.Add($"The subscription with id {Subscription.Subscription.SubscriptionId} not found. Please create subscription");
                    isValid = false;
                }

                if (isValid)
                {
                    var subscription = await _unitOfWork.GetRepository<Subscription>().GetById(Subscription.Subscription.SubscriptionId);

                    switch (Subscription.PaymentChoice)
                    {
                        case 1:
                            try
                            {
                                _result.Data = await _paymentService.CreatePaymentUrlVNPay(_mapper.Map<SubscriptionDto>(subscription), context);

                            }
                            catch (Exception ex)
                            {
                                _result.Message.Add($"{ex.Message}");
                            }
                            break;
                        case 2:
                            try
                            {
                                _result.Data = await _paymentService.CreatePaymentUrlMomo(_mapper.Map<SubscriptionDto>(subscription));

                            }
                            catch (Exception ex)
                            {
                                _result.Message.Add($"{ex.Message}");
                            }
                            break;

                        default:
                            _result.Data = "";
                            break;
                    }
                }
                else
                {
                    _result.isSuccess = false;

                }
            }
            catch (Exception ex)
            {

                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }

            return _result;
        }


        public async Task<AppActionResult> UpdateSubscription(SubscriptionDto Subscription)
        {
            try
            {

                bool isValid = true;
                if (_unitOfWork.GetRepository<Subscription>().GetById(Subscription.SubscriptionId) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The subscription with id {Subscription.SubscriptionId} not found");
                }
                if (isValid)
                {
                    await _unitOfWork.GetRepository<Subscription>().Update(_mapper.Map<Subscription>(Subscription));
                    _unitOfWork.SaveChange();
                    _result.Message.Add(SD.ResponseMessage.UPDATE_SUCCESS);
                }
                else
                {
                    _result.isSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> GetSubscriptionByIdWithPendingStatus(string subcriptionId)
        {
            try
            {

                bool isValid = true;
                if (await _unitOfWork.GetRepository<Subscription>().GetById(subcriptionId) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The subscription with id {subcriptionId} not found");
                }
                if (isValid)
                {
                    _result.Data = await _unitOfWork.GetRepository<Subscription>().GetByExpression(s => s.SubscriptionId == subcriptionId && s.SubscriptionStatusId == SD.Subscription.PENDING);
                }
                else
                {
                    _result.isSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> UpdateStatusSubscription(string subcriptionId, int status)
        {
            try
            {

                bool isValid = true;
                if (await _unitOfWork.GetRepository<Subscription>().GetById(subcriptionId) != null)
                {
                    isValid = false;
                    _result.Message.Add($"The subscription with id {subcriptionId} not found");
                }
                if (status != SD.Subscription.PENDING && status != SD.Subscription.SUCCESSFUL && status != SD.Subscription.FAILED)
                {
                    isValid = false;
                    _result.Message.Add($"The subscription status {status} not found");
                }
                if (isValid)
                {
                    Subscription subscription = await _unitOfWork.GetRepository<Subscription>().GetById(subcriptionId);
                    subscription.SubscriptionStatusId = status;
                    await _unitOfWork.GetRepository<Subscription>().Update(subscription);
                    _unitOfWork.SaveChange();
                    _result.Message.Add(SD.ResponseMessage.UPDATE_SUCCESS);
                }
                else
                {
                    _result.isSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }
    }
}

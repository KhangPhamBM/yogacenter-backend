﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Contracts;
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
                if (Subscription.PaymentChoice != SD.PaymentType.VNPAY && Subscription.PaymentChoice != SD.PaymentType.MOMO)
                {
                    _result.Message.Add("The API support Momo and VNPAY with PaymentChoice (1): VNPAY (2): MoMo");
                    isValid = false;
                }

                if (_unitOfWork.GetRepository<Class>().GetById(Subscription.Subscription.ClassId).Result == null)
                {

                    _result.Message.Add("The class not found");
                    isValid = false;
                }
                else
                {
                    if (DateTime.Now < _unitOfWork.GetRepository<Class>()
                    .GetById(Subscription.Subscription.ClassId)?.Result.EndDate
                    &&
                    await _unitOfWork.GetRepository<ClassDetail>()
                    .GetByExpression(c => c.UserId == Subscription.Subscription.UserId && c.ClassId == Subscription.Subscription.ClassId) != null)
                    {
                        _result.Message.Add("This action was blocked because the trainee is studying a class which hasn't ended ");
                        isValid = false;
                    }
                }
                if (_unitOfWork.GetRepository<ApplicationUser>().GetById(Subscription.Subscription.UserId).Result == null)
                {

                    _result.Message.Add("The user not found");
                    isValid = false;
                }
                if (_unitOfWork.GetRepository<SubscriptionStatus>().GetById(Subscription.Subscription.SubscriptionStatusId).Result == null)
                {

                    _result.Message.Add("The subscription status not found");
                    isValid = false;
                }
               
                if (isValid)
                {
                    var subscription = await _unitOfWork.GetRepository<Subscription>().Insert(_mapper.Map<Subscription>(Subscription.Subscription));
                    switch (Subscription.PaymentChoice)
                    {
                        case 1:
                            try
                            {
                                _result.Data = _paymentService.CreatePaymentUrlVNPay(_mapper.Map<SubscriptionDto>(subscription), context);
                                _unitOfWork.SaveChange();
                                _result.Message.Add(SD.ResponeMessage.CREATE_SUCCESS);

                            }
                            catch (Exception ex)
                            {
                                _result.Message.Add($"{ex.Message}");
                            }
                            break;
                        case 2:
                            try
                            {
                                _result.Data = _paymentService.CreatePaymentUrlMomo(_mapper.Map<SubscriptionDto>(subscription));

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

        //public async Task CreateSubscription(SubscriptionDto Subscription)
        //{
        //    var classDb = await _unitOfWork.GetRepository<Class>().GetById(Subscription.ClassId);
        //    var userDb = await _unitOfWork.GetRepository<ApplicationUser>().GetById(Subscription.UserId);
        //    if(classDb == null || userDb == null || (bool)classDb.IsDeleted) {
        //        return;
        //    }

        //    var userSubscriptions = (IEnumerable<Subscription>)_subscriptionRepository.getSubcriptionByUserId(Subscription.UserId);
        //    if(userSubscriptions != null) {
        //        bool possibleSubscription = true;
        //        foreach(var userSubscription in userSubscriptions)
        //        {
        //            if(userSubscription.ClassId == Subscription.ClassId &&
        //               isValidSubscriptionToAdd(Subscription.SubscriptionStatusId)) { 
        //                   possibleSubscription = false;
        //                break;
        //            }
        //        }
        //        if(possibleSubscription)
        //        {
        //            await _unitOfWork.GetRepository<Subscription>().Insert(_mapper.Map<Subscription>(Subscription));
        //            _unitOfWork.SaveChange();
        //        }
        //    }
        //    await _unitOfWork.GetRepository<Subscription>().Insert(_mapper.Map<Subscription>(Subscription));
        //    _unitOfWork.SaveChange();
        //}

        public async Task<AppActionResult> UpdateSubscription(SubscriptionDto Subscription)
        {
            try
            {

                bool isValid = true;
                if (_unitOfWork.GetRepository<Subscription>().GetById(Subscription.SubscriptionId) != null)
                {
                    isValid = false;
                    _result.Message.Add($"The subscription with id {Subscription.SubscriptionId} not found");
                }
                if (isValid)
                {
                    await _unitOfWork.GetRepository<Subscription>().Update(_mapper.Map<Subscription>(Subscription));
                    _unitOfWork.SaveChange();
                    _result.Message.Add(SD.ResponeMessage.UPDATE_SUCCESS);
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

        //private bool isValidSubscriptionToAdd(int subscriptionStatus)
        //{
        //    return subscriptionStatus == SD.Subscription.FAIL_PAY_BANK_TRANSFER
        //         || subscriptionStatus == SD.Subscription.FAIL_PAY_VNPAY
        //         || subscriptionStatus == SD.Subscription.FAIL_PAY_BANK_TRANSFER
        //         || subscriptionStatus == SD.Subscription.SUCCESS_REFUND_BANK_TRANSFER
        //         || subscriptionStatus == SD.Subscription.SUCCESS_REFUND_VNPAY
        //         || subscriptionStatus == SD.Subscription.SUCCESS_REFUND_MOMO;
        //}


    }
}

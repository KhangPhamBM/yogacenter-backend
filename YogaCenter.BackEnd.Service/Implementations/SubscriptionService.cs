using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.Service.Contracts;
using YogaCenter.BackEnd.Service.Util;

namespace YogaCenter.BackEnd.Service.Implementations
{
    public class SubscriptionService : ISubscriptionService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private ISubscriptionRepository _subscriptionRepository;
        public SubscriptionService(IUnitOfWork unitOfWork, IMapper mapper, ISubscriptionRepository subscriptionRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _subscriptionRepository = subscriptionRepository;
        }
        public async Task CreateSubscription(SubscriptionDto Subscription)
        {
            var classDb = await _unitOfWork.GetRepository<Class>().GetById(Subscription.ClassId);
            var userDb = await _unitOfWork.GetRepository<ApplicationUser>().GetById(Subscription.UserId);
            if(classDb == null || userDb == null || (bool)classDb.IsDeleted) {
                return;
            }

            var userSubscriptions = (IEnumerable<Subscription>)_subscriptionRepository.getSubcriptionByUserId(Subscription.UserId);
            if(userSubscriptions != null) {
                bool possibleSubscription = true;
                foreach(var userSubscription in userSubscriptions)
                {
                    if(userSubscription.ClassId == Subscription.ClassId &&
                       isValidSubscriptionToAdd(Subscription.SubscriptionStatusId)) { 
                           possibleSubscription = false;
                        break;
                    }
                }
                if(possibleSubscription)
                {
                    await _unitOfWork.GetRepository<Subscription>().Insert(_mapper.Map<Subscription>(Subscription));
                    _unitOfWork.SaveChange();
                }
            }
            await _unitOfWork.GetRepository<Subscription>().Insert(_mapper.Map<Subscription>(Subscription));
            _unitOfWork.SaveChange();
        }

        public async Task UpdateSubscription(SubscriptionDto Subscription)
        {
            var subscriptionDb = _unitOfWork.GetRepository<Subscription>().GetById(Subscription.SubscriptionId);
            if(subscriptionDb != null)
            {
                await _unitOfWork.GetRepository<Subscription>().Update(_mapper.Map<Subscription>(Subscription));
                _unitOfWork.SaveChange();
            }
        }

        private bool isValidSubscriptionToAdd(int subscriptionStatus)
        {
            return subscriptionStatus == Constant.Subscription.FAIL_PAY_BANK_TRANSFER
                 || subscriptionStatus == Constant.Subscription.FAIL_PAY_VNPAY
                 || subscriptionStatus == Constant.Subscription.FAIL_PAY_BANK_TRANSFER
                 || subscriptionStatus == Constant.Subscription.SUCCESS_REFUND_BANK_TRANSFER
                 || subscriptionStatus == Constant.Subscription.SUCCESS_REFUND_VNPAY
                 || subscriptionStatus == Constant.Subscription.SUCCESS_REFUND_MOMO;
        }

        
    }
}

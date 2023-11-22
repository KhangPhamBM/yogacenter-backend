using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto.Common;
using YogaCenter.BackEnd.Common.Dto.Request;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Implementations;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.DAL.Util;
using YogaCenter.BackEnd.Service.Contracts;

namespace YogaCenter.BackEnd.Service.Implementations
{
    public class SubscriptionService : GenericBackendService, ISubscriptionService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private ISubscriptionRepository _subscriptionRepository;
        private readonly AppActionResult _result;
        public SubscriptionService(
            IUnitOfWork unitOfWork,
            IMapper mapper, ISubscriptionRepository subscriptionRepository,
            IServiceProvider serviceProvider
            ) : base(serviceProvider)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _subscriptionRepository = subscriptionRepository;
            _result = new AppActionResult();
        }

        public async Task<AppActionResult> CreateSubscription(SubscriptionRequest Subscription, HttpContext context)
        {
            try
            {
                var paymentService = Resolve<IPaymentService>();
                var classRepository = Resolve<IClassRepository>();
                var classDetailRepository = Resolve<IClassDetailRepository>();
                var accountRepository = Resolve<IAccountRepository>();
                var courseRepository = Resolve<ICourseRepository>();
                var subcriptionStatusRepository = Resolve<ISubscriptionStatusRepository>();
                var utility = Resolve<YogaCenter.BackEnd.DAL.Util.Utility>();




                bool isValid = true;
                if (Subscription.PaymentChoice != SD.PaymentType.VNPAY && Subscription.PaymentChoice != SD.PaymentType.MOMO && Subscription.PaymentChoice != 3)
                {
                    _result.Message.Add("The API only support for VNPAY (1) and Momo (2) or add subcription with no payment (3)");
                    isValid = false;
                }

                if (await classRepository.GetById(Subscription.Subscription.ClassId) == null)
                {
                    _result.Message.Add("The class not found");
                    isValid = false;
                }
                else
                {
                    if (utility.GetCurrentDateInTimeZone() < classRepository
                    .GetById(Subscription.Subscription.ClassId).Result.EndDate
                    &&
                    await classDetailRepository
                    .GetByExpression(c => c.UserId == Subscription.Subscription.UserId && c.ClassId == Subscription.Subscription.ClassId) != null)
                    {
                        _result.Message.Add("This action was blocked because the trainee is studying a class which hasn't ended ");
                        isValid = false;
                    }
                }
                if (await accountRepository.GetById(Subscription.Subscription.UserId) == null)
                {
                    _result.Message.Add("The user not found");
                    isValid = false;
                }
                if (await subcriptionStatusRepository.GetById(Subscription.Subscription.SubscriptionStatusId) == null)
                {

                    _result.Message.Add("The subscription status not found");
                    isValid = false;
                }
                if (isValid)
                {
                    Subscription.Subscription.SubscriptionId = Guid.NewGuid().ToString();
                    var classDb = await classRepository.GetById(Subscription.Subscription.ClassId);
                    var course = await courseRepository.GetById(classDb.CourseId);
                    double total = 0;
                    total = (double)((course.Price == null ? 0 : course.Price) * (100 - (course.Discount == null ? 0 : course.Discount)) / 100);
                    Subscription.Subscription.Total = total;

                    var subscription = await _subscriptionRepository.Insert(_mapper.Map<Subscription>(Subscription.Subscription));
                    await _unitOfWork.SaveChangeAsync();

                    switch (Subscription.PaymentChoice)
                    {
                        case 1:
                            try
                            {
                                _result.Result.Data = await paymentService.CreatePaymentUrlVNPay(_mapper.Map<SubscriptionDto>(subscription), context);
                            }
                            catch (Exception ex)
                            {
                                _result.Message.Add($"{ex.Message}");
                            }
                            break;
                        case 2:
                            try
                            {
                                _result.Result.Data = await paymentService.CreatePaymentUrlMomo(_mapper.Map<SubscriptionDto>(subscription));
                            }
                            catch (Exception ex)
                            {
                                _result.Message.Add($"{ex.Message}");
                            }
                            break;
                        default:
                            _result.Result.Data = "";
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

        public async Task<AppActionResult> GetPaymentUrl(string subcriptionId, int choice, HttpContext context)
        {
            try
            {
                var paymentService = Resolve<IPaymentService>();
                var classRepository = Resolve<IClassRepository>();
                var classDetailRepository = Resolve<IClassDetailRepository>();
                var accountRepository = Resolve<IAccountRepository>();
                var courseRepository = Resolve<ICourseRepository>();
                var subcriptionStatusRepository = Resolve<ISubscriptionStatusRepository>();
                var utility = Resolve<YogaCenter.BackEnd.DAL.Util.Utility>();

                bool isValid = true;
                var subcription = await _subscriptionRepository.GetById(subcriptionId);
                if (choice != SD.PaymentType.VNPAY && choice != SD.PaymentType.MOMO && choice != 3)
                {
                    _result.Message.Add("The API only support for VNPAY (1) and Momo (2) or add subcription with no payment (3)");
                    isValid = false;
                }

                if (await classRepository.GetById(subcription.ClassId) == null)
                {
                    _result.Message.Add("The class not found");
                    isValid = false;
                }
                else
                {
                    if (utility.GetCurrentDateInTimeZone() < classRepository
                    .GetById(subcription.ClassId).Result.EndDate
                    &&
                    await classDetailRepository
                    .GetByExpression(c => c.UserId == subcription.UserId && c.ClassId == subcription.ClassId) != null)
                    {
                        _result.Message.Add("This action was blocked because the trainee is studying a class which hasn't ended ");
                        isValid = false;
                    }
                }
                if (await accountRepository.GetById(subcription.UserId) == null)
                {

                    _result.Message.Add("The user not found");
                    isValid = false;
                }
                if (await subcriptionStatusRepository.GetById(subcription.SubscriptionStatusId) == null)
                {

                    _result.Message.Add("The subscription status not found");
                    isValid = false;
                }
                if (subcription == null)
                {

                    _result.Message.Add($"The subscription with id {subcriptionId} not found. Please create subscription");
                    isValid = false;
                }

                if (isValid)
                {
                    var subscription = await _subscriptionRepository.GetById(subcriptionId);

                    switch (choice)
                    {
                        case 1:
                            try
                            {
                                _result.Result.Data = await paymentService.CreatePaymentUrlVNPay(_mapper.Map<SubscriptionDto>(subscription), context);

                            }
                            catch (Exception ex)
                            {
                                _result.Message.Add($"{ex.Message}");
                            }
                            break;
                        case 2:
                            try
                            {
                                _result.Result.Data = await paymentService.CreatePaymentUrlMomo(_mapper.Map<SubscriptionDto>(subscription));

                            }
                            catch (Exception ex)
                            {
                                _result.Message.Add($"{ex.Message}");
                            }
                            break;

                        default:
                            _result.Result.Data = "";
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
                if (_subscriptionRepository.GetById(Subscription.SubscriptionId) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The subscription with id {Subscription.SubscriptionId} not found");
                }
                if (isValid)
                {
                    await _subscriptionRepository.Update(_mapper.Map<Subscription>(Subscription));
                    await _unitOfWork.SaveChangeAsync();
                    _result.Message.Add(SD.ResponseMessage.UPDATE_SUCCESSFUL);
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
                if (await _subscriptionRepository.GetById(subcriptionId) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The subscription with id {subcriptionId} not found");
                }
                if (isValid)
                {
                    _result.Result.Data = await _subscriptionRepository.GetByExpression(s => s.SubscriptionId == subcriptionId && s.SubscriptionStatusId == SD.Subscription.PENDING);
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
                if (await _subscriptionRepository.GetById(subcriptionId) != null)
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
                    Subscription subscription = await _subscriptionRepository.GetById(subcriptionId);
                    subscription.SubscriptionStatusId = status;
                    await _subscriptionRepository.Update(subscription);
                    _unitOfWork.SaveChangeAsync();
                    _result.Message.Add(SD.ResponseMessage.UPDATE_SUCCESSFUL);
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

        public async Task<AppActionResult> SearchApplyingSortingAndFiltering(BaseFilterRequest filterRequest)
        {
            try
            {
                var source = await _subscriptionRepository.GetAll();
                int pageSize = filterRequest.pageSize;
                if (pageSize <= 0) pageSize = SD.MAX_RECORD_PER_PAGE;
                int totalPage = DataPresentationHelper.CalculateTotalPageSize(source.Count(), pageSize);
                if (filterRequest != null)
                {
                    if (filterRequest.pageIndex <= 0 || filterRequest.pageSize <= 0)
                    {
                        _result.Message.Add($"Invalid value of pageIndex or pageSize");
                        _result.isSuccess = false;
                    }
                    else
                    {
                        if (filterRequest.keyword != "")
                        {
                            source = await _subscriptionRepository.GetListByExpression(c => c.UserId.Contains(filterRequest.keyword), null);
                        }
                        if (filterRequest.filterInfoList != null)
                        {
                            source = DataPresentationHelper.ApplyFiltering(source, filterRequest.filterInfoList);
                        }
                        totalPage = DataPresentationHelper.CalculateTotalPageSize(source.Count(), filterRequest.pageSize);
                        if (filterRequest.sortInfoList != null)
                        {
                            source = DataPresentationHelper.ApplySorting(source, filterRequest.sortInfoList);
                        }
                        source = DataPresentationHelper.ApplyPaging(source, filterRequest.pageIndex, filterRequest.pageSize);
                        _result.Result.Data = source;
                    }
                }
                else
                {
                    _result.Result.Data = source;
                }
                _result.Result.TotalPage = totalPage;
            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }

        public Task<AppActionResult> GetPaymentUrl(SubscriptionRequest Subscription, HttpContext context)
        {
            throw new NotImplementedException();
        }
    }
}

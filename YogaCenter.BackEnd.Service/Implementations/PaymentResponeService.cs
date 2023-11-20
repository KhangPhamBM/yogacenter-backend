using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto.Common;
using YogaCenter.BackEnd.Common.Dto.Request;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.DAL.Util;
using YogaCenter.BackEnd.Service.Contracts;

namespace YogaCenter.BackEnd.Service.Implementations
{
    public class PaymentResponeService : GenericBackendService,IPaymentResponeService
    {
        private readonly AppActionResult _result;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private IPaymentResponseRepository _paymentResponseRepository;
        public PaymentResponeService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IPaymentResponseRepository paymentResponseRepository,
            IServiceProvider serviceProvider)
            :base(serviceProvider)
        {
            _result = new();
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _paymentResponseRepository = paymentResponseRepository;
        }

        public async Task<AppActionResult> AddPaymentRespone(PaymentResponseDto paymentRespone)
        {
            bool isValid = true;

            try
            {
                var subcriptionRepository = Resolve<ISubscriptionRepository>();
                if (await _paymentResponseRepository.GetById(paymentRespone.PaymentTypeId) == null)
                {
                    isValid = false;
                    _result.Message.Add("The payment type not found");
                }
                if (await subcriptionRepository.GetById(paymentRespone.SubscriptionId) == null)
                {
                    isValid = false;
                    _result.Message.Add("The subcription not found");
                }
                if (isValid)
                {
                    await _paymentResponseRepository.Insert(_mapper.Map<PaymentRespone>(paymentRespone));
                    await _unitOfWork.SaveChangeAsync();
                    _result.Message.Add(SD.ResponseMessage.CREATE_SUCCESSFUL);
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

using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class PaymentResponeService : IPaymentResponeService
    {
        private readonly AppActionResult _result;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PaymentResponeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _result = new();
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AppActionResult> AddPaymentRespone(PaymentResponseDto paymentRespone)
        {
            bool isValid = true;

            try
            {
                if (await _unitOfWork.GetRepository<PaymentRespone>().GetById(paymentRespone.PaymentTypeId) == null)
                {
                    isValid = false;
                    _result.Message.Add("The payment type not found");
                }
                if (await _unitOfWork.GetRepository<Subscription>().GetById(paymentRespone.SubscriptionId) == null)
                {
                    isValid = false;
                    _result.Message.Add("The subcription not found");
                }
                if (isValid)
                {
                    await _unitOfWork.GetRepository<PaymentRespone>().Insert(_mapper.Map<PaymentRespone>(paymentRespone));
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

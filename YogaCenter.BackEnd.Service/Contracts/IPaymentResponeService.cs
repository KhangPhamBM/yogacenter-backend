﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto.Common;
using YogaCenter.BackEnd.Common.Dto.Request;

namespace YogaCenter.BackEnd.Service.Contracts
{
    public interface IPaymentResponeService
    {
        Task<AppActionResult> AddPaymentRespone(PaymentResponseDto paymentRespone);

    }
}

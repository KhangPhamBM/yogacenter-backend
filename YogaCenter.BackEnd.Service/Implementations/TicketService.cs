﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.Service.Contracts;

namespace YogaCenter.BackEnd.Service.Implementations
{
    public class TicketService : ITicketService
    {
        public Task<AppActionResult> CreateTicket(TicketDto ticket)
        {
            throw new NotImplementedException();
        }

        public Task<AppActionResult> UpdateTicket(TicketDto ticket)
        {
            throw new NotImplementedException();
        }
    }
}
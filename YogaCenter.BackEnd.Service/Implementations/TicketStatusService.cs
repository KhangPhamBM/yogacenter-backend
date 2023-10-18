using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.Service.Contracts;

namespace YogaCenter.BackEnd.Service.Implementations
{
    public class TicketStatusService : ITicketStatusService
    {
        public Task<AppActionResult> CreateTicketStatus(TicketStatusDto ticketStatus)
        {
            throw new NotImplementedException();
        }

        public Task<AppActionResult> UpdateTicketStatus(TicketStatusDto ticketStatus)
        {
            throw new NotImplementedException();
        }
    }
}

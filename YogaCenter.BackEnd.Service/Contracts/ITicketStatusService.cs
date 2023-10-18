using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;

namespace YogaCenter.BackEnd.Service.Contracts
{
    public interface ITicketStatusService
    {
        Task<AppActionResult> CreateTicketStatus(TicketStatusDto ticketStatus);
        Task<AppActionResult> UpdateTicketStatus(TicketStatusDto ticketStatus);
    }
}

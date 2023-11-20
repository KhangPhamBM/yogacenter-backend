using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto.Common;
using YogaCenter.BackEnd.Common.Dto.Request;
using YogaCenter.BackEnd.DAL.Models;

namespace YogaCenter.BackEnd.Service.Contracts
{
    public interface ITicketService : ISearching<Ticket>
    {
        Task<AppActionResult> CreateTicket(TicketDto ticket);
        Task<AppActionResult> UpdateTicket(TicketDto ticket);
        Task<AppActionResult> GetTicketById(int ticketId);
        Task<AppActionResult> GetAllTicket();


    }
}

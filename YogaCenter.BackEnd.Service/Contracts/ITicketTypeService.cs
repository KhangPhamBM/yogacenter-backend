using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto.Common;
using YogaCenter.BackEnd.Common.Dto.Request;

namespace YogaCenter.BackEnd.Service.Contracts
{
    public interface ITicketTypeService
    {
        Task<AppActionResult> CreateTicketType(TicketTypeDto ticketType);
        Task<AppActionResult> UpdateTicketType(TicketTypeDto ticketType);
        Task<AppActionResult> GetTicketTypeById(int id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.Service.Contracts;

namespace YogaCenter.BackEnd.Service.Implementations
{
    public class TicketTypeService : ITicketTypeService
    {
        Task ITicketTypeService.CreateTicketType(TicketTypeDto ticketType)
        {
            throw new NotImplementedException();
        }

        Task ITicketTypeService.UpdateTicketType(TicketTypeDto ticketType)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.Common.Dto.Common
{
    public class TicketStatusDto
    {
        public int TicketStatusId { get; set; }
        public string? TicketStatusName { get; set; }
    }
}

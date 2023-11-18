using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.Common.Dto
{
    public class TicketDto
    {
        public int TicketId { get; set; }
        public string? Note { get; set; }
        public int? TicketStatusId { get; set; }

        public string? UserId { get; set; }

    }
}

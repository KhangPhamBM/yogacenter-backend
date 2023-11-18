using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.DAL.Models
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }
        public string? Note { get; set; }
        public int? TicketTypeId { get; set; }
        [ForeignKey("TicketTypeId"), Column(Order = 1)]
        public TicketType? TicketType { get; set; }
        public int? TicketStatusId { get; set; }
        [ForeignKey("TicketStatusId")]
        public TicketStatus? TicketStatus { get; set; }

        public string? UserId { get; set; }
        [ForeignKey("UserId"), Column(Order = 2)]
        public ApplicationUser? User { get; set; }

    }
}

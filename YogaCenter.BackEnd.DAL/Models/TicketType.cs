using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.DAL.Models
{
    public class TicketType
    {
        [Key]
        public int TicketTypeId { get; set; }
        public string? TicketName { get; set; }
    }
}

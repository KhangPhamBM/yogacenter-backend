using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.DAL.Data;
using YogaCenter.BackEnd.DAL.Models;

namespace YogaCenter.BackEnd.DAL.Implementations
{
    public class TicketTypeRepository : Repository<TicketType>
    {
        public TicketTypeRepository(YogaCenterContext context) : base(context)
        {
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.Service.Contracts;

namespace YogaCenter.BackEnd.API.Controllers
{
    [Route("ticket")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly ITicketTypeService _ticketTypeService;
        private readonly ITicketStatusService _ticketStatusService;

        public TicketController(ITicketService ticketService, ITicketTypeService ticketTypeService, ITicketStatusService ticketStatusService)
        {
            _ticketService = ticketService;
            _ticketTypeService = ticketTypeService;
            _ticketStatusService = ticketStatusService;
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<AppActionResult> GetTicketById(int id)
        {
            return await _ticketService.GetTicketById(id);
        }
    }
}

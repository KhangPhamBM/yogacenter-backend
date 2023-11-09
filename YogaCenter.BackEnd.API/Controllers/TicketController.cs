using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.Service.Contracts;
using YogaCenter.BackEnd.Service.Implementations;

namespace YogaCenter.BackEnd.API.Controllers
{
    [Authorize]
    [Route("ticket")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly ITicketTypeService _ticketTypeService;
        private readonly ITicketStatusService _ticketStatusService;

        public TicketController
            (
            ITicketService ticketService,
            ITicketTypeService ticketTypeService,
            ITicketStatusService ticketStatusService
            )
        {
            _ticketService = ticketService;
            _ticketTypeService = ticketTypeService;
            _ticketStatusService = ticketStatusService;
        }

        [HttpGet("get-ticket-by-id/{id:int}")]
        public async Task<AppActionResult> GetTicketById(int id)
        {
            return await _ticketService.GetTicketById(id);
        }
        [HttpPost("create-ticket")]
        public async Task<AppActionResult> CreateTicket(TicketDto ticketDto)
        {
            return await _ticketService.CreateTicket(ticketDto);
        }
        [HttpPut("update-ticket")]
        public async Task<AppActionResult> UpdateTicket(TicketDto ticketDto)
        {
            return await _ticketService.UpdateTicket(ticketDto);
        }

        [HttpGet("get-ticket-type-by-id/{id:int}")]
        public async Task<AppActionResult> GetTicketTypeById(int id)
        {
            return await _ticketTypeService.GetTicketTypeById(id);
        }
        [HttpPost("create-ticket-type")]
        public async Task<AppActionResult> CreateTicketType(TicketTypeDto ticketTypeDto)
        {
            return await _ticketTypeService.CreateTicketType(ticketTypeDto);
        }
        [HttpPut("update-ticket-type")]
        public async Task<AppActionResult> UpdateTicketType(TicketTypeDto ticketTypeDto)
        {
            return await _ticketTypeService.UpdateTicketType(ticketTypeDto);
        }
        [HttpGet("get-ticket-status-by-id/{id:int}")]
        public async Task<AppActionResult> GetTicketStatusById(int id)
        {
            return await _ticketStatusService.GetTicketStatusById(id);
        }
        [HttpPost("create-ticket-status")]
        public async Task<AppActionResult> CreateTicketStatus(TicketStatusDto ticketStatusDto)
        {
            return await _ticketStatusService.CreateTicketStatus(ticketStatusDto);
        }

        [HttpPut("update-ticket-status")]
        public async Task<AppActionResult> UpdateTicketStatus(TicketStatusDto ticketStatusDto)
        {
            return await _ticketStatusService.UpdateTicketStatus(ticketStatusDto);
        }

        [HttpPost]
        [Route("get-ticket-with-searching")]
        public async Task<AppActionResult> GetTicketWithSearching(BaseFilterRequest baseFilterRequest)
        {
            return await _ticketService.SearchApplyingSortingAndFiltering(baseFilterRequest);
        }

        [HttpGet("get-all-ticket")]
        public async Task<AppActionResult> GetAllTicket()
        { 
            return await _ticketService.GetAllTicket();
        }
    }
}

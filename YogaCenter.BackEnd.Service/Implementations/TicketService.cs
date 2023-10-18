using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.DAL.Util;
using YogaCenter.BackEnd.Service.Contracts;

namespace YogaCenter.BackEnd.Service.Implementations
{
    public class TicketService : ITicketService, ITicketStatusService,ITicketTypeService

    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppActionResult _result;
        private readonly IMapper _mapper;
        public TicketService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _result = new();
            _mapper = mapper;
        }

        public async Task<AppActionResult> CreateTicket(TicketDto ticket)
        {
            bool isValid = true;
            try
            {
                if(await _unitOfWork.GetRepository<TicketStatus>().GetById(ticket.TicketStatusId)== null)
                {
                    isValid = false;
                    _result.Message.Add($"The ticket status with id {ticket.TicketStatusId} not found");
                }
                if (await _unitOfWork.GetRepository<ApplicationUser>().GetById(ticket.UserId) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The user with id {ticket.UserId} not found");
                }
                if (isValid)
                {
                    await _unitOfWork.GetRepository<Ticket>().Insert(_mapper.Map<Ticket>(ticket));
                    _result.Message.Add(SD.ResponseMessage.CREATE_SUCCESS);
                }

            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);

            }
            return _result;

        }

       
        public async Task<AppActionResult> GetTicketById(int ticketId)
        {
            bool isValid = true;
            try
            {
                if (await _unitOfWork.GetRepository<Ticket>().GetById(ticketId) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The ticket with id {ticketId} not found");
                }
               
                if (isValid)
                {
                    await _unitOfWork.GetRepository<Ticket>().GetById(ticketId);
                }

            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);

            }
            return _result;
        }

        public async Task<AppActionResult> UpdateTicket(TicketDto ticket)
        {
            bool isValid = true;
            try
            {
                if (await _unitOfWork.GetRepository<Ticket>().GetById(ticket.TicketId) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The ticket with id {ticket.TicketId} not found");
                }
                if (await _unitOfWork.GetRepository<TicketStatus>().GetById(ticket.TicketStatusId) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The ticket status with id {ticket.TicketStatusId} not found");
                }
                if (await _unitOfWork.GetRepository<ApplicationUser>().GetById(ticket.UserId) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The user with id {ticket.UserId} not found");
                }
                if (isValid)
                {
                    await _unitOfWork.GetRepository<Ticket>().Insert(_mapper.Map<Ticket>(ticket));
                    _result.Message.Add(SD.ResponseMessage.CREATE_SUCCESS);
                }

            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);

            }
            return _result;
        }
        public Task<AppActionResult> CreateTicketStatus(TicketStatusDto ticketStatus)
        {
            throw new NotImplementedException();
        }

        public Task<AppActionResult> CreateTicketType(TicketTypeDto ticketType)
        {
            throw new NotImplementedException();
        }

        public Task<AppActionResult> UpdateTicketStatus(TicketStatusDto ticketStatus)
        {
            throw new NotImplementedException();
        }

        public Task<AppActionResult> UpdateTicketType(TicketTypeDto ticketType)
        {
            throw new NotImplementedException();
        }

        public Task<AppActionResult> GetTicketTypeById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<AppActionResult> GetTicketStatusById(int id)
        {
            throw new NotImplementedException();
        }
    }
}

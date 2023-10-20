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
                    _unitOfWork.SaveChange();
                    _result.Message.Add(SD.ResponseMessage.CREATE_SUCCESSFUL);
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
                    _unitOfWork.SaveChange();
                    _result.Message.Add(SD.ResponseMessage.CREATE_SUCCESSFUL);
                }

            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);

            }
            return _result;
        }
        public async Task<AppActionResult> CreateTicketStatus(TicketStatusDto ticketStatus)
        {
            try
            {
                var existedTicketStatus = await _unitOfWork.GetRepository<TicketStatus>().GetByExpression(ts => ts.TicketStatusName == ticketStatus.TicketStatusName);
                if (existedTicketStatus != null)
                {
                    _result.isSuccess = false;
                    _result.Message.Add($"Tick status whose name is {ticketStatus.TicketStatusName} has already existed");
                }
                else
                {
                    await _unitOfWork.GetRepository<TicketStatus>().Insert(_mapper.Map<TicketStatus>(ticketStatus));
                    _unitOfWork.SaveChange();
                }
            } catch(Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> CreateTicketType(TicketTypeDto ticketType)
        {
            try
            {
                var existedTicketType = await _unitOfWork.GetRepository<TicketType>().GetByExpression(tt => tt.TicketName == ticketType.TicketName);
                if (existedTicketType != null)
                {
                    _result.isSuccess = false;
                    _result.Message.Add($"Ticket type whose name is {ticketType.TicketName} has already existed");
                }
                else
                {
                    await _unitOfWork.GetRepository<TicketType>().Insert(_mapper.Map<TicketType>(ticketType));
                    _unitOfWork.SaveChange();
                }
            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> UpdateTicketStatus(TicketStatusDto ticketStatus)
        {
            bool isValid = true;
            try
            {
                if (await _unitOfWork.GetRepository<TicketStatus>().GetById(ticketStatus.TicketStatusId) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The ticket status with id {ticketStatus.TicketStatusId} not found");
                }

                if (isValid)
                {
                    await _unitOfWork.GetRepository<TicketStatus>().Update(_mapper.Map<TicketStatus>(ticketStatus));
                    _unitOfWork.SaveChange();
                }

            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);

            }
            return _result;
        }

        public async Task<AppActionResult> UpdateTicketType(TicketTypeDto ticketType)
        {
            bool isValid = true;
            try
            {
                if (await _unitOfWork.GetRepository<TicketType>().GetById(ticketType.TicketTypeId) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The ticket status with id {ticketType.TicketTypeId} not found");
                }

                if (isValid)
                {
                    await _unitOfWork.GetRepository<TicketType>().Update(_mapper.Map<TicketType>(ticketType));
                    _unitOfWork.SaveChange();
                }

            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);

            }
            return _result;
        }

        public async Task<AppActionResult> GetTicketTypeById(int id)
        {
            bool isValid = true;
            try
            {
                if (await _unitOfWork.GetRepository<TicketType>().GetById(id) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The ticket type with id {id} not found");
                }

                if (isValid)
                {
                    await _unitOfWork.GetRepository<TicketType>().GetById(id);
                }

            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);

            }
            return _result;
        }

        public async Task<AppActionResult> GetTicketStatusById(int id)
        {
            bool isValid = true;
            try
            {
                if (await _unitOfWork.GetRepository<TicketStatus>().GetById(id) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The ticket status with id {id} not found");
                }

                if (isValid)
                {
                    await _unitOfWork.GetRepository<TicketStatus>().GetById(id);
                }

            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);

            }
            return _result;
        }
    }
}

using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.DAL.Util;
using YogaCenter.BackEnd.Service.Contracts;
using static YogaCenter.BackEnd.DAL.Util.SD;
using TicketStatus = YogaCenter.BackEnd.DAL.Models.TicketStatus;
using TicketType = YogaCenter.BackEnd.DAL.Models.TicketType;

namespace YogaCenter.BackEnd.Service.Implementations
{
    public class TicketService : ITicketService, ITicketStatusService, ITicketTypeService

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
                    await _unitOfWork.GetRepository<Ticket>().Update(_mapper.Map<Ticket>(ticket));
                    _result.Message.Add(SD.ResponseMessage.UPDATE_SUCCESSFUL);
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
            bool isValid = true;
            try
            {
                if (await _unitOfWork.GetRepository<TicketStatus>().GetByExpression(c => c.TicketStatusName == ticketStatus.TicketStatusName) != null)
                {
                    isValid = false;
                    _result.Message.Add($"The ticket name is existed");
                }

                if (isValid)
                {
                    await _unitOfWork.GetRepository<TicketStatus>().Insert(_mapper.Map<TicketStatus>(ticketStatus));
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

        public async Task<AppActionResult> CreateTicketType(TicketTypeDto ticketType)
        {
            bool isValid = true;
            try
            {
                if (await _unitOfWork.GetRepository<TicketType>().GetByExpression(c => c.TicketName == ticketType.TicketName) != null)
                {
                    isValid = false;
                    _result.Message.Add($"The ticket type with name is existed");
                }

                if (isValid)
                {
                    await _unitOfWork.GetRepository<TicketType>().Insert(_mapper.Map<TicketType>(ticketType));
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

        public async Task<AppActionResult> UpdateTicketStatus(TicketStatusDto ticketStatus)
        {
            bool isValid = true;

            try
            {
                if (await _unitOfWork.GetRepository<TicketStatus>().GetById(ticketStatus.TicketStatusId) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The ticket with id not found");
                }

                if (isValid)
                {
                    await _unitOfWork.GetRepository<TicketStatus>().Update(_mapper.Map<TicketStatus>(ticketStatus));
                    _unitOfWork.SaveChange();
                    _result.Message.Add(SD.ResponseMessage.UPDATE_SUCCESSFUL);


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
                    _result.Message.Add($"The ticket type with id not found");
                }

                if (isValid)
                {
                    await _unitOfWork.GetRepository<TicketType>().Update(_mapper.Map<TicketType>(ticketType));
                    _unitOfWork.SaveChange();
                    _result.Message.Add(SD.ResponseMessage.UPDATE_SUCCESSFUL);


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
                    _result.Message.Add($"The ticket type with id not found");
                }

                if (isValid)
                {
                    _result.Result.Data = await _unitOfWork.GetRepository<TicketType>().GetById(id);

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
                    _result.Message.Add($"The ticket status with id not found");
                }

                if (isValid)
                {
                    _result.Result.Data = await _unitOfWork.GetRepository<TicketStatus>().GetById(id);

                }
            }
            catch (Exception ex)
            {

                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> GetTicketWithSearching(BaseFilterRequest baseFilterRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<AppActionResult> SearchApplyingSortingAndFiltering(BaseFilterRequest filterRequest)
        {
            try
            {
                var source = await _unitOfWork.GetRepository<Ticket>().GetAll();
                int totalPage = DataPresentationHelper.CalculateTotalPageSize(source.Count(), filterRequest.pageSize);
                if (filterRequest != null)
                {
                    if (filterRequest.pageIndex <= 0 || filterRequest.pageSize <= 0)
                    {
                        _result.Message.Add($"Invalid value of pageIndex or pageSize");
                        _result.isSuccess = false;
                    }
                    else
                    {
                        if (!filterRequest.keyword.IsEmpty())
                        {
                            source = await _unitOfWork.GetRepository<Ticket>().GetListByExpression(c => c.Note.Contains(filterRequest.keyword), null);
                        }
                        if (filterRequest.filterInfoList != null)
                        {
                            source = DataPresentationHelper.ApplyFiltering(source, filterRequest.filterInfoList);
                        }
                        totalPage = DataPresentationHelper.CalculateTotalPageSize(source.Count(), filterRequest.pageSize);

                        if (filterRequest.sortInfoList != null)
                        {
                            source = DataPresentationHelper.ApplySorting(source, filterRequest.sortInfoList);
                        }
                        source = DataPresentationHelper.ApplyPaging(source, filterRequest.pageIndex, filterRequest.pageSize);
                        _result.Result.Data = source;
                    }
                }
                else
                {
                    _result.Result.Data = source;
                }
                _result.Result.TotalPage = totalPage;
            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> GetAllTicket()
        {
            try
            {

                _result.Result.Data = await _unitOfWork.GetRepository<Ticket>().GetAll();
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

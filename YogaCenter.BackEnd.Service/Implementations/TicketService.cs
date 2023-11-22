using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto.Common;
using YogaCenter.BackEnd.Common.Dto.Request;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.DAL.Util;
using YogaCenter.BackEnd.Service.Contracts;
using static YogaCenter.BackEnd.DAL.Util.SD;
using TicketStatus = YogaCenter.BackEnd.DAL.Models.TicketStatus;
using TicketType = YogaCenter.BackEnd.DAL.Models.TicketType;

namespace YogaCenter.BackEnd.Service.Implementations
{
    public class TicketService : GenericBackendService, ITicketService, ITicketStatusService, ITicketTypeService

    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppActionResult _result;
        private readonly IMapper _mapper;
        private ITicketRepository _ticketRepository;
        public TicketService(IUnitOfWork unitOfWork, IMapper mapper, ITicketRepository ticketRepository, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _unitOfWork = unitOfWork;
            _result = new();
            _mapper = mapper;
            _ticketRepository = ticketRepository;
        }

        public async Task<AppActionResult> CreateTicket(TicketDto ticket)
        {
            bool isValid = true;
            try
            {
                var accountRepository = Resolve<IAccountRepository>();
                var ticketStatusRepository = Resolve<ITicketStatusRepository>();

                if (await ticketStatusRepository.GetById(ticket.TicketStatusId) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The ticket status with id {ticket.TicketStatusId} not found");
                }
                if (await accountRepository.GetById(ticket.UserId) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The user with id {ticket.UserId} not found");
                }
                if (isValid)
                {
                    await _ticketRepository.Insert(_mapper.Map<Ticket>(ticket));
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
                var ticketDB = await _ticketRepository.GetById(ticketId);
                if (ticketDB == null)
                {
                    isValid = false;
                    _result.Message.Add($"The ticket with id {ticketId} not found");
                }

                if (isValid)
                {
                    _result.Result.Data = ticketDB;
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
                var accountRepository = Resolve<IAccountRepository>();
                var ticketStatusRepository = Resolve<ITicketStatusRepository>();
                if (await _ticketRepository.GetById(ticket.TicketId) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The ticket with id {ticket.TicketId} not found");
                }
                if (await ticketStatusRepository.GetById(ticket.TicketStatusId) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The ticket status with id {ticket.TicketStatusId} not found");
                }
                if (await accountRepository.GetById(ticket.UserId) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The user with id {ticket.UserId} not found");
                }
                if (isValid)
                {
                    await _ticketRepository.Update(_mapper.Map<Ticket>(ticket));
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
                var ticketStatusRepository = Resolve<ITicketStatusRepository>();
                if (await ticketStatusRepository.GetByExpression(c => c.TicketStatusName == ticketStatus.TicketStatusName) != null)
                {
                    isValid = false;
                    _result.Message.Add($"The ticket name is existed");
                }

                if (isValid)
                {
                    await ticketStatusRepository.Insert(_mapper.Map<TicketStatus>(ticketStatus));
                    await _unitOfWork.SaveChangeAsync();
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
                var ticketTypeRepository = Resolve<ITicketTypeRepository>();
                if (await ticketTypeRepository.GetByExpression(c => c.TicketName == ticketType.TicketName) != null)
                {
                    isValid = false;
                    _result.Message.Add($"The ticket type with name is existed");
                }

                if (isValid)
                {
                    await ticketTypeRepository.Insert(_mapper.Map<TicketType>(ticketType));
                    await _unitOfWork.SaveChangeAsync();
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
                var ticketStatusRepository = Resolve<ITicketStatusRepository>();
                if (await ticketStatusRepository.GetById(ticketStatus.TicketStatusId) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The ticket with id not found");
                }

                if (isValid)
                {
                    await ticketStatusRepository.Update(_mapper.Map<TicketStatus>(ticketStatus));
                    await _unitOfWork.SaveChangeAsync();
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
                var ticketTypeRepository = Resolve<ITicketTypeRepository>();
                if (await ticketTypeRepository.GetById(ticketType.TicketTypeId) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The ticket type with id not found");
                }

                if (isValid)
                {
                    await ticketTypeRepository.Update(_mapper.Map<TicketType>(ticketType));
                    await _unitOfWork.SaveChangeAsync();
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
                var ticketTypeRepository = Resolve<ITicketTypeRepository>();
                var ticketTypeDb = await ticketTypeRepository.GetById(id);

                if (ticketTypeDb == null)
                {
                    isValid = false;
                    _result.Message.Add($"The ticket type with id not found");
                }

                if (isValid)
                {
                    _result.Result.Data = ticketTypeDb;

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
                var ticketStatusRepository = Resolve<ITicketStatusRepository>();
                var ticketStatusDb = await ticketStatusRepository.GetById(id);
                if (ticketStatusDb == null)
                {
                    isValid = false;
                    _result.Message.Add($"The ticket status with id not found");
                }

                if (isValid)
                {
                    _result.Result.Data = ticketStatusDb;

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
                var source = await _ticketRepository.GetAll();
                var SD = Resolve<YogaCenter.BackEnd.DAL.Util.SD>();
                int pageSize = filterRequest.pageSize;
                if (pageSize <= 0) pageSize = SD.MAX_RECORD_PER_PAGE;
                int totalPage = DataPresentationHelper.CalculateTotalPageSize(source.Count(), pageSize);
                if (filterRequest != null)
                {
                    if (filterRequest.pageIndex <= 0 || filterRequest.pageSize <= 0)
                    {
                        _result.Message.Add($"Invalid value of pageIndex or pageSize");
                        _result.isSuccess = false;
                    }
                    else
                    {
                        if (filterRequest.keyword != "")
                        {
                            source = await _ticketRepository.GetListByExpression(c => c.Note.Contains(filterRequest.keyword), null);
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

                _result.Result.Data = await _ticketRepository.GetAll();
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

using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto.Common;
using YogaCenter.BackEnd.Common.Dto.Request;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Implementations;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.DAL.Util;
using YogaCenter.BackEnd.Service.Contracts;
using static YogaCenter.BackEnd.DAL.Util.SD;
using Room = YogaCenter.BackEnd.DAL.Models.Room;

namespace YogaCenter.BackEnd.Service.Implementations
{
    public class RoomService : GenericBackendService, IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly AppActionResult _result;
        private IRoomRepository _roomRepository;
        public RoomService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IRoomRepository roomRepository,
            IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _result = new AppActionResult();
            _roomRepository = roomRepository;
        }

        public async Task<AppActionResult> CreateRoom(RoomDto room)
        {
            try
            {
                bool isValid = true;
                if (await _roomRepository.GetByExpression(r => r.RoomName == room.RoomName) != null)
                {
                    isValid = false;
                    _result.Message.Add($"The room with name {room.RoomName} is exist");
                }

                if (isValid)
                {
                    await _roomRepository.Insert(_mapper.Map<Room>(room));
                    await _unitOfWork.SaveChangeAsync();
                    _result.Message.Add(SD.ResponseMessage.CREATE_SUCCESSFUL);

                }
                else
                {
                    _result.isSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;

        }

        public async Task<AppActionResult> GetRoomById(int RoomId)
        {
            try
            {
                bool isValid = true;
                if (await _roomRepository.GetById(RoomId) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The room with name {RoomId} not found");
                }

                if (isValid)
                {
                    _result.Result.Data = await _roomRepository.GetById(RoomId);


                }
                else
                {
                    _result.isSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }


        public async Task<AppActionResult> UpdateRoom(RoomDto room)
        {
            try
            {
                bool isValid = true;
                if (await _roomRepository.GetById(room.RoomId) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The room with name {room.RoomId} not found");
                }

                if (isValid)
                {
                    await _roomRepository.Update(_mapper.Map<Room>(room));
                    await _unitOfWork.SaveChangeAsync();
                    _result.Message.Add(SD.ResponseMessage.UPDATE_SUCCESSFUL);

                }
                else
                {
                    _result.isSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> SearchApplyingSortingAndFiltering(BaseFilterRequest filterRequest)
        {
            try
            {
                var source = await _roomRepository.GetAll();
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
                            source = await _roomRepository.GetListByExpression(c => c.RoomName.Contains(filterRequest.keyword), null);
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
    }
}

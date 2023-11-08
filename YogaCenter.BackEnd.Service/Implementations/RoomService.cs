using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.DAL.Util;
using YogaCenter.BackEnd.Service.Contracts;
using static YogaCenter.BackEnd.DAL.Util.SD;
using Room = YogaCenter.BackEnd.DAL.Models.Room;

namespace YogaCenter.BackEnd.Service.Implementations
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly AppActionResult _result;
        public RoomService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _result = new AppActionResult();
        }

        public async Task<AppActionResult> CreateRoom(RoomDto room)
        {
            try
            {
                bool isValid = true;
                if (await _unitOfWork.GetRepository<Room>().GetByExpression(r => r.RoomName == room.RoomName) != null)
                {
                    isValid = false;
                    _result.Message.Add($"The room with name {room.RoomName} is exist");
                }

                if (isValid)
                {
                    await _unitOfWork.GetRepository<Room>().Insert(_mapper.Map<Room>(room));
                    _unitOfWork.SaveChange();
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
                if (await _unitOfWork.GetRepository<Room>().GetById(RoomId) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The room with name {RoomId} not found");
                }

                if (isValid)
                {
                    _result.Result.Data = await _unitOfWork.GetRepository<Room>().GetById(RoomId);


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
                if (await _unitOfWork.GetRepository<Room>().GetById(room.RoomId) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The room with name {room.RoomId} not found");
                }

                if (isValid)
                {
                    await _unitOfWork.GetRepository<Room>().Update(_mapper.Map<Room>(room));
                    _unitOfWork.SaveChange();
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
                var source = await _unitOfWork.GetRepository<Room>().GetAll();
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
                            source = await _unitOfWork.GetRepository<Room>().GetListByExpression(c => c.RoomName.Contains(filterRequest.keyword), null);
                        }
                        if (filterRequest.filterInfoList != null)
                        {
                            source = DataPresentationHelper.ApplyFiltering(source, filterRequest.filterInfoList);
                        }

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

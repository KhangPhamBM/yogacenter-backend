using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.DAL.Util;
using YogaCenter.BackEnd.Service.Contracts;

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
            bool isValid = true;
            if (await _unitOfWork.GetRepository<Room>().GetByExpression(r => r.RoomName == room.RoomName) != null)
            {
                isValid = false;
                _result.Message.Add($"The room with name {room.RoomName} is exist");
            }
           
            if(isValid)
            {
                await _unitOfWork.GetRepository<Room>().Insert(_mapper.Map<Room>(room));
                _unitOfWork.SaveChange();
                _result.Message.Add(SD.ResponeMessage.CREATE_SUCCESS);

            }
            else
            {
                _result.isSuccess = false;
            }
            return _result;

        }

        public async Task<AppActionResult> UpdateRoom(RoomDto room)
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
                _result.Message.Add(SD.ResponeMessage.UPDATE_SUCCESS);

            }
            else
            {
                _result.isSuccess = false;
            }
            return _result;
        }
    }
}

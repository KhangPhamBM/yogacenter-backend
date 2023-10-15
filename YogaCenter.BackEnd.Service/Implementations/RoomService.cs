using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.Service.Contracts;

namespace YogaCenter.BackEnd.Service.Implementations
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RoomService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateRoom(RoomDto room)
        {
            var roomDb = await _unitOfWork.GetRepository<Room>().GetById(room.RoomId);
            if(roomDb == null)
            {
                await _unitOfWork.GetRepository<Room>().Insert(_mapper.Map<Room>(room));
                _unitOfWork.SaveChange();
            }
        }

        public async Task UpdateRoom(RoomDto room)
        {
            var roomDb = await _unitOfWork.GetRepository<Room>().GetById(room.RoomId);
            if (roomDb != null)
            {
                await _unitOfWork.GetRepository<Room>().Update(_mapper.Map<Room>(room));
                _unitOfWork.SaveChange();
            }
        }
    }
}

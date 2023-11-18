using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Util;
using YogaCenter.BackEnd.Service.Contracts;
using YogaCenter.BackEnd.Service.Implementations;

namespace YogaCenter.BackEnd.API.Controllers
{
    [Route("room")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpPost("create-room")]
        [Authorize(Roles = Permission.MANAGEMENT)]

        public async Task<AppActionResult> CreateRoom(RoomDto roomDto)
        {
            return await _roomService.CreateRoom(roomDto);
        }

        [HttpPut("update-room")]
        [Authorize(Roles = Permission.MANAGEMENT)]

        public async Task<AppActionResult> UpdateRoom(RoomDto roomDto)
        {
            return await _roomService.UpdateRoom(roomDto);
        }
        [HttpGet("get-room-by-id/{id:int}")]
        public async Task<AppActionResult> GetRoomById(int id)
        {
            return await _roomService.GetRoomById(id);
        }

        [HttpPost("get-room-with-searching")]
        public async Task<AppActionResult> GetRoomWithSearching(BaseFilterRequest baseFilterRequest)
        {
            return await _roomService.SearchApplyingSortingAndFiltering(baseFilterRequest);
        }
    }
}

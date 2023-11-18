using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Util;
using YogaCenter.BackEnd.Service.Contracts;
using YogaCenter.BackEnd.Service.Implementations;

namespace YogaCenter.BackEnd.API.Controllers
{
    [Route("timeframe")]
    [ApiController]
    public class TimeFrameController : ControllerBase
    {
        private readonly ITimeFrameService _timeFrameService;

        public TimeFrameController(ITimeFrameService timeFrameService)
        {
            _timeFrameService = timeFrameService;
        }
        [Authorize(Roles = Permission.MANAGEMENT)]

        [HttpPost("create-time-frame")]
        public async Task<AppActionResult> CreateTimeFrame(TimeFrameDto timeFrameDto)
        {
            return await _timeFrameService.CreateTimeFrame(timeFrameDto);
        }
        [Authorize(Roles = Permission.MANAGEMENT)]

        [HttpPut("update-time-frame")]
        public async Task<AppActionResult> UpdateTimeFrame(TimeFrameDto timeFrameDto)
        {
            return await _timeFrameService.UpdateTimeFrame(timeFrameDto);
        }
        [Authorize(Roles = Permission.MANAGEMENT)]

        [HttpGet("get-time-frame-by-id/{id:int}")]
        public async Task<AppActionResult> GetTimeFrameById(int id)
        {
            return await _timeFrameService.GetTimeFrameById(id);
        }
        [HttpPost]
        [Authorize(Roles = Permission.MANAGEMENT)]

        [Route("get-timeframe-with-searching")]
        public async Task<AppActionResult> GetTimeFrameWithSearching(BaseFilterRequest baseFilterRequest)
        {
            return await _timeFrameService.SearchApplyingSortingAndFiltering(baseFilterRequest);
        }
    }
}

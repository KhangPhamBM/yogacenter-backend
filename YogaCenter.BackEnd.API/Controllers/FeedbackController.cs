using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Util;
using YogaCenter.BackEnd.Service.Contracts;
using YogaCenter.BackEnd.Service.Implementations;

namespace YogaCenter.BackEnd.API.Controllers
{
    [Route("feedback")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        public readonly IFeedbackService _feedbackService;
        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpPost("create-feedback")]
        [Authorize]

        public async Task<AppActionResult> CreateFeedback(FeedbackDto feedback)
        {
            return await _feedbackService.CreateFeedback(feedback);
        }

        [HttpPut("update-feedback")]
        [Authorize]

        public async Task<AppActionResult> UpdateFeedback(FeedbackDto feedback)
        {
            return await _feedbackService.UpdateFeedback(feedback);
        }
        [HttpGet("get-feedback-by-id/{id:int}")]

        public async Task<AppActionResult> GetFeedbackById(int id)
        {
            return await _feedbackService.GetFeedbackById(id);
        }

        [HttpPost]
        [Route("get-all-feedback")]
        public async Task<AppActionResult> GetAllFeedback(int pageIndex, int pageSize, IList<SortInfo> sortInfos)
        {
            return await _feedbackService.GetAll(pageIndex, pageSize, sortInfos);
        }

        [HttpDelete("delete-feedback/{id:int}")]
        [Authorize(Roles = Permission.ADMIN)]

        public async Task<AppActionResult> DeleteFeedback(int id)
        {
            return await _feedbackService.DeleteFeedback(id);
        }

        [HttpPost("get-feedback-with-searching")]
        public async Task<AppActionResult> GetFeedbackWithSearching(BaseFilterRequest baseFilterRequest)
        {
            return await _feedbackService.SearchApplyingSortingAndFiltering(baseFilterRequest);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YogaCenter.BackEnd.Common.Dto.Request;
using YogaCenter.BackEnd.Service.Contracts;

namespace YogaCenter.BackEnd.API.Controllers
{
    [Route("message")]
    [Authorize]
    [ApiController]
    public class MessageController : ControllerBase
    {
        public IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet("get-message-by-classId/{classId:int}")]
        public async Task<AppActionResult> GetMessageByClassId(int classId)
        {
            return await _messageService.GetMessageByClassId(classId);
        }
        [HttpPost("send-message")]
        public async Task<AppActionResult> SendMessage(MessageRequest request)
        {
            return await _messageService.SendMessage(request);
        }

        [HttpDelete("delete-message-by-id/{id:int}")]
        public async Task<AppActionResult> DeleteMessageById(int id)
        {
            return await _messageService.DeleteMessageById(id);
        }
    }
}

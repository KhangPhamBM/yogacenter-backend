using Microsoft.AspNetCore.SignalR;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.Service.Contracts;

namespace YogaCenter.BackEnd.Realtime
{
    public class ChatHub : Hub
    {
        private IMessageService _messageService;
        private IUnitOfWork _unitOfWork;

        private AppActionResult _result;

        public ChatHub(IUnitOfWork unitOfWork, IMessageService messageService)
        {
            _unitOfWork = unitOfWork;
            _messageService = messageService;
            _result = new AppActionResult();
        }
        public async Task<AppActionResult> SendMessage(MessageRequest request)
        {
            try
            {
                var message = await _messageService.SendMessage(request);
                await Clients.All.SendAsync("ReceiveMessage", message);
                _result = message;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

            }
            return _result;
        }
    }
}
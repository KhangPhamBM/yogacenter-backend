using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;

namespace YogaCenter.BackEnd.Service.Contracts
{
    public interface IMessageService
    {
        Task<AppActionResult> GetMessageByClassId(int classId);
        Task<AppActionResult> SendMessage(MessageRequest request);
        Task<AppActionResult> DeleteMessageById(int id);
    }
}

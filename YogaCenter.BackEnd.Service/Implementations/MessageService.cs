using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto.Request;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.DAL.Util;
using YogaCenter.BackEnd.Service.Contracts;

namespace YogaCenter.BackEnd.Service.Implementations
{
    public class MessageService : GenericBackendService,IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private readonly AppActionResult _result;
        private IMessageRepository _messageRepository;

        public MessageService(
            IUnitOfWork unitOfWork,
            IMapper mapper, 
            IMessageRepository messageRepository,
            IServiceProvider serviceProvider)
            :base(serviceProvider) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _result = new AppActionResult();
            _messageRepository = messageRepository;
        }

        public async Task<AppActionResult> DeleteMessageById(int id)
        {
            try
            {
                bool isValid = true;
                var message = await _messageRepository.GetById(id);
                if (message == null)
                {
                    isValid = false;
                    _result.Message.Add("The message is not existed");

                }
                if (isValid)
                {
                    message.isDeleted = true;
                    await _messageRepository.Update(message);
                    _unitOfWork.SaveChange();
                }

            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> GetMessageByClassId(int classId)
        {
            try
            {
                bool isValid = true;
                var classRepository = Resolve<IClassRepository>();
                if (await classRepository.GetById(classId) == null)
                {
                    isValid = false;
                    _result.Message.Add("The class is not existed");

                }

                if (isValid)
                {
                    List<Message> messages = new List<Message>();
                    var list = await _messageRepository.GetListByExpression(c => c.ClassDetail.ClassId == classId, c => c.ClassDetail.User, c => c.ClassDetail.Class);
                    foreach (var item in list)
                    {
                        if (item.isDeleted)
                        {
                            item.MessageContent = "This message is deleted";
                        }
                        else
                        {
                            item.MessageContent = EncryptionHelper.Decrypt(item.MessageContent);

                        }
                        messages.Add(item);
                    }
                    _result.Result.Data = messages;
                }

            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> SendMessage(MessageRequest request)
        {
            try
            {
                bool isValid = true;
                var classDetailRepository = Resolve<IClassDetailRepository>();
                var classDetail = await classDetailRepository.GetByExpression(c => c.UserId == request.UserId && c.ClassId == request.ClassId);
                if (classDetail == null)
                {
                    isValid = false;
                    _result.Message.Add($"This trainee is not a member of class with id {request.ClassId}");
                }
                if (isValid)
                {
                    Message message = new Message()
                    {
                        isDeleted = false,
                        MessageContent = EncryptionHelper.Encrypt(request.Message),
                        SendTime = DateTime.Now,
                        ClassDetailId = (int)(classDetail?.ClassDetailId),
                    };
                    await _messageRepository.Insert(message);
                    _unitOfWork.SaveChange();
                    message.MessageContent = EncryptionHelper.Decrypt(message.MessageContent);
                    _result.Result.Data = message;
                    _result.Message.Add(SD.ResponseMessage.CREATE_SUCCESSFUL);

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

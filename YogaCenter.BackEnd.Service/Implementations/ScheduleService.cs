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
    public class ScheduleService : IScheduleService
    {
        private IMapper _mapper;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IClassDetailRepository _classDetailRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppActionResult _result;

        public ScheduleService(IMapper mapper, IScheduleRepository scheduleRepository, IUnitOfWork unitOfWork, IClassDetailRepository classDetailRepository)
        {
            _mapper = mapper;
            _scheduleRepository = scheduleRepository;
            _unitOfWork = unitOfWork;
            _classDetailRepository = classDetailRepository;
            _result = new();
        }

        public async Task<AppActionResult> GetScheduleByClassId(int id)
        {
            _result.Data = await _unitOfWork.GetRepository<Schedule>().GetListByExpression(c => c.ClassId == id);
            return _result;
        }
        public async Task<AppActionResult> RegisterSchedulesForClass(IEnumerable<ScheduleDto> scheduleListDto, int classId)
        {
            try
            {

                bool isValid = true;
                if (await _unitOfWork.GetRepository<Class>().GetById(classId) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The class with {classId} not found");
                }
                foreach (ScheduleDto scheduleDto in scheduleListDto)
                {

                    if (scheduleDto.ClassId != classId)
                    {
                        isValid = false;
                        _result.Message.Add($"The classId Of Schedule {scheduleDto.ClassId} must same {classId} ");

                    }

                    var check = await _unitOfWork.GetRepository<Schedule>()
                        .GetByExpression(s => s.Date == scheduleDto.Date &&
                        s.TimeFrameId == scheduleDto.TimeFrameId &&
                        s.ClassId == scheduleDto.ClassId &&
                        s.RoomId == scheduleDto.RoomId
                        );
                    if (check != null)
                    {
                        isValid = false;
                        _result.Message.Add($"The schedule is exist ");


                    }
                }
                if (isValid)
                {
                    foreach (ScheduleDto scheduleDto in scheduleListDto)
                    {
                        await _unitOfWork.GetRepository<Schedule>().Insert(_mapper.Map<Schedule>(scheduleDto));
                        _unitOfWork.SaveChange();

                    }
                    _result.Message.Add(SD.ResponeMessage.CREATE_SUCCESS);
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

        public async Task<AppActionResult> GetSchedulesByUserId(string UserId)
        {
            try {
                bool isValid = true;
                var classDetail = await _unitOfWork.GetRepository<ClassDetail>().GetByExpression(s => s.UserId == UserId);
                if (classDetail == null)
                {
                    isValid = false;
                    _result.Message.Add($"The user with Id {UserId} not found");
                }
                if (isValid)
                {
                    _result.Data = await _unitOfWork.GetRepository<Schedule>().GetListByExpression(c => c.ClassId == classDetail.ClassId);

                }
                else
                {
                    _result.isSuccess = false;
                }
            }
            catch(Exception ex) {

                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }


    }
}

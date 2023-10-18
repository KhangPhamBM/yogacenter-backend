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
using static YogaCenter.BackEnd.Common.Dto.CreateScheduleRequest;

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

        public async Task<AppActionResult> GetSchedulesByUserId(string UserId)
        {
            try
            {
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
            catch (Exception ex)
            {

                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> UpdateSchedule(ScheduleDto scheduleDto)
        {
            try
            {
                bool isValid = true;
                if (await _unitOfWork.GetRepository<Schedule>().GetById(scheduleDto.ScheduleId) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The schedule with Id {scheduleDto.ScheduleId} not found");
                }
                if (isValid)
                {
                    await _unitOfWork.GetRepository<Schedule>().Update(_mapper.Map<Schedule>(scheduleDto));
                    _unitOfWork.SaveChange();
                    _result.Message.Add(SD.ResponseMessage.UPDATE_SUCCESS);
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

        public async Task<AppActionResult> GenerateScheduleForClass(CreateScheduleRequest request)
        {
            bool isValid = true;
            try
            {
                if (await _unitOfWork.GetRepository<Class>().GetById(request.ClassId) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The class with id {request.ClassId} not found");

                }
                foreach (var item in request.Schedules)
                {
                    if (await _unitOfWork.GetRepository<TimeFrame>().GetById(item.TimeFrameId) == null)
                    {
                        isValid = false;
                        _result.Message.Add($"The timeframe with id {item.TimeFrameId} not found");


                    }

                    if (await _unitOfWork.GetRepository<Room>().GetById(item.RoomId) == null)
                    {
                        isValid = false;
                        _result.Message.Add($"The room with id {item.RoomId} not found");
                    }
                }

                if (isValid)
                {
                    var classDto = await _unitOfWork.GetRepository<Class>().GetById(request.ClassId);

                    DateTime currentDate = (DateTime)classDto.StartDate;
                    foreach (var item in request.Schedules)
                    {
                        while (currentDate < classDto.EndDate)
                        {
                            if (currentDate.DayOfWeek == item.DayOfWeek) 
                            {
                                ScheduleDto scheduleDto = new ScheduleDto
                                {
                                    ClassId = classDto.ClassId,
                                    Date = currentDate,
                                    TimeFrameId = item.TimeFrameId,
                                    RoomId = item.RoomId
                                };

                                await _unitOfWork.GetRepository<Schedule>().Insert(_mapper.Map<Schedule>(scheduleDto));
                            }
                            currentDate = currentDate.AddDays(1);

                        }
                    }
                    _unitOfWork.SaveChange();
                    _result.Message.Add(SD.ResponseMessage.CREATE_SUCCESS);

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

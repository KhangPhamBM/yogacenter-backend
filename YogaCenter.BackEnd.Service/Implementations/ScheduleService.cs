using AutoMapper;
using Microsoft.VisualBasic;
using NPOI.SS.Formula.Functions;
using NPOI.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Implementations;
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
            _result.Result .Data= await _unitOfWork.GetRepository<Schedule>().GetListByExpression(c => c.ClassId == id, c => c.TimeFrame, c => c.Room, c => c.Class);
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
                    _result.Result.Data = await _unitOfWork.GetRepository<Schedule>().GetListByExpression(c => c.ClassId == classDetail.ClassId, c => c.TimeFrame, c => c.Room, c => c.Class);

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
                    _result.isSuccess = false;
                }
                if (isValid)
                {
                    var collidedSchedule = await _unitOfWork.GetRepository<Schedule>()
                                                            .GetByExpression(s => s.Date == scheduleDto.Date
                                                                            && s.RoomId == scheduleDto.RoomId
                                                                            && s.TimeFrameId == scheduleDto.TimeFrameId);
                    if (collidedSchedule != null)
                    {
                        await _unitOfWork.GetRepository<Schedule>().Update(_mapper.Map<Schedule>(scheduleDto));
                        _unitOfWork.SaveChange();
                        _result.Message.Add(SD.ResponseMessage.UPDATE_SUCCESSFUL);
                    }
                    else
                    {
                        _result.isSuccess = false;
                        _result.Message.Add($"Input schedule information is collided");
                    }
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
                    List<ScheduleOfClassDto> schedules = new List<ScheduleOfClassDto>();
                    bool isCollided = false;
                    foreach (var item in request.Schedules)
                    {
                        DateTime currentDate = (DateTime)classDto.StartDate;
                        int diff = (item.DayOfWeek - currentDate.DayOfWeek + 7) % 7;
                        currentDate = currentDate.AddDays(diff).Date;

                        while (currentDate <= classDto.EndDate)
                        {
                            if (await _unitOfWork.GetRepository<Schedule>().GetByExpression(s => s.RoomId == item.RoomId && s.TimeFrameId == item.TimeFrameId && s.Date.Date == currentDate.Date) != null)
                            {
                                isCollided = true;
                                isValid = false;

                                _result.Message.Add($"Collided schedule time at timeFrameId: {item.TimeFrameId}, on {currentDate.DayOfWeek}, {currentDate}, ar roomId: {item.RoomId}");
                            }
                            if (!isCollided)
                            {
                                {
                                    if (currentDate.DayOfWeek == item.DayOfWeek)
                                    {
                                        ScheduleOfClassDto scheduleDto = new ScheduleOfClassDto
                                        {
                                            ClassId = classDto.ClassId,
                                            Date = currentDate.Date,
                                            TimeFrameId = item.TimeFrameId,
                                            RoomId = item.RoomId
                                        };
                                        schedules.Add(scheduleDto);
                                    }
                                }
                            }
                            currentDate = currentDate.AddDays(7);

                        }
                    }
                    if (!isCollided)
                    {
                        schedules = schedules.OrderBy(s => s.Date).ThenBy(s => s.TimeFrameId).ToList();

                        await _unitOfWork.GetRepository<Schedule>().InsertRange(_mapper.Map<IEnumerable<Schedule>>(schedules));

                        _unitOfWork.SaveChange();
                        _result.Message.Add(SD.ResponseMessage.CREATE_SUCCESSFUL);
                    }

                }
            }
            catch (Exception ex)
            {

                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> GetScheduleOfClassByDate(int classId, DateTime date)
        {
            try
            {
                bool isValid = true;
                if (await _unitOfWork.GetRepository<Class>().GetById(classId) == null)
                {
                    isValid = false;
                    _result.Message.Add($"Class with id {classId} does not exist");
                }
                if (isValid)
                {
                    _result.Result.Data = await _unitOfWork.GetRepository<Schedule>().GetListByExpression(s => s.ClassId == classId && s.Date.Day == date.Day, s => s.TimeFrame, s => s.Room);
                }
            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> GetScheduleOfClassByWeek(int classId, int week, int year)
        {
            try
            {
                bool isValid = true;
                if (await _unitOfWork.GetRepository<Class>().GetById(classId) == null)
                {
                    isValid = false;
                    _result.Message.Add($"Class with id {classId} does not exist");
                }

                if (year < 1900)
                {
                    isValid = false;
                    _result.Message.Add("Year must be equal or greater than 1900");
                }

                if (week < 1 || week > 53)
                {
                    isValid = false;
                    _result.Message.Add("Invalid week value");
                }

                if (isValid)
                {
                    DateTime[] weekpoints = GetWeekDates(year, week);
                    _result.Result.Data = await _unitOfWork.GetRepository<Schedule>().GetListByExpression(s => s.ClassId == classId && s.Date <= weekpoints[1] && s.Date >= weekpoints[0] && s.Date.Year == year, s => s.TimeFrame, s => s.Room);
                }
            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> GetScheduleOfClassByMonth(int classId, int month, int year)
        {
            try
            {
                bool isValid = true;
                if (await _unitOfWork.GetRepository<Class>().GetById(classId) == null)
                {
                    isValid = false;
                    _result.Message.Add($"Class with id {classId} does not exist");
                }

                if (year < 1900)
                {
                    isValid = false;
                    _result.Message.Add("Year must be equal or greater than 1900");
                }

                if (month < 1 || month > 12)
                {
                    isValid = false;
                    _result.Message.Add("Invalid month value");
                }

                if (isValid)
                {
                    _result.Result.Data = await _unitOfWork.GetRepository<Schedule>().GetListByExpression(s => s.ClassId == classId && s.Date.Month == month && s.Date.Year == year, s => s.TimeFrame, s => s.Room);
                }
            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }


        public async Task<AppActionResult> GetSchedulesByDate(DateTime date)
        {
            try
            {
                bool isValid = true;
                var classList = await _unitOfWork.GetRepository<Class>().GetAll();
                var scheduleList = new List<ClassDto>();
                if (classList.Count() < 1)
                {
                    _result.Message.Add("The list class is empty");
                    isValid = false;
                }

                if (isValid)
                {
                    foreach (var item in classList)
                    {
                        var classDto = _mapper.Map<ClassDto>(item);
                        classDto.Schedules = _mapper.Map<IEnumerable<ScheduleOfClassDto>>
                           (
                               await _unitOfWork.GetRepository<Schedule>()
                               .GetListByExpression(s => s.ClassId == item.ClassId && s.Date.Day == date.Day, s => s.TimeFrame, s => s.Room)
                           );
                        scheduleList.Add(classDto);

                    }
                    _result.Result.Data = scheduleList;
                }
            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> GetSchedulesByWeek(int week, int year)
        {
            try
            {
                bool isValid = true;
                var classList = await _unitOfWork.GetRepository<Class>().GetAll();
                if (classList.Count() < 1)
                {
                    isValid = false;
                    _result.Message.Add($"List class is empty");
                }

                if (year < 1900)
                {
                    isValid = false;
                    _result.Message.Add("Year must be equal or greater than 1900");
                }

                if (week < 1 || week > 53)
                {
                    isValid = false;
                    _result.Message.Add("Invalid week value");
                }



                if (isValid)
                {
                    var scheduleList = new List<ClassDto>();

                    DateTime[] weekpoints = GetWeekDates(year, week);

                    foreach (var item in classList)
                    {
                        var classDto = _mapper.Map<ClassDto>(item);
                        classDto.Schedules = _mapper.Map<IEnumerable<ScheduleOfClassDto>>
                           (
                                await _unitOfWork.GetRepository<Schedule>().GetListByExpression(s => s.ClassId == item.ClassId && s.Date <= weekpoints[1] && s.Date >= weekpoints[0] && s.Date.Year == year, s => s.TimeFrame, s => s.Room)
                           );
                        scheduleList.Add(classDto);

                    }
                    _result.Result.Data = scheduleList;
                }
            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> GetSchedulesByMonth(int month, int year)
        {
            try
            {
                bool isValid = true;
                var classList = await _unitOfWork.GetRepository<Class>().GetAll();
                if (classList.Count() < 1)
                {
                    isValid = false;
                    _result.Message.Add($"List class is empty");
                }

                if (year < 1900)
                {
                    isValid = false;
                    _result.Message.Add("Year must be equal or greater than 1900");
                }

                if (month < 1 || month > 12)
                {
                    isValid = false;
                    _result.Message.Add("Invalid month value");
                }


                if (isValid)
                {
                    var scheduleList = new List<ClassDto>();


                    foreach (var item in classList)
                    {
                        var classDto = _mapper.Map<ClassDto>(item);
                        classDto.Schedules = _mapper.Map<IEnumerable<ScheduleOfClassDto>>
                           (
                              await _unitOfWork.GetRepository<Schedule>().GetListByExpression(s => s.ClassId == item.ClassId && s.Date.Month == month && s.Date.Year == year, s => s.TimeFrame, s => s.Room)
                           );
                        scheduleList.Add(classDto);

                    }
                    _result.Result.Data = scheduleList;
                }
            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }


        private DateTime[] GetWeekDates(int year, int week)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            DateTime firstDayOfYear = jan1.AddDays(-((int)jan1.DayOfWeek - 1));

            DateTime firstDateOfWeek = firstDayOfYear.AddDays((week - 1) * 7);
            DateTime lastDateOfWeek = firstDateOfWeek.AddDays(6);

            return new DateTime[] { firstDateOfWeek, lastDateOfWeek };
        }

        
    }
}

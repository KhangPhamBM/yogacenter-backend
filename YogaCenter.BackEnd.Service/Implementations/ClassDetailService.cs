using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Implementations;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.DAL.Util;
using YogaCenter.BackEnd.Service.Contracts;
using static YogaCenter.BackEnd.DAL.Util.SD;

namespace YogaCenter.BackEnd.Service.Implementations
{
    public class ClassDetailService : IClassDetailService
    {
        private readonly IClassDetailRepository _classDetailRepository;
        private IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppActionResult _result;

        public ClassDetailService(IClassDetailRepository classDetailRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _classDetailRepository = classDetailRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _result = new();
        }

        public async Task<AppActionResult> RegisterClass(ClassDetailDto detail)
        {
            try
            {
                bool isValid = true;
                var classDto = await _unitOfWork.GetRepository<Class>().GetById(detail.ClassId);
                if (classDto == null)
                {
                    isValid = false;
                    _result.Message.Add($"The class with id {detail.ClassDetailId} not found");
                }
                if (await _unitOfWork.GetRepository<ApplicationUser>().GetById(detail.UserId) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The user with id {detail.ClassDetailId} not found");
                }
                var scheduleList = await _unitOfWork.GetRepository<Schedule>().GetListByExpression(s => s.ClassId == detail.ClassId, null);
                if (scheduleList.Count() < 1)
                {
                    isValid = false;
                    _result.Message.Add($"This class doesn't have any schedules. Please create schedules for the class with id {detail.ClassId}");
                }


                var traineeRole = await _unitOfWork.GetRepository<IdentityRole>().GetByExpression(r => r.NormalizedName.ToLower().Equals("trainee"));
                var trainerRole = await _unitOfWork.GetRepository<IdentityRole>().GetByExpression(r => r.NormalizedName.ToLower().Equals("trainer"));
                if (traineeRole != null && trainerRole != null)
                {
                    if (await _unitOfWork.GetRepository<IdentityUserRole<string>>().GetByExpression(r => r.RoleId == traineeRole.Id && r.UserId == detail.UserId, null) != null)
                    {

                        if (await _unitOfWork.GetRepository<ClassDetail>().GetByExpression(_classDetailRepository.GetByClassIdAndUserId(_mapper.Map<ClassDetail>(detail))) != null && classDto.EndDate >= DateTime.Now)
                        {
                            isValid = false;
                            _result.Message.Add($"The trainee has been registed in this class with id {detail.ClassId}");
                        }
                    }
                    else if (await _unitOfWork.GetRepository<IdentityUserRole<string>>().GetByExpression(r => r.RoleId == trainerRole.Id, null) != null)
                    {
                        var classDetail = await _unitOfWork.GetRepository<ClassDetail>().GetByExpression(c => c.UserId == detail.UserId);
                        var list = await _unitOfWork.GetRepository<Schedule>().GetListByExpression(s => s.ClassId == classDto.ClassId);
                        if (classDetail != null)
                        {
                            var scheduleTrainee = await _unitOfWork.GetRepository<Schedule>().GetListByExpression(s => s.ClassId == classDetail.ClassId);

                            foreach (var item in list)
                            {
                                foreach (var item2 in scheduleTrainee)
                                {
                                    if (item.TimeFrameId == item2.TimeFrameId && item.Date.Date == item2.Date)
                                    {
                                        isValid = false;
                                        var timeFrame = await _unitOfWork.GetRepository<DAL.Models.TimeFrame>().GetById(item.TimeFrameId);
                                        _result.Message.Add($"Collided schedule time : {timeFrame?.TimeFrameName?.ToLower()}, on {item.Date.DayOfWeek} {SD.FormatDateTime(item.Date)}, at roomId: {item.RoomId} & roomId {item2.RoomId}");

                                    }
                                }
                            }
                        }


                    }
                }







                if (isValid)
                {


                    var classDetail = await _unitOfWork.GetRepository<ClassDetail>().Insert(_mapper.Map<ClassDetail>(detail));
                    _unitOfWork.SaveChange();
                    foreach (var schedule in scheduleList)
                    {
                        await _unitOfWork.GetRepository<Attendance>().Insert(new Attendance() { ClassDetailId = classDetail.ClassDetailId, ScheduleId = schedule.ScheduleId, AttendanceStatusId = SD.AttendanceStatus.NOT_YET });
                    }
                    _unitOfWork.SaveChange();
                    _result.Message.Add(SD.ResponseMessage.CREATE_SUCCESSFUL);


                    if (isValid)
                    {
                        _result.Result.Data = await _unitOfWork.GetRepository<ClassDetail>().GetByExpression(_classDetailRepository.GetClassDetailByUserId(detail.UserId));
                    }
                    else
                    {
                        _result.isSuccess = false;
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

        public async Task<AppActionResult> GetClassDetailsByClassId(int classId, int pageIndex, int pageSize, IList<SortInfo> sortInfos)
        {
            try
            {
                bool isValid = true;
                if (await _unitOfWork.GetRepository<Class>().GetById(classId) == null)
                {
                    isValid = false;
                    _result.Message.Add("The class with id {detail.ClassDetailId} not found");
                }
                if (isValid)
                {
                    var details = await _unitOfWork.GetRepository<ClassDetail>().GetListByExpression(cd => cd.ClassId == classId, null);
                    if (details != null)
                    {
                        if (sortInfos != null)
                        {
                            details = DataPresentationHelper.ApplySorting(details, sortInfos);
                        }
                        if (pageIndex > 0 && pageSize > 0)
                        {
                            details = DataPresentationHelper.ApplyPaging(details, pageIndex, pageSize);
                        }
                        _result.Result.Data = details;
                    }
                    else
                    {
                        _result.isSuccess = false;
                        _result.Message.Add($"The class detail with class id {classId} not found");
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
    }
}

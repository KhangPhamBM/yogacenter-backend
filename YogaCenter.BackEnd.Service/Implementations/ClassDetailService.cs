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
                var traineeRole = await _unitOfWork.GetRepository<IdentityRole>()
                    .GetByExpression(r => r.NormalizedName.ToLower().Equals("trainee"));

                var trainerRole = await _unitOfWork.GetRepository<IdentityRole>()
                    .GetByExpression(r => r.NormalizedName.ToLower().Equals("trainer"));

                if (traineeRole == null || trainerRole == null)
                {
                    isValid = false;
                    _result.Message.Add("Please insert role");
                }

                var classDto = await _unitOfWork.GetRepository<Class>().GetById(detail.ClassId);
                if (classDto == null)
                {
                    isValid = false;
                    _result.Message.Add($"The class with id {detail.ClassId} not found");
                }

                if(classDto.MaxOfTrainee == await CountTrainee(classDto))
                {
                    isValid = false;
                    _result.Message.Add($"The class with id {classDto.ClassId} has reached maximum number of trainees");

                }

                if (await _unitOfWork.GetRepository<ApplicationUser>().GetById(detail.UserId) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The user with id {detail.ClassDetailId} not found");
                }

                var scheduleList = await _unitOfWork.GetRepository<Schedule>()
                    .GetListByExpression(s => s.ClassId == detail.ClassId, null);

                if (scheduleList.Count() < 1)
                {
                    isValid = false;
                    _result.Message.Add($"This class doesn't have any schedules. Please create schedules for the class with id {detail.ClassId}");
                }

                if (traineeRole != null && trainerRole != null)
                {
                    isValid = await IsCollidedSchedule(detail, isValid, traineeRole, trainerRole, classDto);
                }

                if (isValid)
                {
                    var classDetail = await _unitOfWork.GetRepository<ClassDetail>().Insert(_mapper.Map<ClassDetail>(detail));
                    _unitOfWork.SaveChange();
                    foreach (var schedule in scheduleList)
                    {
                        await _unitOfWork.GetRepository<Attendance>()
                            .Insert(new Attendance() { 
                                ClassDetailId = classDetail.ClassDetailId, 
                                ScheduleId = schedule.ScheduleId, 
                                AttendanceStatusId = SD.AttendanceStatus.NOT_YET 
                            });
                    }

                    _unitOfWork.SaveChange();
                    _result.Message.Add(SD.ResponseMessage.CREATE_SUCCESSFUL);
                    _result.Result.Data = await _unitOfWork.GetRepository<ClassDetail>()
                        .GetByExpression(_classDetailRepository.GetClassDetailByUserId(detail.UserId));
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

        private async Task<int> CountTrainee(Class classDto)
        {
            int total = 0;
            var classDetail = await _unitOfWork.GetRepository<ClassDetail>().GetAll();

            var traineeRole = await _unitOfWork.GetRepository<IdentityRole>()
                .GetByExpression(r => r.NormalizedName.ToLower().Equals("trainee"));

            var trainerRole = await _unitOfWork.GetRepository<IdentityRole>()
                .GetByExpression(r => r.NormalizedName.ToLower().Equals("trainer"));

            if (traineeRole == null || trainerRole == null || !classDetail.Any())
            {
                return 0;
            }
            List<ApplicationUser> users = new List<ApplicationUser>();
            foreach (var item in classDetail)
            {
                var user = await _unitOfWork.GetRepository<ApplicationUser>().GetById(item.UserId);
                users.Add(user);
            }
            foreach (var user in users)
            {
                if (await _unitOfWork.GetRepository<IdentityUserRole<string>>()
                    .GetByExpression(r => r.RoleId == traineeRole.Id && r.UserId == user.Id, null) != null)
                {
                     total++;
                }
            }
            return total;
        }

        private async Task<bool> IsCollidedSchedule(ClassDetailDto detail, bool isValid, IdentityRole? traineeRole, IdentityRole? trainerRole, Class? classDto)
        {
            if (await _unitOfWork.GetRepository<IdentityUserRole<string>>()
                .GetByExpression(r => r.RoleId == traineeRole.Id && r.UserId == detail.UserId, null) != null)
            {
                var classDetailList = await _unitOfWork.GetRepository<ClassDetail>()
                    .GetListByExpression(c => c.ClassId == detail.ClassId);

                List<ApplicationUser> users = new List<ApplicationUser>();
                foreach (var item in classDetailList)
                {
                    var user = await _unitOfWork.GetRepository<ApplicationUser>().GetById(item.UserId);
                    users.Add(user);
                }

                bool haveTrainer = false;

                foreach (var user in users)
                {
                    if (await _unitOfWork.GetRepository<IdentityUserRole<string>>()
                        .GetByExpression(r => r.RoleId == trainerRole.Id && r.UserId == user.Id, null) != null)
                    {
                        haveTrainer = true;
                    }
                }

                if (!haveTrainer)
                {
                    isValid = false;
                    _result.Message.Add($"This class don't have trainer");
                }

                if (await _unitOfWork.GetRepository<ClassDetail>()
                    .GetByExpression(_classDetailRepository.GetByClassIdAndUserId(_mapper.Map<ClassDetail>(detail))) != null && classDto.EndDate >= DateTime.Now)
                {
                    isValid = false;
                    _result.Message.Add($"The trainee has been registed in this class with id {detail.ClassId}");
                }
            }
            else if (await _unitOfWork.GetRepository<IdentityUserRole<string>>()
                .GetByExpression(r => r.RoleId == trainerRole.Id && r.UserId == detail.UserId, null) != null)
            {
                var classDetail = await _unitOfWork.GetRepository<ClassDetail>()
                    .GetByExpression(c => c.UserId == detail.UserId);

                //Schedule of class that trainer is intended to lecture
                var classSchedules = await _unitOfWork.GetRepository<Schedule>()
                    .GetListByExpression(s => s.ClassId == classDto.ClassId);

                if (classDetail != null)
                {
                    //Current Schedule of that trainer 
                    var trainerSchedules = await _unitOfWork.GetRepository<Schedule>()
                        .GetListByExpression(s => s.ClassId == classDetail.ClassId);

                    HashSet<Schedule> schedules = new HashSet<Schedule>();
                    foreach (var item in classSchedules)
                        schedules.Add(item);          
                    
                    foreach (var schedule in trainerSchedules)
                    {
                        if (schedules.Contains(schedule))
                        {
                            isValid = false;
                            var timeFrame = await _unitOfWork.GetRepository<DAL.Models.TimeFrame>().GetById(schedule.TimeFrameId);
                            _result.Message.Add($"Collided schedule time : {timeFrame?.TimeFrameName?.ToLower()}, on {schedule.Date.DayOfWeek} {SD.FormatDateTime(schedule.Date)} at class id: {schedule.ClassId}");
                        }
                    }

                }
            }
            return isValid;
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
                        if (pageIndex <= 0) pageIndex = 1;
                        if (pageSize <= 0) pageSize = SD.MAX_RECORD_PER_PAGE;
                        int totalPage = DataPresentationHelper.CalculateTotalPageSize(details.Count(), pageSize);

                        if (sortInfos != null)
                            details = DataPresentationHelper.ApplySorting(details, sortInfos);

                        if (pageIndex > 0 && pageSize > 0)
                            details = DataPresentationHelper.ApplyPaging(details, pageIndex, pageSize);
                        _result.Result.Data = details;
                        _result.Result.TotalPage = totalPage;
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

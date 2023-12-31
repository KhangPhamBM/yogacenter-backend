﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto.Common;
using YogaCenter.BackEnd.Common.Dto.Request;
using YogaCenter.BackEnd.Common.Dto.Response;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.DAL.Util;
using YogaCenter.BackEnd.Service.Contracts;

namespace YogaCenter.BackEnd.Service.Implementations
{
    public class AttendanceService : GenericBackendService, IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private readonly AppActionResult _result;

        public AttendanceService
            (
            IAttendanceRepository attendanceRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IFileService fileService,
            IServiceProvider serviceProvider
            )
            : base(serviceProvider)
        {
            _attendanceRepository = attendanceRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _result = new AppActionResult();
        }

        public async Task<IActionResult> ExportData()
        {
            return Resolve<IFileService>().GenerateTemplateExcel(new ClassDto());

        }

        public async Task<AppActionResult> AddListAttendance(IEnumerable<AttendanceDto> attendances)
        {
            try
            {
                bool isValid = true;


                foreach (var attendance in attendances)
                {
                    if (await _attendanceRepository.GetById(attendance.ClassDetailId) == null)
                    {
                        _result.Message.Add($"The class detail with id {attendance.ClassDetailId} not found ");
                        isValid = false;
                    }
                    if (await _attendanceRepository.GetById(attendance.ScheduleId) == null)
                    {
                        _result.Message.Add($"The schedule with id {attendance.ScheduleId} not found ");
                        isValid = false;
                    }
                    if (await _attendanceRepository.GetById(attendance.AttendanceStatusId) == null)
                    {
                        _result.Message.Add($"The attendance status with id {attendance.AttendanceStatusId} not found ");
                        isValid = false;
                    }
                }

                if (isValid)
                {
                    foreach (var attendance in attendances)
                    {
                        await _attendanceRepository.Insert(_mapper.Map<Attendance>(attendance));
                    }
                    await _unitOfWork.SaveChangeAsync();

                    _result.Message.Add(SD.ResponseMessage.CREATE_SUCCESSFUL);

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

        public async Task<AppActionResult> GetAttendancesByClassId(int classId, int pageIndex, int pageSize, IList<SortInfo> sortInfos)
        {
            {
                try
                {
                    var attendances = await _attendanceRepository.GetListByExpression(c => c.ClassDetail.ClassId == classId, c => c.ClassDetail.User, c => c.Schedule, c => c.AttendanceStatus);
                    int totalPage = DataPresentationHelper.CalculateTotalPageSize(attendances.Count(), pageSize);
                    if (sortInfos != null)
                    {
                        attendances = DataPresentationHelper.ApplySorting(attendances, sortInfos);
                    }
                    if (pageIndex > 0 && pageSize > 0)
                    {
                        attendances = DataPresentationHelper.ApplyPaging(attendances, pageIndex, pageSize);
                    }
                    _result.Result.Data = attendances;
                    _result.Result.TotalPage = totalPage;
                }
                catch (Exception ex)
                {
                    _result.isSuccess = false;
                    _result.Message.Add(ex.Message);
                }
                return _result;
            }




        }
        public async Task<AppActionResult> GetAttendancesByScheduleId(int scheduleId, int pageIndex, int pageSize, IList<SortInfo> sortInfos)
        {
            try
            {
                var attendances = await _attendanceRepository.GetListByExpression(c => c.ScheduleId == scheduleId, c => c.ClassDetail.User, c => c.Schedule, c => c.AttendanceStatus);
                int totalPage = DataPresentationHelper.CalculateTotalPageSize(attendances.Count(), pageSize);

                if (sortInfos != null)
                {
                    attendances = DataPresentationHelper.ApplySorting(attendances, sortInfos);
                }
                if (pageIndex > 0 && pageSize > 0)
                {
                    attendances = DataPresentationHelper.ApplyPaging(attendances, pageIndex, pageSize);
                }
                _result.Result.Data = attendances;
                _result.Result.TotalPage = totalPage;
            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;

        }

        public async Task<AppActionResult> GetAttendancesByUserId(string userId, int pageIndex, int pageSize, IList<SortInfo> sortInfos)
        {
            try
            {
                var attendances = await _attendanceRepository.GetListByExpression(c => c.ClassDetail.UserId == userId, c => c.ClassDetail.User, c => c.Schedule, c => c.AttendanceStatus);
                int totalPage = DataPresentationHelper.CalculateTotalPageSize(attendances.Count(), pageSize);

                if (sortInfos != null)
                {
                    attendances = DataPresentationHelper.ApplySorting(attendances, sortInfos);
                }
                if (pageIndex > 0 && pageSize > 0)
                {
                    attendances = DataPresentationHelper.ApplyPaging(attendances, pageIndex, pageSize);
                }
                _result.Result.Data = attendances;
                _result.Result.TotalPage = totalPage;
            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;

        }

        public async Task<AppActionResult> UpdateAttendance(IEnumerable<AttendanceDto> attendances)
        {
            try
            {
                bool isValid = true;
                foreach (var attendance in attendances)
                {
                    if (await _attendanceRepository.GetById(attendance.ClassDetailId) == null)
                    {
                        _result.Message.Add($"The class detail with id {attendance.ClassDetailId} not found ");
                        isValid = false;
                    }
                    if (await _attendanceRepository.GetById(attendance.ScheduleId) == null)
                    {
                        _result.Message.Add($"The schedule with id {attendance.ScheduleId} not found ");
                        isValid = false;
                    }
                    if (await _attendanceRepository.GetById(attendance.AttendanceStatusId) == null)
                    {
                        _result.Message.Add($"The attendance status with id {attendance.AttendanceStatusId} not found ");
                        isValid = false;
                    }
                }

                if (isValid)
                {
                    foreach (var attendance in attendances)
                    {
                        await _attendanceRepository.Update(_mapper.Map<Attendance>(attendance));
                    }
                    await _unitOfWork.SaveChangeAsync();
                    _result.Message.Add(SD.ResponseMessage.UPDATE_SUCCESSFUL);
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
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Models;

namespace YogaCenter.BackEnd.Service.Contracts
{
    public interface IAttendanceService
    {
        Task<AppActionResult> AddListAttendance(IEnumerable<AttendanceDto> attendances); 
        Task<AppActionResult> UpdateAttendance(IEnumerable<AttendanceDto> attendances);
        Task<AppActionResult> GetAttendancesByScheduleId(int scheduleId);
        Task<AppActionResult> GetAttendancesByClassId(int classId);
        Task<AppActionResult> GetAttendancesByUserId(string userId);

    }
}

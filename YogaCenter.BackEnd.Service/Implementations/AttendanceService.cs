using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.Service.Contracts;

namespace YogaCenter.BackEnd.Service.Implementations
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public AttendanceService(IAttendanceRepository attendanceRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _attendanceRepository = attendanceRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddListAttendance(IEnumerable<AttendanceDto> attendances)
        {
            foreach (var attendance in attendances)
            {
                await _unitOfWork.GetRepository<Attendance>().Insert(_mapper.Map<Attendance>(attendance));
            }
            _unitOfWork.SaveChange();
        }
    }
}

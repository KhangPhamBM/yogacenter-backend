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
    public class ScheduleService : IScheduleService
    {
        private IMapper _mapper;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IClassDetailRepository _classDetailRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ScheduleService(IMapper mapper, IScheduleRepository scheduleRepository, IUnitOfWork unitOfWork, IClassDetailRepository classDetailRepository)
        {
            _mapper = mapper;
            _scheduleRepository = scheduleRepository;
            _unitOfWork = unitOfWork;
            _classDetailRepository = classDetailRepository;
        }

        public async Task<IEnumerable<Schedule>> GetScheduleByClassId(int id)
        {
            return await _scheduleRepository.GetSchedulesByClassId(id);
        }
        public async Task RegisterSchedulesForClass(IEnumerable<ScheduleDto> scheduleListDto, int classId)
        {
            bool flag = false;
            var classDb = await _unitOfWork.GetRepository<Class>().GetById(classId);
            if (classDb == null)
            {
                return;
            }
            foreach (ScheduleDto scheduleDto in scheduleListDto)
            {

                if (scheduleDto.ClassId != classId)
                {
                    flag = true;
                    break;
                }
                var check = await _scheduleRepository.GetSchedule(scheduleDto);
                if (check != null)
                {
                    flag = true;
                    break;

                }
            }
            if (flag == false)
            {
                foreach (ScheduleDto scheduleDto in scheduleListDto)
                {
                    await _unitOfWork.GetRepository<Schedule>().Insert(_mapper.Map<Schedule>(scheduleDto));
                    _unitOfWork.SaveChange();
                }
            }
        }

        public async Task<IEnumerable<Schedule>> GetSchedulesByUserId(string UserId)
        {
            var classDetail = await _classDetailRepository.GetClassDetailByUserId(UserId);
            if (classDetail != null)
            {
                var scheduleList = await _scheduleRepository.GetSchedulesByClassId((int)classDetail.ClassId);
                return scheduleList;
            }
            return Enumerable.Empty<Schedule>();
        }
    }
}

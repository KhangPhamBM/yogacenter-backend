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
    public class TimeFrameService : ITimeFrameService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TimeFrameService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<TimeSpan[]> ConvertStringTimeFrameToTime(string timeFrame)
        {
            string[] time = timeFrame.Split('-');
            if (TimeSpan.TryParse(time[0], out TimeSpan start)) {
                if (TimeSpan.TryParse(time[1], out TimeSpan end))
                {
                    return new TimeSpan[] { start, end };
                }
                else
                {
                    Console.WriteLine("Unable to convert ending time");
                }
            }
            else
            {
                Console.WriteLine("Unable to convert starting time");
            }
            return new TimeSpan[0];
        }

        public async Task CreateTimeFrame(TimeFrameDto timeFrameDto)
        {
            var timeFrameDb = _unitOfWork.GetRepository<TimeFrame>().GetById(timeFrameDto.TimeFrameId);
            if (timeFrameDb == null)
            {
                await _unitOfWork.GetRepository<TimeFrame>().Insert(_mapper.Map<TimeFrame>(timeFrameDto));
                _unitOfWork.SaveChange();
            }
        }

        public async Task UpdateTimeFrame(TimeFrameDto timeFrameDto)
        {
            var timeFrameDb = _unitOfWork.GetRepository<TimeFrame>().GetById(timeFrameDto.TimeFrameId);
            if (timeFrameDb != null)
            {
                await _unitOfWork.GetRepository<TimeFrame>().Update(_mapper.Map<TimeFrame>(timeFrameDto));
                _unitOfWork.SaveChange();
            }
        }
    }
}

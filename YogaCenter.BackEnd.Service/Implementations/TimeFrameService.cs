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
    public class TimeFrameService : ITimeFrameService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly AppActionResult _result;
        public TimeFrameService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _result = new AppActionResult();
        }
        public async Task<TimeSpan[]> ConvertStringTimeFrameToTime(string timeFrame)
        {
            string[] time = timeFrame.Split('-');
            if (TimeSpan.TryParse(time[0], out TimeSpan start))
            {
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

        public async Task<AppActionResult> CreateTimeFrame(TimeFrameDto timeFrameDto)
        {
            try
            {

                bool isValid = true;
                if (await _unitOfWork.GetRepository<TimeFrame>().GetByExpression(t => t.TimeFrameName == timeFrameDto.TimeFrameName) != null)
                {
                    isValid = false;
                    _result.Message.Add($"The timeframe with name {timeFrameDto.TimeFrameName} is existed");
                }
                if (isValid)
                {
                    await _unitOfWork.GetRepository<TimeFrame>().Insert(_mapper.Map<TimeFrame>(timeFrameDto));
                    _unitOfWork.SaveChange();
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

        public async Task<AppActionResult> UpdateTimeFrame(TimeFrameDto timeFrameDto)
        {
            try
            {
                bool isValid = true;
                if (await _unitOfWork.GetRepository<TimeFrame>().GetById(timeFrameDto.TimeFrameId) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The timeframe with id {timeFrameDto.TimeFrameId} not found");
                }
                if (isValid)
                {
                    await _unitOfWork.GetRepository<TimeFrame>().Update(_mapper.Map<TimeFrame>(timeFrameDto));
                    _unitOfWork.SaveChange();
                    _result.Message.Add(SD.ResponeMessage.UPDATE_SUCCESS);
                }
                else
                {
                    _result.isSuccess = false;
                }
            }
            catch (Exception ex) {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;

        }
    }
}

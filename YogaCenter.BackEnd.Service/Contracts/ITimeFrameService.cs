using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;

namespace YogaCenter.BackEnd.Service.Contracts
{
    public interface ITimeFrameService
    {
        Task<AppActionResult> CreateTimeFrame(TimeFrameDto timeFrameDto);
        Task<AppActionResult> UpdateTimeFrame(TimeFrameDto timeFrameDto);
        Task<AppActionResult> GetTimeFrameById(int timeframeId);

        Task<TimeSpan[]> ConvertStringTimeFrameToTime(string timeFrame);
    }
}

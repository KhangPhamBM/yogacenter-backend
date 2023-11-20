using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto.Common;
using YogaCenter.BackEnd.Common.Dto.Request;
using YogaCenter.BackEnd.DAL.Models;

namespace YogaCenter.BackEnd.Service.Contracts
{
    public interface ITimeFrameService : ISearching<TimeFrame>
    {
        Task<AppActionResult> CreateTimeFrame(TimeFrameDto timeFrameDto);
        Task<AppActionResult> UpdateTimeFrame(TimeFrameDto timeFrameDto);
        Task<AppActionResult> GetTimeFrameById(int timeframeId);

        Task<TimeSpan[]> ConvertStringTimeFrameToTime(string timeFrame);
    }
}

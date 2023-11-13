using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Models;

namespace YogaCenter.BackEnd.Service.Contracts
{
    public interface IFeedbackService : ISearching<Feedback>
    {
        Task<AppActionResult> CreateFeedback(FeedbackDto Feedback);
        Task<AppActionResult> UpdateFeedback(FeedbackDto Feedback);
        Task<AppActionResult> GetFeedbackById(int id);
        Task<AppActionResult> GetAll(int pageIndex, int pageSize, IList<SortInfo> sortInfos);
        Task<AppActionResult> DeleteFeedback(int id);
    }
}

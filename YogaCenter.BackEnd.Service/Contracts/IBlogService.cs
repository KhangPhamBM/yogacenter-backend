using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto.Request;
using YogaCenter.BackEnd.Common.Dto.Response;
using YogaCenter.BackEnd.DAL.Models;

namespace YogaCenter.BackEnd.Service.Contracts
{
    public interface IBlogService : ISearching<Blog>
    {
        Task<AppActionResult> CreateBlog(BlogRequestDto Blog);
        Task<AppActionResult> UpdateBlog(BlogRequestDto Blog);
        Task<AppActionResult> GetBlogById(int id);
        Task<AppActionResult> GetAll(int pageIndex, int pageSize, IList<SortInfo> sortInfos);
        Task<AppActionResult> DeleteBlog(int id);
    }
}

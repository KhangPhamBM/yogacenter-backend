using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;

namespace YogaCenter.BackEnd.Service.Contracts
{
    public interface IDataPresentationService<T> where T : class
    {
     //   public Task<AppActionResult> ApplyPaging(IEnumerable<T> source, int pageIndex, int pageSize);

      //  public Task<AppActionResult> ApplyFiltering(BaseFilterRequest searchRequest);
    }
}

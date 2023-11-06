using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;

namespace YogaCenter.BackEnd.Service.Contracts
{
    public interface IClassDetailService
    {
        Task<AppActionResult> GetClassDetailsByClassId(int classId, int pageIndex, int pageSize, IList<SortInfo> sortInfos);
        Task<AppActionResult> RegisterClass(ClassDetailDto detail);

       // Task<AppActionResult> GetClassDetailByUserId(string userId);    

    }
}

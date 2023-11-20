using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto.Request;
using YogaCenter.BackEnd.DAL.Models;

namespace YogaCenter.BackEnd.Service.Contracts
{
    public interface IClassService
    {
        Task<AppActionResult> CreateClass(ClassRequest classDto);
        Task<AppActionResult> UpdateClass(ClassRequest classDto);
        Task<AppActionResult> GetClassById(int classId);
        Task<AppActionResult> GetAllClass(int pageIndex, int pageSize, IList<SortInfo> sortInfos);
        Task<AppActionResult> GetAllAvailableClass(int pageIndex, int pageSize, IList<SortInfo> sortInfos);
    }
}

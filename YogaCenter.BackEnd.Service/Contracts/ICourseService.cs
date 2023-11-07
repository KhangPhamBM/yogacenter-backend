using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Models;

namespace YogaCenter.BackEnd.Service.Contracts
{
    public interface ICourseService : ISearching<Course>
    {
        Task<AppActionResult> CreateCourse(CourseDto course);
        Task<AppActionResult> UpdateCourse(CourseDto course);
        Task<AppActionResult> GetCourseById(int id);
        Task<AppActionResult> GetAll(int pageIndex, int pageSize, IList<SortInfo> sortInfos);
        Task<AppActionResult> DeleteCourse(int id);

        //Task<AppActionResult> SearchApplyingSortingAndFiltering(BaseFilterRequest filterRequest);
    }
}

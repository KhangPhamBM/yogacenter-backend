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
    public interface ICourseService
    {
        Task<AppActionResult> CreateCourse(CourseDto course);
        Task<AppActionResult> UpdateCourse(CourseDto course);
        Task<AppActionResult> GetCourseById(int id);
        Task<AppActionResult> GetAll();
        Task<AppActionResult> DeleteCourse(int id);
        Task<AppActionResult> Filter(SearchCourseRequest searchCourseRequest);
        Task<AppActionResult> ApplyPaging(IEnumerable<CourseDto> source, int pageIndex, int pageSize);
    }
}

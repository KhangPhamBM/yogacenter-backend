using System;
using System.Collections.Generic;
using System.Linq;
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

    }
}

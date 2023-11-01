using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.Service.Contracts;

namespace YogaCenter.BackEnd.API.Controllers
{
    [Route("course")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly AppActionResult _responeDto;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
            _responeDto = new AppActionResult();
        }

        [HttpPost("create-course")]
        public async Task<AppActionResult> CreateCourse(CourseDto course)
        {
           return await _courseService.CreateCourse(course);  
        }

        [HttpPut("update-course")]
        public async Task<AppActionResult> UpdateCourse(CourseDto course)
        {
            return await _courseService.UpdateCourse(course);
        }

        [HttpGet]
        [Route("Get-course-by-id")]
        public async Task<AppActionResult> GetCourseById(int id)
        {
            return await _courseService.GetCourseById(id);
        }

        [HttpPost]
        [Route("Get-all-course")]
        public async Task<AppActionResult> GetAllCourse(int pageIndex, int pageSize, IList<SortInfo> sortInfos)
        {
            return await _courseService.GetAll(pageIndex, pageSize, sortInfos);
        }

        [HttpPost]
        [Route("delete-course")]
        public async Task<AppActionResult> DeleteCourse(int id)
        {
            return await _courseService.DeleteCourse(id);
        }

        [HttpPost]
        [Route("Get-course-with-searching")]
        public async Task<AppActionResult> GetCourseWithSearching(BaseFilterRequest baseFilterRequest)
        {
            return await _courseService.SearchApplyingSortingAndFiltering(baseFilterRequest);
        }
    }
}

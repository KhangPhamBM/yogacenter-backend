using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.DAL.Util;
using YogaCenter.BackEnd.Service.Contracts;

namespace YogaCenter.BackEnd.API.Controllers
{
    [Route("course")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;   
        

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;        }

        [HttpPost("create-course")]
        [Authorize(Roles = Permission.MANAGEMENT)]

        public async Task<AppActionResult> CreateCourse(CourseDto course)
        {
           return await _courseService.CreateCourse(course);  
        }

        [HttpPut("update-course")]
        [Authorize(Roles = Permission.MANAGEMENT)]

        public async Task<AppActionResult> UpdateCourse(CourseDto course)
        {
            return await _courseService.UpdateCourse(course);
        }

        [HttpGet("get-course-by-id/{id:int}")]

        public async Task<AppActionResult> GetCourseById(int id)
        {
            return await _courseService.GetCourseById(id);
        }

        [HttpPost]
        [Route("get-all-course")]
        public async Task<AppActionResult> GetAllCourse(int pageIndex, int pageSize, IList<SortInfo> sortInfos)
        {
            return await _courseService.GetAll(pageIndex, pageSize, sortInfos);
        }

        [HttpDelete("delete-course/{id:int}")]
        [Authorize(Roles = Permission.ADMIN)]

        public async Task<AppActionResult> DeleteCourse(int id)
        {
            return await _courseService.DeleteCourse(id);
        }

        [HttpPost("get-course-with-searching")]
        public async Task<AppActionResult> GetCourseWithSearching( BaseFilterRequest baseFilterRequest)
        {
            return await _courseService.SearchApplyingSortingAndFiltering(baseFilterRequest);
        }
    }
}

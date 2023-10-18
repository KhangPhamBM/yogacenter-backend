using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        [HttpPost("get-course-by-id/{id:int}")]
        public async Task<AppActionResult> GetCourseById(int id)
        {
            return await _courseService.GetCourseById(id);
        }


    }
}

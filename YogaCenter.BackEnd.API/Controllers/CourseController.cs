﻿using Microsoft.AspNetCore.Http;
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
        public AppActionResult CreateCourse(CourseDto course)
        {
            try
            {
                _courseService.CreateCourse(course);
                _responeDto.Data = true;
            }
            catch (Exception ex)
            {
                _responeDto.isSuccess = false;
            }
            return _responeDto;
        }



    }
}

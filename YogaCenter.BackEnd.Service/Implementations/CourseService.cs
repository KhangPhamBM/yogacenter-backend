using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.DAL.Util;
using YogaCenter.BackEnd.Service.Contracts;

namespace YogaCenter.BackEnd.Service.Implementations
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private readonly AppActionResult _result;
        public CourseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _result = new AppActionResult();
        }

        public async Task<AppActionResult> CreateCourse(CourseDto course)
        {
            bool isValid = true;
            if (await _unitOfWork.GetRepository<Course>().GetByExpression(c => c.CourseName == course.CourseName) !=null)
            {
                _result.Message.Add("The course is existed");
                isValid = false;
            }

            if (isValid)
            {
                await _unitOfWork.GetRepository<Course>().Insert(_mapper.Map<Course>(course));
                _unitOfWork.SaveChange();
                _result.Message.Add(SD.ResponeMessage.CREATE_SUCCESS);
            }
            else
            {
                _result.isSuccess = false;
            }
           return _result;
        }

        public async Task<AppActionResult> UpdateCourse(CourseDto course)
        {
            bool isValid = true;
            if (await _unitOfWork.GetRepository<Course>().GetById(course.CourseId) == null)
            {
                _result.Message.Add($"The course with id {course.CourseId} not found");
                isValid = false;
            }

            if (isValid)
            {
                await _unitOfWork.GetRepository<Course>().Update(_mapper.Map<Course>(course));
                _unitOfWork.SaveChange();
                _result.Message.Add(SD.ResponeMessage.UPDATE_SUCCESS);
            }
            else
            {
                _result.isSuccess = false;
            }
            return _result;
        }


    }
}

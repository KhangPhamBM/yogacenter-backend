using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.Service.Contracts;

namespace YogaCenter.BackEnd.Service.Implementations
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public CourseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateCourse(CourseDto course)
        {
            var courseDb = await _unitOfWork.GetRepository<Course>().GetById(course.CourseId);
            if (courseDb == null)
            {
                await _unitOfWork.GetRepository<Course>().Insert(_mapper.Map<Course>(course));
                _unitOfWork.SaveChange();
            }
        }

        public async Task UpdateCourse(CourseDto course)
        {
            var courseDb = await _unitOfWork.GetRepository<Course>().GetById(course.CourseId);
            if (courseDb != null)
            {
                await _unitOfWork.GetRepository<Course>().Update(_mapper.Map<Course>(course));
                _unitOfWork.SaveChange();
            }
        }


    }
}

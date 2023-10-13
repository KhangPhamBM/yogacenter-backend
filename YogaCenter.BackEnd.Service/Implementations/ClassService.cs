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
    public class ClassService : IClassService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClassDetailRepository _classDetailRepository;
        private readonly IMapper _mapper;
        public ClassService(IUnitOfWork unitOfWork, IClassDetailRepository classDetailRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _classDetailRepository = classDetailRepository;
            _mapper = mapper;
        }

      
        public async Task AddClass(ClassDto classDto)
        {
            var courseDb = await _unitOfWork.GetRepository<Course>().GetById(classDto.CourseId);
            var classDb = await _unitOfWork.GetRepository<Class>().GetById(classDto.ClassId);
            if (courseDb != null && classDb == null)
            {
                await _unitOfWork.GetRepository<Class>().Insert(_mapper.Map<Class>(classDto));
                _unitOfWork.SaveChange();
            }
        }
        public async Task UpdateClass(ClassDto classDto) 
        {
            var classDb = await _unitOfWork.GetRepository<Class>().GetById(classDto.ClassId);
            if (classDb != null)
            {
                await _unitOfWork.GetRepository<Class>().Update(_mapper.Map<Class>(classDto));
                _unitOfWork.SaveChange();
            }
        }

     

    }
}

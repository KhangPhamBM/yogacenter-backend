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
    public class ClassService : IClassService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClassDetailRepository _classDetailRepository;
        private readonly IMapper _mapper;
        private readonly AppActionResult _result;
        public ClassService(IUnitOfWork unitOfWork, IClassDetailRepository classDetailRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _classDetailRepository = classDetailRepository;
            _mapper = mapper;
            _result = new();
        }


        public async Task<AppActionResult> CreateClass(ClassRequest classDto)
        {
            try
            {
                bool isValid = true;
                if (await _unitOfWork.GetRepository<Course>().GetById(classDto.CourseId) == null)
                {
                    _result.Message.Add($"The course with id {classDto.CourseId} not found");
                    isValid = false;

                }
                if (await _unitOfWork.GetRepository<Class>().GetByExpression(c => c.ClassName == classDto.ClassName) != null)
                {
                    _result.Message.Add($"The class with name {classDto.CourseId} is exist");
                    isValid = false;

                }
                if (isValid)
                {
                    await _unitOfWork.GetRepository<Class>().Insert(_mapper.Map<Class>(classDto));
                    _unitOfWork.SaveChange();
                    _result.Message.Add(SD.ResponseMessage.CREATE_SUCCESSFUL);
                }
                else
                {
                    _result.isSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }

            return _result;
        }

        public async Task<AppActionResult> GetClassById(int classId)
        {
            try
            {
                bool isValid = true;

                if (await _unitOfWork.GetRepository<Class>().GetById(classId) == null)
                {
                    _result.Message.Add($"The class with id {classId} not found");
                    isValid = false;

                }
                if (isValid)
                {
                    _result.Data = await _unitOfWork.GetRepository<Class>().GetById(classId);
                }
                else
                {
                    _result.isSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> UpdateClass(ClassRequest classDto)
        {
            try
            {
                bool isValid = true;
                if (await _unitOfWork.GetRepository<Course>().GetById(classDto.CourseId) == null)
                {
                    _result.Message.Add($"The course with id {classDto.CourseId} not found");
                    isValid = false;

                }
                if (await _unitOfWork.GetRepository<Class>().GetById(classDto.ClassId) == null)
                {
                    _result.Message.Add($"The class with id {classDto.ClassId} not found");
                    isValid = false;

                }
                if (isValid)
                {
                    await _unitOfWork.GetRepository<Class>().Update(_mapper.Map<Class>(classDto));
                    _unitOfWork.SaveChange();
                    _result.Message.Add(SD.ResponseMessage.UPDATE_SUCCESSFUL);
                }
                else
                {
                    _result.isSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> GetAllClass()
        {
            try
            {
                _result.Data = _mapper.Map<IEnumerable<ClassRequest>>(await _unitOfWork.GetRepository<Class>().GetAll());
            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> GetAllAvailableClass()
        {
            try
            {
                _result.Data = await _unitOfWork.GetRepository<Class>().GetByExpression(c => !(bool)c.IsDeleted);
            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }
    }
}

using AutoMapper;
using NPOI.Util;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
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
    public class ClassService : GenericBackendService,IClassService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClassRepository _classRepository;
        private readonly IMapper _mapper;
        private readonly AppActionResult _result;
        public ClassService(
            IUnitOfWork unitOfWork, 
            IClassRepository classRepository, 
            IMapper mapper,
            IServiceProvider serviceProvider
            ):base(serviceProvider)


        {
            _unitOfWork = unitOfWork;
            _classRepository = classRepository;
            _mapper = mapper;
            _result = new();
        }


        public async Task<AppActionResult> CreateClass(ClassRequest classDto)
        {
            try
            {
                var courseRepository = Resolve<ICourseRepository>();
                bool isValid = true;
                if (await courseRepository.GetById(classDto.CourseId) == null)
                {
                    _result.Message.Add($"The course with id {classDto.CourseId} not found");
                    isValid = false;

                }
                if (await _classRepository.GetByExpression(c => c.ClassName == classDto.ClassName) != null)
                {
                    _result.Message.Add($"The class with name {classDto.CourseId} is exist");
                    isValid = false;

                }
                if (isValid)
                {

                    await  _classRepository.Insert(_mapper.Map<Class>(classDto));
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

                if (await _classRepository.GetById(classId) == null)
                {
                    _result.Message.Add($"The class with id {classId} not found");
                    isValid = false;

                }
                if (isValid)
                {
                    _result.Result.Data = await _classRepository.GetById(classId);
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
                var courseRepository = Resolve<ICourseRepository>();
                if (await courseRepository.GetById(classDto.CourseId) == null)
                {
                    _result.Message.Add($"The course with id {classDto.CourseId} not found");
                    isValid = false;

                }
                if (await _classRepository.GetById(classDto.ClassId) == null)
                {
                    _result.Message.Add($"The class with id {classDto.ClassId} not found");
                    isValid = false;

                }
                if (isValid)
                {
                    await _classRepository.Update(_mapper.Map<Class>(classDto));
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

        public async Task<AppActionResult> GetAllClass(int pageIndex, int pageSize, IList<SortInfo> sortInfos)
        {
            try
            {
                var classes = await _classRepository.GetAll();
                if (pageIndex <= 0) pageIndex = 1;
                if (pageSize <= 0) pageSize = SD.MAX_RECORD_PER_PAGE;
                int totalPage = DataPresentationHelper.CalculateTotalPageSize(classes.Count(), pageSize);
                if (sortInfos != null)
                {
                    classes = DataPresentationHelper.ApplySorting(classes, sortInfos);
                }
                if (pageIndex > 0 && pageSize > 0)
                {
                    classes = DataPresentationHelper.ApplyPaging(classes, pageIndex, pageSize);
                }
                _result.Result.Data = classes;
                _result.Result.TotalPage = totalPage;
            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> GetAllAvailableClass(int pageIndex, int pageSize, IList<SortInfo> sortInfos)
        {
            try
            {
                var classes = await _classRepository.GetListByExpression(c => c.IsDeleted == false, null);
                if (pageIndex <= 0) pageIndex = 1;
                if (pageSize <= 0) pageSize = SD.MAX_RECORD_PER_PAGE;
                int totalPage = DataPresentationHelper.CalculateTotalPageSize(classes.Count(), pageSize);

                //var classes =  _mapper.Map<IOrderedQueryable<ClassDto>>(src);
                if (sortInfos != null)
                {
                    classes = DataPresentationHelper.ApplySorting(classes, sortInfos);
                }
                if (pageIndex > 0 && pageSize > 0)
                {
                    classes = DataPresentationHelper.ApplyPaging(classes, pageIndex, pageSize);
                }
                _result.Result.Data = classes;
                _result.Result.TotalPage = totalPage;
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


using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using NPOI.SS.Formula.Functions;
using NPOI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.DAL.Util;
using YogaCenter.BackEnd.Service.Contracts;

namespace YogaCenter.BackEnd.Service.Implementations
{
    public class CourseService : GenericBackendService, ICourseService
    {

        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private readonly AppActionResult _result;
        private ICourseRepository _courseRepository;
        public CourseService
            (IUnitOfWork unitOfWork,
            IMapper mapper,
            ICourseRepository courseRepository,
            IServiceProvider serviceProvider
            ) : base(serviceProvider)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _courseRepository = courseRepository;
            _result = new AppActionResult();
        }

        public async Task<AppActionResult> CreateCourse(CourseDto course)
        {
            try
            {
                bool isValid = true;
                if (await _courseRepository.GetByExpression(c => c.CourseName == course.CourseName, null) != null)
                {
                    _result.Message.Add("The course is existed");
                    isValid = false;
                }

                if (isValid)
                {
                    var courseDB = _mapper.Map<Course>(course);
                    await _courseRepository.Insert(courseDB);
                    _unitOfWork.SaveChange();

                    var pathFileName = SD.FirebasePathName.COURSE_PREFIX + $"{courseDB.CourseId}.jpg";
                    courseDB.CourseImageUrl = pathFileName;
                    var fileService = Resolve<IFileService>();
                    var upload = await fileService.UploadImageToFirebase(course.CourseImage, pathFileName);
                    if (upload.isSuccess && upload.Result.Data != null)
                    {
                        _result.Message.Add("Upload firebase successful");

                    }
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
            finally
            {
                _unitOfWork.SaveChange();
            }
            return _result;
        }

        public async Task<AppActionResult> UpdateCourse(CourseDto course)
        {
            try
            {
                var fileService = Resolve<IFileService>();

                bool isValid = true;
                var courseDB = await _courseRepository.GetById(course.CourseId);
                if (courseDB == null)
                {
                    _result.Message.Add($"The course with id {course.CourseId} not found");
                    isValid = false;
                }

                if (isValid)
                {
                    var imgUrl = courseDB.CourseImageUrl;
                    await fileService.DeleteImageFromFirebase(imgUrl);
                    await fileService.UploadImageToFirebase(course.CourseImage, imgUrl);
                    _mapper.Map(course, courseDB);
                    courseDB.CourseImageUrl = imgUrl;
                    await _courseRepository.Update(courseDB);
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

        public async Task<AppActionResult> GetCourseById(int id)
        {
            try
            {
                bool isValid = true;
                if (await _courseRepository.GetById(id) == null)
                {
                    _result.Message.Add($"The course with id {id} not found");
                    isValid = false;
                }

                if (isValid)
                {
                    _result.Result.Data = await _courseRepository.GetById(id);
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

        public async Task<AppActionResult> GetAll(int pageIndex, int pageSize, IList<SortInfo> sortInfos)
        {
            try
            {
                var courseList = await _courseRepository.GetAll();
                if (courseList.Any())
                {
                    if (pageIndex <= 0) pageIndex = 1;
                    if (pageSize <= 0) pageSize = SD.MAX_RECORD_PER_PAGE;
                    int totalPage = DataPresentationHelper.CalculateTotalPageSize(courseList.Count(), pageSize);

                    if (sortInfos != null)
                    {
                        courseList = DataPresentationHelper.ApplySorting(courseList, sortInfos);
                    }
                    if (pageIndex > 0 && pageSize > 0)
                    {
                        courseList = DataPresentationHelper.ApplyPaging(courseList, pageIndex, pageSize);
                    }
                    _result.Result.Data = courseList;
                    _result.Result.TotalPage = totalPage;
                }
                else
                {
                    _result.Message.Add("Empty course list");
                }
            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> DeleteCourse(int id)
        {
            try
            {
                bool isValid = true;
                if (id < 0)
                {
                    _result.Message.Add("Invalid CourseId");
                    isValid = false;
                    _result.isSuccess = false;
                }
                if (isValid)
                {
                    var course = _courseRepository.GetById(id);
                    if (course.Result != null && !(bool)course.Result.IsDeleted)
                    {
                        course.Result.IsDeleted = true;
                        await _courseRepository.Update(course.Result);
                        _unitOfWork.SaveChange();
                    }
                    else
                    {
                        _result.isSuccess = false;
                        _result.Message.Add($"Course with id: {id} not found");
                    }
                }
            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> SearchApplyingSortingAndFiltering(BaseFilterRequest filterRequest)
        {
            try
            {
                var source = await _courseRepository.GetListByExpression(c => (bool)!c.IsDeleted, null);
                int pageIndex = filterRequest.pageIndex;
                if (pageIndex <= 0) pageIndex = 1;
                int pageSize = filterRequest.pageSize;
                if (pageSize <= 0) pageSize = SD.MAX_RECORD_PER_PAGE;
                int totalPage = DataPresentationHelper.CalculateTotalPageSize(source.Count(), pageSize);
                if (filterRequest != null)
                {
                    if (filterRequest.pageIndex <= 0 || filterRequest.pageSize <= 0)
                    {
                        _result.Message.Add($"Invalid value of pageIndex or pageSize");
                        _result.isSuccess = false;
                    }
                    else
                    {
                        if (filterRequest.keyword != "" && filterRequest.keyword != null)
                        {
                            source = await _courseRepository.GetListByExpression(c => (bool)!c.IsDeleted && c.CourseName.Contains(filterRequest.keyword), null);
                        }
                        if (filterRequest.filterInfoList != null)
                        {
                            source = DataPresentationHelper.ApplyFiltering(source, filterRequest.filterInfoList);
                        }
                        totalPage = DataPresentationHelper.CalculateTotalPageSize(source.Count(), filterRequest.pageSize);

                        if (filterRequest.sortInfoList != null)
                        {
                            source = DataPresentationHelper.ApplySorting(source, filterRequest.sortInfoList);
                        }
                        source = DataPresentationHelper.ApplyPaging(source, filterRequest.pageIndex, filterRequest.pageSize);
                        _result.Result.Data = source;

                    }
                }
                else
                {
                    _result.Result.Data = source;
                }
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
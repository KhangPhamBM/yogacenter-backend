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
using System.Web.Mvc;
using System.Web.WebPages;
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
            try
            {
                bool isValid = true;
                if (await _unitOfWork.GetRepository<Course>().GetByExpression(c => c.CourseName == course.CourseName, null) != null)
                {
                    _result.Message.Add("The course is existed");
                    isValid = false;
                }

                if (isValid)
                {
                    await _unitOfWork.GetRepository<Course>().Insert(_mapper.Map<Course>(course));
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

        public async Task<AppActionResult> UpdateCourse(CourseDto course)
        {
            try
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
            try {
                bool isValid = true;
                if (await _unitOfWork.GetRepository<Course>().GetById(id) == null)
                {
                    _result.Message.Add($"The course with id {id} not found");
                    isValid = false;
                }

                if (isValid)
                {
                    _result.Result.Data = await _unitOfWork.GetRepository<Course>().GetById(id);
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
                var courseList = await _unitOfWork.GetRepository<Course>().GetAll();
                if (courseList.Any())
                {
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
                    var course = _unitOfWork.GetRepository<Course>().GetById(id);
                    if (course.Result != null && !(bool)course.Result.IsDeleted)
                    {
                        course.Result.IsDeleted = true;
                        await _unitOfWork.GetRepository<Course>().Update(course.Result);
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
                var source = await _unitOfWork.GetRepository<Course>().GetListByExpression(c => (bool)!c.IsDeleted, null);
                int totalPage = DataPresentationHelper.CalculateTotalPageSize(source.Count(), filterRequest.pageSize);
                if (filterRequest != null)
                {
                    if (filterRequest.pageIndex <= 0 || filterRequest.pageSize <= 0)
                    {
                        _result.Message.Add($"Invalid value of pageIndex or pageSize");
                        _result.isSuccess=false;
                    } else
                    {
                        if (!filterRequest.keyword.IsEmpty() && filterRequest.keyword != null)
                        {
                            source = await _unitOfWork.GetRepository<Course>().GetListByExpression(c => (bool)!c.IsDeleted && c.CourseName.Contains(filterRequest.keyword), null);
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
                } else
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
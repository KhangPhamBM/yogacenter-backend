using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
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
                if (await _unitOfWork.GetRepository<Course>().GetByExpression(c => c.CourseName == course.CourseName) != null)
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
                    _result.Message.Add(SD.ResponeMessage.UPDATE_SUCCESS);
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
                if(id < 0)
                {
                    _result.Message.Add("Invalid CourseId");
                    isValid = false;
                    _result.isSuccess = false;
                }
                if(isValid)
                {
                    var course = _unitOfWork.GetRepository<Course>().GetById(id);
                    if(course.Result != null)
                    {
                        _result.Data = course.Result;
                    } else
                    {
                        _result.isSuccess = false;
                        _result.Message.Add($"Course with id: {id} not found");
                    }
                }
            } catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> GetAll()
        {
            try
            {
                var courseList = _unitOfWork.GetRepository<Course>().GetAll();
                if(courseList.Result.Any()) { 
                    _result.Data = courseList.Result;
                } else
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

        public async Task<AppActionResult> ApplyPaging(IEnumerable<CourseDto> source, int pageIndex, int pageSize)
        {
            try
            {
                int toSkip = (pageIndex - 1) * pageSize;
                _result.Data = source.Skip(toSkip).Take(pageSize).ToList();
            } catch (Exception ex) {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> Filter(SearchCourseRequest searchCourseRequest)
        {
            try
            {
                var sortList = searchCourseRequest.sortInfoList; 
                var filterList = searchCourseRequest.filterInfoList;
                Expression<Func<CourseDto, bool>> combinedExpression = c => c.IsDeleted == false && (c.CourseName != null && c.CourseName.Contains(searchCourseRequest.searchKeyWord));                
                if (filterList != null)
                {
                    foreach (var filterInfo in filterList)
                    {
                        Expression<Func<CourseDto, bool>> subFilter = null;
                        if (filterInfo is FilterInfoToRange toRange)
                        {
                            subFilter = CreateRangeFilterExpression(toRange);
                        }
                        else if (filterInfo is FilterInfoToValue toValue)
                        {
                            subFilter = CreateValueFilterExpression(toValue);
                        }
                        
                        if(subFilter!= null)
                        {
                            var invokedExpr = Expression.Invoke(subFilter, combinedExpression.Parameters);
                            combinedExpression = Expression.Lambda<Func<CourseDto, bool>>(
                                Expression.AndAlso(invokedExpr, combinedExpression.Body),
                                combinedExpression.Parameters);
                        }
                    }
                }
                var result = await _unitOfWork.GetRepository<CourseDto>().GetListByExpression(combinedExpression);
                if (sortList != null && sortList.Count > 0)
                {
                    result = ApplySorting((IQueryable<CourseDto>)result, sortList);
                }
                _result.Data = result;
            }

            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }

        private Expression<Func<CourseDto, bool>> CreateRangeFilterExpression(FilterInfoToRange filterInfoToRange)
        {
            var parameter = Expression.Parameter(typeof(CourseDto), "c");
            var property = Expression.PropertyOrField(parameter, filterInfoToRange.fieldName);
            var minValue = Expression.Constant(filterInfoToRange.minValue);
            var maxValue = Expression.Constant(filterInfoToRange.maxValue);

            var greaterThanOrEqual = Expression.GreaterThanOrEqual(property, minValue);
            var lessThanOrEqual = Expression.LessThanOrEqual(property, maxValue);
            var andAlso = Expression.AndAlso(greaterThanOrEqual, lessThanOrEqual);

            return Expression.Lambda<Func<CourseDto, bool>>(andAlso, parameter);
        }

        private Expression<Func<CourseDto, bool>> CreateValueFilterExpression(FilterInfoToValue filterInfoToValue)
        {
            var parameter = Expression.Parameter(typeof(CourseDto), "c");
            var conjunctions = new List<Expression>();

            var fieldName = filterInfoToValue.fieldName;
            var filterValues = filterInfoToValue.filterValues;

            if (filterValues is IEnumerable<object> values)
            {
                // Create equality expressions for each value
                foreach (var filterValue in values)
                {
                    var value = Expression.Constant(filterValue);
                    var property = Expression.Property(parameter, fieldName);
                    var equality = Expression.Equal(property, value);
                    conjunctions.Add(equality);
                }
            }
            else
            {
                throw new InvalidOperationException("filterValues must be of type IEnumerable<object> for filtering.");
            }

            // Use OR for the same field name and AND for different field names
            var combinedFilter = conjunctions.Aggregate((current, next) => Expression.Or(current, next));

            return Expression.Lambda<Func<CourseDto, bool>>(combinedFilter, parameter);
        }

        private IOrderedQueryable<CourseDto> ApplySorting(IQueryable<CourseDto> filteredData, IList<SortInfo> sortingList) {
            IOrderedQueryable<CourseDto> orderedQuery = filteredData as IOrderedQueryable<CourseDto>;

            if (orderedQuery == null)
            {
                orderedQuery = (IOrderedQueryable<CourseDto>?)filteredData.OrderBy(x => 0); // Order by a constant to initiate sorting.
            }

            foreach (var sortInfo in sortingList)
            {
                var property = typeof(CourseDto).GetProperty(sortInfo.fieldName);

                if (property == null)
                {
                    throw new ArgumentException($"Property '{sortInfo.fieldName}' not found in type '{typeof(CourseDto).FullName}'.");
                }

                Expression<Func<CourseDto, object>> expression = x => property.GetValue(x);

                if (sortInfo.ascending)
                {
                    orderedQuery = orderedQuery.ThenBy(expression);
                }
                else
                {
                    orderedQuery = orderedQuery.ThenByDescending(expression);
                }
            }

            return orderedQuery;

        }

        
    }
}
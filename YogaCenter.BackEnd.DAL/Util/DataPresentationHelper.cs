using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;

namespace YogaCenter.BackEnd.DAL.Util
{
    public class DataPresentationHelper
    {
        public static IQueryable<T> ApplyFiltering<T>(IQueryable<T> source, IList<FilterInfo> filterList)
        {
            if (source == null || source.Count() == 0)
            {
                return source; 
            }
                Expression<Func<T, bool>> combinedExpression = t => true; 
                if (filterList != null)
                {
                    foreach (var filterInfo in filterList)
                    {
                        Expression<Func<T, bool>> subFilter = null;
                        if (filterInfo is FilterInfoToRange toRange)
                        {
                            subFilter = CreateRangeFilterExpression<T>(toRange);
                        }
                        else if (filterInfo is FilterInfoToValue toValue)
                        {
                            subFilter = CreateValueFilterExpression<T>(toValue);
                        }

                        if (subFilter != null)
                        {
                            var invokedExpr = Expression.Invoke(subFilter, combinedExpression.Parameters);
                            combinedExpression = Expression.Lambda<Func<T, bool>>(
                                Expression.AndAlso(invokedExpr, combinedExpression.Body),
                                combinedExpression.Parameters);
                        }
                    }
                    var result = (IQueryable<T>)source.Where(combinedExpression);
                    return result;
                }
            return source;
        }

        public static IQueryable<T> ApplyPaging<T>(IQueryable<T> source, int pageIndex, int pageSize)
        {
             int toSkip = (pageIndex - 1) * pageSize;
             return source.Skip(toSkip).Take(pageSize);
        }

        private static Expression<Func<T, bool>> CreateRangeFilterExpression<T>(FilterInfoToRange filterInfoToRange)
        {
            var parameter = Expression.Parameter(typeof(T), "c");
            var property = Expression.PropertyOrField(parameter, filterInfoToRange.fieldName);
            var minValue = Expression.Constant(filterInfoToRange.minValue);
            var maxValue = Expression.Constant(filterInfoToRange.maxValue);

            var greaterThanOrEqual = Expression.GreaterThanOrEqual(property, minValue);
            var lessThanOrEqual = Expression.LessThanOrEqual(property, maxValue);
            var andAlso = Expression.AndAlso(greaterThanOrEqual, lessThanOrEqual);

            return Expression.Lambda<Func<T, bool>>(andAlso, parameter);
        }

        private static Expression<Func<T, bool>> CreateValueFilterExpression<T>(FilterInfoToValue filterInfoToValue)
        {
            var parameter = Expression.Parameter(typeof(T), "c");
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

            return Expression.Lambda<Func<T, bool>>(combinedFilter, parameter);
        }

        public static IOrderedQueryable<T> ApplySorting<T>(IQueryable<T> filteredData, IList<SortInfo> sortingList)
        {
            IOrderedQueryable<T> orderedQuery = filteredData as IOrderedQueryable<T>;

            if (orderedQuery == null)
            {
                orderedQuery = filteredData.OrderBy(x => 0); // Order by a constant to initiate sorting.
            }

            foreach (var sortInfo in sortingList)
            {
                var property = typeof(T).GetProperty(sortInfo.fieldName);

                if (property == null)
                {
                    throw new ArgumentException($"Property '{sortInfo.fieldName}' not found in type '{typeof(T).FullName}'.");
                }

                Expression<Func<T, object>> expression = x => property.GetValue(x);

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

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.DAL.Contracts
{
    public interface IRepository<T> where T : class
    {
        Task< IEnumerable<T>> GetAll();
        Task<T> GetById(object id);
        Task<T> GetByExpression(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includeProperties);
        Task<IEnumerable<T>> GetListByExpression(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includeProperties);
        Task<T> Insert(T entity);
        Task<IEnumerable<T>> InsertRange(IEnumerable<T> entities);
        Task Update(T entity);
        Task DeleteById(object id);

    }
}

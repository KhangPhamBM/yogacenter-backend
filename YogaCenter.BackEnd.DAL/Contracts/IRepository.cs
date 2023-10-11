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
        IQueryable<T> GetAll();
         Task<T> GetById(object id);
        Task Insert(T entity);
        Task Update(T entity);
        Task DeleteById(object id);
    }
}

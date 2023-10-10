using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Data;

namespace YogaCenter.BackEnd.DAL.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly YogaCenterContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(YogaCenterContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task<T> GetById(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task Insert(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public Task Update(T entity)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        public Task DeleteById(object id)
        {
            T entityToDelete = _dbSet.Find(id);
            if (entityToDelete != null)
            {
                _dbSet.Remove(entityToDelete);

            }
            return Task.CompletedTask;

        }

      
    }
}

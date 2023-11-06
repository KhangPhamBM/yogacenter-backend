﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public async Task<IOrderedQueryable<T>> GetAll()
        {
            return (IOrderedQueryable<T>)_dbSet.AsNoTracking();
        }

        public async Task<T> GetById(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> Insert(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task Update(T entity)
        {
            _dbSet.Update(entity);
          
        }

        public async Task DeleteById(object id)
        {
            T entityToDelete = await _dbSet.FindAsync(id);
            if (entityToDelete != null)
            {
                _dbSet.Remove(entityToDelete);

            }

        }

        public async Task<IOrderedQueryable<T>> GetListByExpression(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = _dbSet.AsQueryable();

            // Apply eager loading
            if(includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }

            }

            if (filter == null && includeProperties.Length > 0)
            {
                return (IOrderedQueryable<T>)await query.ToListAsync();
            }

            return (IOrderedQueryable<T>)query.Where(filter);
        }

        public async Task<T> GetByExpression(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includeProperties)
        {
            if(includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    _dbSet.Include(includeProperty);
                }
            }
          
            return await _dbSet.SingleOrDefaultAsync(filter);
        }

        public async Task<IEnumerable<T>> InsertRange(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
            return entities;
        }
    }
}

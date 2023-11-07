using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Data;

namespace YogaCenter.BackEnd.DAL.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly YogaCenterContext _db;

        public UnitOfWork(YogaCenterContext db)
        {
            _db = db;
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return new Repository<TEntity>(_db);
        }

        public void SaveChange()
        {
             _db.SaveChanges();
        }
    }
}

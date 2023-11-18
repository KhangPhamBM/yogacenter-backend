using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Data;
using YogaCenter.BackEnd.DAL.Models;

namespace YogaCenter.BackEnd.DAL.Implementations
{
    public class ClassDetailRepository : Repository<ClassDetail>, IClassDetailRepository
    {
        public ClassDetailRepository(YogaCenterContext context) : base(context)
        {
        }



        //public async Task<ClassDetail> GetByClassIdAndUserId(ClassDetail classDetail)
        //{
        //    return await _context.ClassDetails.SingleOrDefaultAsync(c => c.UserId == classDetail.UserId && c.ClassId == classDetail.ClassId);
        //}

        public Expression<Func<ClassDetail, bool>> GetByClassIdAndUserId(ClassDetail classDetail)
        {
            return (c => c.UserId == classDetail.UserId && c.ClassId == classDetail.ClassId );
        }

        public  Expression<Func<ClassDetail, bool>> GetClassDetailByUserId(string UserId)
        {
            return (c => c.UserId == UserId);
        }
    }
}

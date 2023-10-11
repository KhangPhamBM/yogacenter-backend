using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Data;
using YogaCenter.BackEnd.DAL.Models;

namespace YogaCenter.BackEnd.DAL.Implementations
{
    public class ClassDetailRepository : Repository<ClassDetail>, IClassDetailRepository
    {
        private readonly YogaCenterContext _context;

        public ClassDetailRepository(YogaCenterContext context) : base(context)
        {
            _context = context; 
        }

        public async Task<ClassDetail> GetByClassIdAndUserId(ClassDetail classDetail)
        {
            return await _context.ClassDetails.SingleOrDefaultAsync(c => c.UserId == classDetail.UserId && c.ClassId == classDetail.ClassId);
        }

        public async Task<ClassDetail> GetClassDetailByUserId(string UserId)
        {
            return await _context.ClassDetails.FirstOrDefaultAsync(c => c.UserId == UserId);
        }
    }
}

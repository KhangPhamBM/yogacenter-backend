﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.DAL.Models;

namespace YogaCenter.BackEnd.DAL.Contracts
{
    public interface IClassDetailRepository: IRepository<ClassDetail>
    {
        public Expression<Func<ClassDetail, bool>> GetByClassIdAndUserId(ClassDetail classDetail);


        public Expression<Func<ClassDetail, bool>> GetClassDetailByUserId(string UserId);
        
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Data;
using YogaCenter.BackEnd.DAL.Models;

namespace YogaCenter.BackEnd.DAL.Implementations
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(YogaCenterContext context) : base(context)
        {
        }
    }
}

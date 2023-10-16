using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Data;
using YogaCenter.BackEnd.DAL.Models;

namespace YogaCenter.BackEnd.DAL.Implementations
{
    public class AttendanceRepository : Repository<Attendance>, IAttendanceRepository
    {
        private readonly YogaCenterContext _context;
        public AttendanceRepository(YogaCenterContext context, IMapper mapper) : base(context)
        {
        }
    }
}

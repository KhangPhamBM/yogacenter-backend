using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.DAL.Models;

namespace YogaCenter.BackEnd.DAL.Contracts
{
    public interface IClassDetailRepository: IRepository<ClassDetail>
    {
        Task<ClassDetail> GetByClassIdAndUserId(ClassDetail classDetail);
        Task<ClassDetail> GetClassDetailByUserId(string UserId);

    }
}

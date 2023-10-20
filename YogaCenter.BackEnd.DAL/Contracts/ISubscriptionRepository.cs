using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.DAL.Models;

namespace YogaCenter.BackEnd.DAL.Contracts
{
    public interface ISubscriptionRepository: IRepository<Subscription>
    {
        Task<IEnumerable<Subscription>> getSubscriptionByUserIdAndClassId(string userId, int ClassId);
        Task<IEnumerable<Subscription>> getSubcriptionByUserId(string userId);
    }
}

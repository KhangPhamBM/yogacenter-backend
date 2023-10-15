using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.DAL.Models;

namespace YogaCenter.BackEnd.DAL.Contracts
{
    public interface ISubscriptionRepository: IRepository<Subscription>
    {
        Task<IEnumerable<Subscription>> getSubscriptionByUserIdAndClassId(string userId, string ClassId);
        Task<IEnumerable<Subscription>> getSubcriptionByUserId(string userId);
    }
}

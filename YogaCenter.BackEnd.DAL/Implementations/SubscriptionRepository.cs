using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Data;
using YogaCenter.BackEnd.DAL.Models;

namespace YogaCenter.BackEnd.DAL.Implementations
{
    public class SubscriptionRepository : Repository<Subscription>,ISubscriptionRepository
    {
        YogaCenterContext _context;
        public SubscriptionRepository(YogaCenterContext context) : base(context)
        {
            _context = context; 
        }

        public async Task<IEnumerable<Subscription>> getSubcriptionByUserId(string userId)
        {
            var subscriptionList = _context.Subscriptions.Where(s => s.UserId == userId).ToList();
            return subscriptionList;
        }

        public async Task<IEnumerable<Subscription>> getSubscriptionByUserIdAndClassId(string userId, int ClassId)
        {
            var subscriptionList = _context.Subscriptions.Where(s => s.UserId == userId && s.ClassId == ClassId).ToList();
            return subscriptionList;
        }

    }
}

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
    public class PaymentResponseRepository : Repository<PaymentRespone>,IPaymentResponseRepository
    {
        YogaCenterContext _context;

      
        public PaymentResponseRepository(YogaCenterContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PaymentRespone>> GetPaymentResponsesByMonth(int year, int month)
        {
            return GetPaymentResponsesByYear(year).Result.Where(p => p.Subscription?.SubscriptionDate.Month == month).ToList();
        }

        public async Task<IEnumerable<PaymentRespone>> GetPaymentResponsesByUserId(string UserId)
        {
            var subscriptionList = _context.Subscriptions.Where(s => s.UserId.Equals(UserId)).ToList();
            if(subscriptionList.Any())
            {
                var userPayments = new List<PaymentRespone>();
                foreach(var subscription in subscriptionList)
                {
                    var paymentRespone = _context.PaymentRespones.FirstOrDefault(p => p.SubscriptionId == subscription.SubscriptionId);
                    if(paymentRespone != null)
                    {
                        userPayments.Add(paymentRespone);
                    }
                }
                return userPayments;
            }
            return new List<PaymentRespone>();
        }

        public async Task<IEnumerable<PaymentRespone>> GetPaymentResponsesByYear(int year)
        {
            return _context.PaymentRespones.Where(p => p.Subscription.SubscriptionDate.Year == year).ToList();
        }
    }
}

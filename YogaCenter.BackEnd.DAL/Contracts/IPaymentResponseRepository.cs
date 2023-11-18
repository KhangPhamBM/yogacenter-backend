using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.DAL.Models;

namespace YogaCenter.BackEnd.DAL.Contracts
{
    public interface IPaymentResponseRepository: IRepository<PaymentRespone>
    {
        Task<IEnumerable<PaymentRespone>> GetPaymentResponsesByUserId(string UserId);
        Task<IEnumerable<PaymentRespone>> GetPaymentResponsesByYear(int year);
        Task<IEnumerable<PaymentRespone>> GetPaymentResponsesByMonth(int year,int month);



    }
}

using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Data;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.DAL.Util;
using YogaCenter.BackEnd.Service.Contracts;

namespace YogaCenter.BackEnd.Service.Services
{
    public class WorkerService
    {
        private static ILogger<WorkerService> _logger;
        private IServiceProvider _serviceProvider;

        public WorkerService(ILogger<WorkerService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }


        /*
       Trong đó:
       0: Phút (chạy vào phút thứ 0 của mỗi giờ).
       12: Giờ (chạy vào giờ 12 trưa).
       *: Mỗi ngày trong tháng.
       *: Mọi tháng.
       *: Mọi ngày trong tuần.
       */

        [Obsolete]
        public void Main()
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    var _emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                    TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                    DateTime vietnamTime = TimeZoneInfo.ConvertTime(DateTime.Now, vietnamTimeZone);
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    await PaymentReminder(_unitOfWork, _emailService, vietnamTime);
                }
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);


            }
        }

        private static async Task PaymentReminder(IUnitOfWork _unitOfWork, IEmailService _emailService, DateTime vietnamTime)
        {
            
            var list = await _unitOfWork.GetRepository<Subscription>().GetListByExpression(s => s.SubscriptionStatusId == SD.Subscription.PENDING, s => s.User, s => s.Class);
            foreach (var item in list)
            {
                if(vietnamTime > item.SubscriptionDate)
                {
                    _emailService.SendEmail(item.User.Email, "NHAC NHO THANH TOAN", $"BAN CHUA THANH TOAN KIAAAA. THANH TOAN KHOA HOC THUOC LOP {item.Class.ClassName} GIA LA {item.Total}");
                }
            }
        }
       

    }
}

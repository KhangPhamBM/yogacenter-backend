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
using YogaCenter.BackEnd.Service.Implementations;

namespace YogaCenter.BackEnd.Service.Services
{
    public class WorkerService : GenericBackendService
    {
        private static ILogger<WorkerService> _logger;

        public WorkerService(ILogger<WorkerService> logger, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _logger = logger;
        }


        public void Start()
        {
            TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");


            RecurringJob.AddOrUpdate(() => PaymentReminder(), Cron.DayInterval(1), vietnamTimeZone);

        }

        public async Task PaymentReminder()
        {
            var _unitOfWork = Resolve<IUnitOfWork>();
            var _emailService = Resolve<IEmailService>();
            var subcriptionRepository = Resolve<ISubscriptionRepository>();
            TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime _vietnamTime = TimeZoneInfo.ConvertTime(DateTime.Now, vietnamTimeZone);
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            var list = await subcriptionRepository.GetListByExpression(s => s.SubscriptionStatusId == SD.Subscription.PENDING, s => s.User, s => s.Class);
            foreach (var item in list)
            {
                if (_vietnamTime > item.SubscriptionDate)
                {
                    _emailService.SendEmail(item.User.Email, "NHAC NHO THANH TOAN", $"BAN CHUA THANH TOAN KIAAAA. THANH TOAN KHOA HOC THUOC LOP {item.Class.ClassName} GIA LA {item.Total}");
                }
            }

        }
    }
}

using AutoMapper;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.Common.Dto.Common;
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
        private IMapper _mapper;

        public WorkerService(ILogger<WorkerService> logger, IServiceProvider serviceProvider, IMapper mapper) : base(serviceProvider)
        {
            _logger = logger;
            _mapper = mapper;
        }
        public void Start()
        {
            TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            RecurringJob.AddOrUpdate(() => PaymentReminder(), Cron.DayInterval(1), vietnamTimeZone);
            RecurringJob.AddOrUpdate(() => ScheduleReminder(), Utility.ConvertToCronExpression(6, 45), vietnamTimeZone);
        }
        public async Task PaymentReminder()
        {
            var _emailService = Resolve<IEmailService>();
            var paymentService = Resolve<IPaymentService>();
            var subcriptionRepository = Resolve<ISubscriptionRepository>();
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            DateTime _vietnamTime = Utility.GetInstance().GetCurrentDateTimeInTimeZone();
            var list = await subcriptionRepository
                .GetListByExpression(s => s.SubscriptionDate < _vietnamTime &&
                s.SubscriptionStatusId == SD.Subscription.PENDING, s => s.User, s => s.Class);
            foreach (var item in list)
            {
                string urlPayment = await paymentService.CreatePaymentUrlMomo(_mapper.Map<SubscriptionDto>(item));
                _emailService.SendEmail
                (
                item.User.Email,
                "NHAC NHO THANH TOAN",
                TemplateMappingHelper.GetInstance().GetTemplateRemindPayment(item, urlPayment)
                );
            }
        }
        public async Task ScheduleReminder()
        {
            DateTime _vietnamTime = Utility.GetInstance().GetCurrentDateTimeInTimeZone();
            var scheduleRepository = Resolve<IScheduleRepository>();
            var classDetailRepository = Resolve<IClassDetailRepository>();
            var emailService = Resolve<IEmailService>();
            var listScheduleToday = await scheduleRepository.
                                          GetListByExpression(s => s.Date.Date == _vietnamTime.Date, s => s.Class);
            HashSet<Class> classes = new HashSet<Class>();
            foreach (var item in listScheduleToday)
            {
                classes.Add(item.Class);
            }
            foreach (var item in classes)
            {
                var classDetail = await classDetailRepository.
                                        GetListByExpression(c => c.ClassId == item.ClassId, c => c.User);
                var listScheduleForClass = await scheduleRepository.
                                                GetListByExpression(s => s.Date.Date == _vietnamTime.Date && s.ClassId == item.ClassId);
                foreach (var item2 in classDetail)
                {
                    emailService.SendEmail(item2.User.Email, "Schedule Remind", TemplateMappingHelper.GetInstance().GetTemplateRemindSchedule(Utility.ConvertIOrderQueryAbleToList(listScheduleForClass)));
                }
            }

        }
    }
}

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

namespace YogaCenter.BackEnd.Service.Services
{
    public class WorkerService : BackgroundService
    {
        private readonly ILogger<WorkerService> _logger;
        private readonly IServiceProvider _serviceProvider;
        public WorkerService(ILogger<WorkerService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _db = scope.ServiceProvider.GetRequiredService<YogaCenterContext>();
                    TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                    DateTime vietnamTime = TimeZoneInfo.ConvertTime(DateTime.Now, vietnamTimeZone);
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    var endedClasses = await _db.Classes.Where((c => c.EndDate <= vietnamTime && c.IsDeleted == false)).ToListAsync();
                   foreach (var endedClass in endedClasses)
                    {
                        endedClass.IsDeleted = true;
                        _db.Update(endedClass);
                    }
                    _db.SaveChanges();
                }


                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}

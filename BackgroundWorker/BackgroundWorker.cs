using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Models;

namespace BackgroundWorker
{
    public class BackgroundWorker : BackgroundService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BackgroundWorker> _logger;

        public BackgroundWorker(IUnitOfWork unitOfWork, ILogger<BackgroundWorker> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                DateTime vietnamTime = TimeZoneInfo.ConvertTime(DateTime.Now, vietnamTimeZone);
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                var endedClasses = await _unitOfWork.GetRepository<Class>().GetListByExpression(c => c.EndDate >= vietnamTime && !(bool)c.IsDeleted);
                foreach ( var endedClass in endedClasses)
                {
                    endedClass.IsDeleted = true;
                    _unitOfWork.GetRepository<Class>().Update(endedClass);
                }
                _unitOfWork.SaveChange();
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.DAL.Util;
using YogaCenter.BackEnd.Service.Contracts;

namespace YogaCenter.BackEnd.Service.Implementations
{
    public class ReportService : GenericBackendService,IReportService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public ReportService(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IServiceProvider serviceProvider)
            :base(serviceProvider)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ReportDto> GetReportByMonthAndYear(int month, int year)
        {
            var courseRepository = Resolve<ICourseRepository>();
            var classRepository = Resolve<IClassRepository>();
            var subcriptionRepository = Resolve<ISubscriptionRepository>();

            ReportDto report = new ReportDto();
             report.Date = DateTime.Now;
          

            var courseList = await courseRepository.GetAll();
            var classDto = await classRepository.GetAll();

            report.TotalCourse = courseList.Count();
            report.TotalClass = classDto.Count();

           List<double> totalList = new List<double>();
            foreach ( var course in courseList )
            {
                var subcription = await subcriptionRepository.GetListByExpression(c => c.Class.CourseId == course.CourseId && c.SubscriptionDate.Value.Month == month && c.SubscriptionDate.Value.Year == year && c.SubscriptionStatusId == SD.Subscription.SUCCESSFUL, null);
                double total = 0;
                foreach (var i in subcription)
                {
                    total = (double)(total + i.Total);
                }
                report.ReportMonths.Add(
                    new ReportDto.ReportMonth()
                    {
                        Course = _mapper.Map<CourseDto>(course),
                        Classes = _mapper.Map<IEnumerable<ClassDto>>(await classRepository.GetListByExpression(c => c.CourseId == course.CourseId, null)),
                        Total = total
                    }) ;
                totalList.Add(total);

            }

            double totalReport = 0;

            foreach(var total in  totalList)
            {
                totalReport = totalReport + total;
            }

            report.Total = totalReport;
            return report;
        }
    }
}

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
    public class ReportService : IReportService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public ReportService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ReportDto> GetReportByMonthAndYear(int month, int year)
        {
            ReportDto report = new ReportDto();
           report.Date = DateTime.Now;
            var courseDto = await _unitOfWork.GetRepository<Course>().GetAll();
            var classDto = await _unitOfWork.GetRepository<Class>().GetAll();

            report.TotalCourse = courseDto.Count();
            report.TotalClass = classDto.Count();

           List<double> totalList = new List<double>();
            foreach ( var course in courseDto )
            {
                var subcription = await _unitOfWork.GetRepository<Subscription>().GetListByExpression(c => c.Class.CourseId == course.CourseId && c.SubscriptionDate.Value.Month == month && c.SubscriptionDate.Value.Year == year && c.SubscriptionStatusId == SD.Subscription.SUCCESSFUL, null);
                double total = 0;
                foreach (var i in subcription)
                {
                    total = (double)(total + i.Total);
                }
                report.ReportMonths.Add(
                    new ReportDto.ReportMonth()
                    {
                        Course = _mapper.Map<CourseDto>(course),
                        Classes = _mapper.Map<IEnumerable<ClassDto>>(await _unitOfWork.GetRepository<Class>().GetListByExpression(c => c.CourseId == course.CourseId, null)),
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

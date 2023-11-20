using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto.Response;

namespace YogaCenter.BackEnd.Service.Contracts
{
    public interface IReportService
    {
        Task<ReportDto> GetReportByMonthAndYear(int month, int year);
    }
}

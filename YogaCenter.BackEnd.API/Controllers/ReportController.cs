using DinkToPdf.Contracts;
using DinkToPdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using NPOI.POIFS.Crypt.Dsig;
using YogaCenter.BackEnd.Service.Contracts;
using YogaCenter.BackEnd.DAL.Util;
using YogaCenter.BackEnd.Common.Dto;

namespace YogaCenter.BackEnd.API.Controllers
{
    [Route("report")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private  IFileService _fileService;
        private IReportService _reportService;

        public ReportController(IFileService fileService, IReportService reportService)
        {
            _fileService = fileService;
            _reportService = reportService;
        }

        [HttpPost("get-pdf-file-report")]
        public async Task<IActionResult> ConvertHtmlToPdf(int month, int year)
        {
            try
            {
                var fileResult = await _fileService.ConvertHtmlToPdf(month, year);
                return fileResult;
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpPost("report")]
        public async Task<ReportDto> Report(int month, int year)
        {
            return await _reportService.GetReportByMonthAndYear(month, year);
            
        }

    }



}

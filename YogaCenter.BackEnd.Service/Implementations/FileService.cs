﻿using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.Service.Contracts;


namespace YogaCenter.BackEnd.Service.Implementations
{
    public class FileService : IFileService
    {

        private readonly IConverter _pdfConverter;
        private IReportService _reportService;
        public FileService(IConverter pdfConverter, IReportService reportService)
        {
            _pdfConverter = pdfConverter;
            _reportService = reportService;
        }


        public string GetTemplate(ReportDto report)
        {
            string body = $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Report</title>
    <link rel=""stylesheet"" href=""https://stackpath.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css"" integrity=""sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm"" crossorigin=""anonymous"">
</head>

<body>
    <div class=""container mt-5"">
        <div class=""row"">
            <div class=""col-md-6"">
                <h1><b>Yoga Center</b></h1>
                <p class=""p-0 m-0"">Address: Ho Chi Minh City</p>
                <p  class=""p-0 m-0"">Phone Number: 0366 967 957</p>
                <p  class=""p-0 m-0"">Email : hongquan.contact@gmail.com</p>

            </div>
            <div class=""col-md-6"">
              
            </div>
        </div>
        <h3 class=""text-center"" style=""color: red;"">Monthly Report " + report.Date.Month + "/" + +report.Date.Year + @"</h3>

            <div class=""row"">
                <div class=""col-md-6"">
                    <p>Report Date : " + report.Date + @"</p>
                    <p>Total course : " + report.TotalCourse + @"</p>
                    <p>Total class : " + report.TotalClass + @"</p>


                </div>
                <div class=""col-md-6""></div>

            </div>


        <table class=""table"">
            <thead class=""thead-dark"">
              <tr>
                <th scope=""col"">#</th>

                <th scope=""col"">Course name</th>
                <th scope=""col"">Class</th>
                <th scope=""col"">Total</th>
              </tr>
            </thead>
            <tbody>
";
            int i = 1;

            foreach (var item in report.ReportMonths)
            {
                body += @"
      <tr>
        <th scope=""row"">" + i + @"</th>
        <td>" + item.Course.CourseName + @"</td>
        <td>";

                foreach (var item2 in item.Classes)
                {
                    body += @"
            <p>" + item2.ClassName + @"</p>
            ";
                }
                body += @"
        </td>
        <td>" + item.Total.ToString("N0", new System.Globalization.CultureInfo("en-US")).Replace(",", ".") + @"</td>
      </tr>
      ";
                i++;
            };

            body += @"
            </tbody>
          </table>
          <div class=""d-flex justify-content-end"">
          <h6 class=""p-3"" style=""border: 1px solid #ccc; border-radius: 10px;"">Total: " + report.Total.ToString("N0", new System.Globalization.CultureInfo("en-US")).Replace(",", ".") +@"</h6>
          </div>
        
      
    </div>

    <script src=""https://code.jquery.com/jquery-3.2.1.slim.min.js"" integrity=""sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo"" crossorigin=""anonymous""></script>

    <script src=""https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.9/umd/popper.min.js"" integrity=""sha384-ApNbgh9B+Y1QKtv3Rn7W3mgPxhU9K/ScQsAP7hUibX39j7fakFPskvXusvfa0b4Q"" crossorigin=""anonymous""></script>

    <script src=""https://stackpath.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js"" integrity=""sha384-JZR6Spejh4U02d8jOt6vLEHfe/JQGiRRSQQxSfFWpi1MquVdAyjUar5+76PVCmYl"" crossorigin=""anonymous""></script>
</body>
</html>

"; 
            
            return body;
        }


        public async Task<IActionResult> ConvertHtmlToPdf(int month, int year)
        {
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait,
                },
                Objects = {
                    new ObjectSettings() {
                        PagesCount = true,
                    HtmlContent = GetTemplate(await _reportService.GetReportByMonthAndYear(month,year)),


                     WebSettings = { DefaultEncoding = "utf-8" },

                    }
                }
            };

            byte[] pdfBytes = _pdfConverter.Convert(doc);

            return new FileContentResult(pdfBytes, "application/pdf")
            {
                FileDownloadName = $"report_{month}/{year}.pdf"
            };
        }



        public IActionResult ConvertDataToExcel()
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells["A1"].Value = "Ticket name";
                worksheet.Cells["B1"].Value = "Ticket issue";

                byte[] excelBytes = package.GetAsByteArray();

                return new FileContentResult(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = "template.xlsx"
                };
            }
        }


    }


}

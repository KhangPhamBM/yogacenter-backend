using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.Service.Contracts
{
    public interface IFileService
    {
        public Task<IActionResult> ConvertHtmlToPdf(int month, int year);
        public IActionResult ConvertDataToExcel();


    }
}

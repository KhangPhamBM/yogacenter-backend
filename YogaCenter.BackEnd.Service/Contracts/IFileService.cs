using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto.Request;

namespace YogaCenter.BackEnd.Service.Contracts
{
    public interface IFileService
    {
        public Task<IActionResult> ConvertHtmlToPdf(int month, int year);
        public IActionResult ConvertDataToExcel();
        public ActionResult<List<List<string>>> UploadExcel(IFormFile file);

        public IActionResult GenerateExcelContent<T>(IEnumerable<T> dataList, string sheetName);
        public IActionResult GenerateTemplateExcel<T>(T dataList);


        public Task<AppActionResult> UploadImageToFirebase(IFormFile file, string pathFileName);
        public  Task<string> GetUrlImageFromFirebase(string pathFileName);

        public Task<AppActionResult> DeleteImageFromFirebase( string pathFileName);


    }
}

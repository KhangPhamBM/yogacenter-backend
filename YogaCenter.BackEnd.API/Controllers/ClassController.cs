using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.Service.Contracts;

namespace YogaCenter.BackEnd.API.Controllers
{
    [Route("class")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        public readonly IClassService _classService;
        public AppActionResult _responeDto;
        public ClassController(IClassService classService)
        {
            _classService = classService;
            _responeDto = new AppActionResult();
        }

        [HttpPost("create-class")]
        public async Task<AppActionResult> CreateClass(ClassDto classDto)
        {
            return await _classService.CreateClass(classDto);
        }
        [HttpPut("update-class")]
        public async Task<AppActionResult> UpdateClass(ClassDto classDto)
        {
           return await _classService.UpdateClass(classDto);
        }

        [HttpGet("get-class-by-id/{id:int}")]
        public async Task<AppActionResult> GetClassById(int id)
        {
            return await _classService.GetClassById(id);
        }



    }
}

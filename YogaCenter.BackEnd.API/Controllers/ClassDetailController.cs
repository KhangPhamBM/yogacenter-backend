using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Util;
using YogaCenter.BackEnd.Service.Contracts;
using YogaCenter.BackEnd.Service.Implementations;

namespace YogaCenter.BackEnd.API.Controllers
{
    [Route("class-detail")]
    [ApiController]
    public class ClassDetailController : ControllerBase
    {
        public AppActionResult _responeDto;
        public IClassDetailService _classDetail;

        public ClassDetailController(IClassDetailService classDetail)
        {
            _classDetail = classDetail;
            _responeDto = new AppActionResult();
        }
        [HttpPost("register-class")]
        public async Task<AppActionResult> RegisterClass(ClassDetailDto classDto)
        {
           return await _classDetail.RegisterClass(classDto); 
        }

        [HttpPost("get-class-details-by-classId/{classId:int}")]
        [Authorize(Roles = Permission.CLASS)]

        public async Task<AppActionResult> GetClassDetailsByClassId(int classId, int pageIndex, int pageSize, IList<SortInfo> sortInfos)
        {
            return await _classDetail.GetClassDetailsByClassId(classId, pageIndex, pageSize, sortInfos);
        }
    }
}

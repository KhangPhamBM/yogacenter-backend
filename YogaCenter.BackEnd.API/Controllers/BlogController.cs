using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YogaCenter.BackEnd.Common.Dto.Request;
using YogaCenter.BackEnd.Common.Dto.Response;
using YogaCenter.BackEnd.DAL.Util;
using YogaCenter.BackEnd.Service.Contracts;
using YogaCenter.BackEnd.Service.Implementations;

namespace YogaCenter.BackEnd.API.Controllers
{
    [Route("blog")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        public readonly IBlogService _blogService;
        private IFileService _fileService;
        public BlogController(IBlogService blogService, IFileService fileService)
        {
            _blogService = blogService;
            _fileService = fileService;
        }

        [HttpPost("create-blog")]
        [Authorize(Roles = Permission.MANAGEMENT)]

        public async Task<AppActionResult> CreateBlog([FromForm]BlogRequestDto Blog)
        {
            return await _blogService.CreateBlog(Blog);
        }

        [HttpPut("update-blog")]
        [Authorize(Roles = Permission.MANAGEMENT)]

        public async Task<AppActionResult> UpdateBlog([FromForm] BlogRequestDto Blog)
        {
            return await _blogService.UpdateBlog(Blog);
        }
        [HttpGet("get-blog-by-id/{id:int}")]

        public async Task<AppActionResult> GetBlogById(int id)
        {
            return await _blogService.GetBlogById(id);
        }

        [HttpPost]
        [Route("get-all-blog")]
        public async Task<AppActionResult> GetAllBlog(int pageIndex, int pageSize, IList<SortInfo> sortInfos)
        {
            return await _blogService.GetAll(pageIndex, pageSize, sortInfos);
        }

        [HttpDelete("delete-blog/{id:int}")]
        [Authorize(Roles = Permission.ADMIN)]

        public async Task<AppActionResult> DeleteBlog(int id)
        {
            return await _blogService.DeleteBlog(id);
        }

        [HttpPost("get-blog-with-searching")]
        public async Task<AppActionResult> GetBlogWithSearching(BaseFilterRequest baseFilterRequest)
        {
            return await _blogService.SearchApplyingSortingAndFiltering(baseFilterRequest);
        }
        [HttpPost("export-template")]
        public IActionResult ExportTemplate()
        {
            return _fileService.GenerateTemplateExcel(new BlogResponseDto());
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Util;
using YogaCenter.BackEnd.Service.Contracts;

namespace YogaCenter.BackEnd.API.Controllers
{
    [Route("blog")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        public readonly IBlogService _BlogService;
    public BlogController(IBlogService BlogService)
    {
        _BlogService = BlogService;
    }

    [HttpPost("create-blog")]
    [Authorize(Roles = Permission.MANAGEMENT)]

    public async Task<AppActionResult> CreateBlog(BlogDto Blog)
    {
        return await _BlogService.CreateBlog(Blog);
    }

    [HttpPut("update-blog")]
    [Authorize(Roles = Permission.MANAGEMENT)]

    public async Task<AppActionResult> UpdateBlog(BlogDto Blog)
    {
        return await _BlogService.UpdateBlog(Blog);
    }
    [HttpGet("get-blog-by-id/{id:int}")]

    public async Task<AppActionResult> GetBlogById(int id)
    {
        return await _BlogService.GetBlogById(id);
    }

    [HttpPost]
    [Route("get-all-blog")]
    public async Task<AppActionResult> GetAllBlog(int pageIndex, int pageSize, IList<SortInfo> sortInfos)
    {
        return await _BlogService.GetAll(pageIndex, pageSize, sortInfos);
    }

    [HttpDelete("delete-blog/{id:int}")]
    [Authorize(Roles = Permission.ADMIN)]

    public async Task<AppActionResult> DeleteBlog(int id)
    {
        return await _BlogService.DeleteBlog(id);
    }

    [HttpPost("get-blog-with-searching")]
    public async Task<AppActionResult> GetBlogWithSearching(BaseFilterRequest baseFilterRequest)
    {
        return await _BlogService.SearchApplyingSortingAndFiltering(baseFilterRequest);
    }
}
}

using AutoMapper;
using NPOI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto.Request;
using YogaCenter.BackEnd.Common.Dto.Response;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.DAL.Util;
using YogaCenter.BackEnd.Service.Contracts;

namespace YogaCenter.BackEnd.Service.Implementations
{
    public class BlogService : GenericBackendService, IBlogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;
        private readonly AppActionResult _result;
        public BlogService(IUnitOfWork unitOfWork, IBlogRepository blogRepository, IMapper mapper, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _unitOfWork = unitOfWork;
            _blogRepository = blogRepository;
            _mapper = mapper;
            _result = new();
        }
        public async Task<AppActionResult> CreateBlog(BlogRequestDto blog)
        {
            try
            {
                bool isValid = true;
                var accountRepository = Resolve<IAccountRepository>();
                var fileService = Resolve<IFileService>();

                if (await accountRepository.GetById(blog.UserId) == null)
                {
                    isValid = false;
                    _result.Message.Add($"User with id {blog.UserId} not found");
                }

                if (await _blogRepository.GetByExpression(b => b.Title.Equals(blog.Title)) != null)
                {
                    isValid = false;
                    _result.Message.Add($"Duplicated Title");
                }

                if (isValid)
                {
                    var blogDB = _mapper.Map<Blog>(blog);
                    await _blogRepository.Insert(blogDB);
                    await _unitOfWork.SaveChangeAsync();
                    var pathName = SD.FirebasePathName.BLOG_PREFIX + blogDB.Id;
                    var upload = await fileService.UploadImageToFirebase(blog.BlogImgFile, pathName);
                    if (upload.isSuccess)
                    {
                        blogDB.BlogImg = pathName;
                    }
                    _result.Message.Add(SD.ResponseMessage.CREATE_SUCCESSFUL);
                }
                else
                {
                    _result.isSuccess = false;
                }

            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            finally
            {
                await _unitOfWork.SaveChangeAsync();
            }
            return _result;
        }

        public async Task<AppActionResult> DeleteBlog(int id)
        {
            try
            {
                bool isValid = true;
                var fileService = Resolve<IFileService>();
                var blogDB = await _blogRepository.GetById(id);
                if (blogDB == null)
                {
                    isValid = false;
                    _result.Message.Add($"Duplicated Title");
                }

                if (isValid)
                {
                    var deleteFirebase = await fileService.DeleteImageFromFirebase(blogDB.BlogImg);
                    if (deleteFirebase.isSuccess)
                    {
                        await _blogRepository.DeleteById(id);
                        await _unitOfWork.SaveChangeAsync();
                        _result.Message.Add(SD.ResponseMessage.DELETE_SUCCESSFUL);
                    }

                }
                else
                {
                    _result.isSuccess = false;
                }

            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> GetAll(int pageIndex, int pageSize, IList<SortInfo> sortInfos)
        {
            try
            {
                var fileService = Resolve<IFileService>();
                var blogs = await _blogRepository.GetAll();

               var blogList = Utility.ConvertIOrderQueryAbleToList(blogs);
                foreach( var blog in blogList)
                {
                    blog.BlogImg = await fileService.GetUrlImageFromFirebase(blog.BlogImg);
                }
                blogs = Utility.ConvertListToIOrderedQueryable(blogList);
                if (pageIndex <= 0) pageIndex = 1;
                if (pageSize <= 0) pageSize = SD.MAX_RECORD_PER_PAGE;
                int totalPage = DataPresentationHelper.CalculateTotalPageSize(blogs.Count(), pageSize);
                if (sortInfos != null)
                {
                    blogs = DataPresentationHelper.ApplySorting(blogs, sortInfos);
                }

                if (pageIndex > 0 && pageSize > 0)
                {
                    blogs = DataPresentationHelper.ApplyPaging(blogs, pageIndex, pageSize);
                }

                _result.Result.Data = blogs;
                _result.Result.TotalPage = totalPage;
            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> GetBlogById(int id)
        {
            try
            {
                var fileService = Resolve<IFileService>();

                bool isValid = true;
                var blog = await _blogRepository.GetById(id);
                if (blog == null)
                {
                    _result.Message.Add($"The Blog with id {id} not found");
                    isValid = false;

                }
                if (isValid)
                {
                    blog.BlogImg = await fileService.GetUrlImageFromFirebase(blog.BlogImg);
                    _result.Result.Data = blog;
                }
                else
                {
                    _result.isSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> SearchApplyingSortingAndFiltering(BaseFilterRequest filterRequest)
        {
            try
            {
                var source = await _blogRepository.GetAll();
                int pageIndex = filterRequest.pageIndex;
                if (pageIndex <= 0) pageIndex = 1;
                int pageSize = filterRequest.pageSize;
                if (pageSize <= 0) pageSize = SD.MAX_RECORD_PER_PAGE;
                int totalPage = DataPresentationHelper.CalculateTotalPageSize(source.Count(), pageSize);
                if (filterRequest != null)
                {
                    if (filterRequest.pageIndex <= 0 || filterRequest.pageSize <= 0)
                    {
                        _result.Message.Add($"Invalid value of pageIndex or pageSize");
                        _result.isSuccess = false;
                    }
                    else
                    {
                        if (filterRequest.keyword != "" && filterRequest.keyword != null)
                        {
                            source = await _blogRepository.GetListByExpression(b => b.Title.Contains(filterRequest.keyword), null);
                        }
                        if (filterRequest.filterInfoList != null)
                        {
                            source = DataPresentationHelper.ApplyFiltering(source, filterRequest.filterInfoList);
                        }
                        totalPage = DataPresentationHelper.CalculateTotalPageSize(source.Count(), filterRequest.pageSize);

                        if (filterRequest.sortInfoList != null)
                        {
                            source = DataPresentationHelper.ApplySorting(source, filterRequest.sortInfoList);
                        }
                        source = DataPresentationHelper.ApplyPaging(source, filterRequest.pageIndex, filterRequest.pageSize);
                        _result.Result.Data = source;

                    }
                }
                else
                {
                    _result.Result.Data = source;
                }
                _result.Result.TotalPage = totalPage;

            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> UpdateBlog(BlogRequestDto blog)
        {
            try
            {
                bool isValid = true;
                var fileService = Resolve<IFileService>();
                var blogDB = await _blogRepository.GetById(blog.Id);
                if (blogDB == null)
                {
                    _result.Message.Add($"The blog with id {blog.Id} not found");
                    isValid = false;

                }

                if (isValid)
                {
                    await fileService.DeleteImageFromFirebase(blogDB.BlogImg);
                    await fileService.UploadImageToFirebase(blog.BlogImgFile, SD.FirebasePathName.BLOG_PREFIX + blogDB.Id);
                    _mapper.Map(blog, blogDB);
                    await _blogRepository.Update(blogDB);
                    await _unitOfWork.SaveChangeAsync();
                    _result.Message.Add(SD.ResponseMessage.UPDATE_SUCCESSFUL);
                }
                else
                {
                    _result.isSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }
    }
}

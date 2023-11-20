using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.Common.Dto.Request
{
    public class BlogRequestDto
    {
        public int? Id { get; set; }
        public string? UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public IFormFile BlogImgFile { get; set; }
    }
}

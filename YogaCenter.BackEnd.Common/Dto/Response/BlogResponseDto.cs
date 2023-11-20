using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.Common.Dto.Response
{
    public class BlogResponseDto
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string BlogImg { get; set; }
    }
}

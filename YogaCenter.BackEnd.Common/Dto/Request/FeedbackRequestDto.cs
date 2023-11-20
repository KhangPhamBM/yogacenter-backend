using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.Common.Dto.Request
{
    public class FeedbackRequestDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ClassId { get; set; }

        public string Content { get; set; }
        public RatingStar Rating { get; set; }
        public FeedbackStatus Status { get; set; }

        public enum RatingStar
        {
            OneStar = 1, TwoStar = 2, ThreeStar = 3, FourStar = 4, FiveStar = 5
        }

        public enum FeedbackStatus
        {
            Pending = 1, Approved = 2, Rejected = 3
        }

    }
}

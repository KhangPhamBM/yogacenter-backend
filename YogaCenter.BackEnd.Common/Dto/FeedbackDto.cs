using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.Common.Dto
{
    public class FeedbackDto
    {
        public int Id { get; set; }
        public int ClassDetailId { get; set; }
        public string Content { get; set; }
        public RatingStar Rating { get; set; }
        public FeedbackRating Status { get; set; }

        public enum RatingStar
        {
            OneStar = 1, TwoStar = 2, ThreeStar = 3, FourStar = 4, FiveStar = 5
        }

        public enum FeedbackRating
        {
            Pending = 1, Approved = 2, Rejected = 3
        }

    }
}

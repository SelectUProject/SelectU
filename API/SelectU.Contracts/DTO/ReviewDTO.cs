using SelectU.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelectU.Contracts.DTO
{
    public class ReviewDTO
    {
        public Guid Id { get; set; }
        public string ReviewerId { get; set; }
        public Guid ScholarshipApplicationId { get; set; }
        public byte? Rating { get; set; }
        public string? Comment { get; set; }
        public ReviewDTO() { }

        public ReviewDTO(Review review)
        {
            Id = review.Id;
            ReviewerId = review.ReviewerId;
            Rating = review.Rating;
            Comment = review.Comment;
        }
    }
}

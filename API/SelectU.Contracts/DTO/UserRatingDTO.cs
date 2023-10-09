using SelectU.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelectU.Contracts.DTO
{
    public class UserRatingDTO
    {
        public Guid Id { get; set; }
        public string ApplicantId { get; set; }
        public string ReviewerId { get; set; }
        public Guid ScholarshipApplicationId { get; set; }
        public int? Rating { get; set; }
        public string? Comment { get; set; }
        public UserRatingDTO() { }

        public UserRatingDTO(UserRating userRating)
        {
            ApplicantId = userRating.ApplicantId;
            Id = userRating.Id;
            ReviewerId = userRating.ReviewerId;
            Rating = userRating.Rating;
            Comment = userRating.Comment;
        }
        public List<UserRatingDTO> UserRatingsToUserRatingDTOs(ICollection<UserRating>? ratings)
        {
            if (ratings == null) return new List<UserRatingDTO>();
            List<UserRatingDTO> list = new List<UserRatingDTO>();
            ratings.ToList().ForEach(x => list.Add(new UserRatingDTO(x)));
            return list;
        }
    }
}

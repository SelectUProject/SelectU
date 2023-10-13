using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelectU.Contracts.Services
{
    public interface IReviewService
    {
        Task CreateReviewAsync(ReviewDTO reviewDTO);
        Task UpdateReviewAsync(ReviewDTO reviewDTO, bool isAdmin);
        Task DeleteReviewAsync(Guid reviewId, string creatorId, bool isAdmin);
        Task<double> GetAverageRating(Guid applicationId);
        Task<List<ReviewDTO>> GetApplicationReviews(Guid applicationId, string userId, bool mineOnly);
        Task<Review?> GetReview(Guid reviewId);
    }
}

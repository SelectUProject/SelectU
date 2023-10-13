using SelectU.Contracts.DTO;
using SelectU.Contracts;
using SelectU.Contracts.Services;
using SelectU.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelectU.Contracts.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace SelectU.Core.Services
{
    public class ReviewService: IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReviewService(IUnitOfWork context)
        {
            _unitOfWork = context;
        }


        public async Task CreateReviewAsync(ReviewDTO reviewDTO)
        {
            var scholarshipApplication = await _unitOfWork.ScholarshipApplications.Where(x => x.Id == reviewDTO.ScholarshipApplicationId).Include(x => x.Reviews).FirstOrDefaultAsync() ?? throw new ReviewException($"Unable able to add rating as the application does not exist");

            if (scholarshipApplication.Reviews != null && scholarshipApplication.Reviews.Any(x => x.ReviewerId == reviewDTO.ReviewerId)) throw new ReviewException($"Unable able to create rating as the reviewer has an existing review");

            Review review = new()
            {
                ScholarshipApplicationId = reviewDTO.ScholarshipApplicationId,
                Rating = reviewDTO.Rating,
                ReviewerId = reviewDTO.ReviewerId,
                Comment = reviewDTO.Comment
            };

            _unitOfWork.Reviews.Add(review);

            await _unitOfWork.CommitAsync();

        }

        public async Task UpdateReviewAsync(ReviewDTO reviewDTO, bool isAdmin)
        {

            var review = await _unitOfWork.Reviews.GetAsync(reviewDTO.Id) ?? throw new ReviewException($"Unable able to update review as it does not exist");
            
            if (review.ReviewerId != reviewDTO.ReviewerId && !isAdmin) throw new ReviewException($"Unable able to update rating as you need to be the owner of the review");
            
            review.Rating = reviewDTO.Rating;
            review.Comment = reviewDTO.Comment;

            _unitOfWork.Reviews.Update(review);

            await _unitOfWork.CommitAsync();


        }

        public async Task DeleteReviewAsync(Guid reviewId, string creatorId, bool isAdmin)
        {
            var review = await _unitOfWork.Reviews.GetAsync(reviewId) ?? throw new ReviewException($"Unable able to delete rating as the application does not exist");
            
            if (review.ReviewerId != creatorId && !isAdmin) throw new ReviewException($"Unable able to delete rating as you must be the owner of the review");

            await _unitOfWork.Reviews.DeleteAsync(review);
        }

        public async Task<double> GetAverageRating(Guid applicationId)
        {
            var reviews = _unitOfWork.Reviews.Where(x => x.ScholarshipApplicationId == applicationId).AsQueryable();

            if (!await reviews.AnyAsync()) return 0;

            return await reviews.AverageAsync(x => x.Rating);
        }

        public async Task<List<ReviewDTO>> GetApplicationReviews(Guid applicationId, string userId, bool mineOnly)
        {
            var reviews = _unitOfWork.Reviews.Where(x => x.ScholarshipApplicationId == applicationId).AsQueryable();

            if(mineOnly) reviews = reviews.Where(x => x.ReviewerId == userId);

            return await reviews.Select(x => new ReviewDTO(x)).ToListAsync();
        }

        public async Task<Review?> GetReview(Guid reviewId)
        {
            return await _unitOfWork.Reviews.Where(x => x.Id == reviewId).FirstOrDefaultAsync();
        }
    }
}

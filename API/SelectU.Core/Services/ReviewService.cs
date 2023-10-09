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
            var scholarshipApplication = await _unitOfWork.ScholarshipApplications.GetAsync(reviewDTO.ScholarshipApplicationId);

            if (scholarshipApplication == null) throw new ReviewException($"Unable able to add rating as the application does not exist");

            if (scholarshipApplication.Reviews.FirstOrDefault(x => x.ReviewerId == reviewDTO.ReviewerId) == null) throw new ReviewException($"Unable able to create rating as the reviewer has an existing review");

            Review review = new Review()
            {
                ScholarshipApplicationId = reviewDTO.ScholarshipApplicationId,
                Rating = reviewDTO.Rating,
                ReviewerId = reviewDTO.ReviewerId,
                Comment = reviewDTO.Comment
            };

            scholarshipApplication.Reviews.Add(review);
            _unitOfWork.ScholarshipApplications.Update(scholarshipApplication);

            await _unitOfWork.CommitAsync();

        }

        public async Task UpdateReviewAsync(ReviewDTO reviewDTO, bool isAdmin)
        {

            var review = await _unitOfWork.Review.GetAsync(reviewDTO.Id);
            if (review == null)
            {
                throw new ReviewException($"Unable able to update review as it does not exist");
            }
            if (review.ReviewerId != reviewDTO.ReviewerId && !isAdmin) throw new ReviewException($"Unable able to update rating as you need to be the owner of the review");
            review.ScholarshipApplicationId = reviewDTO.ScholarshipApplicationId;
            review.Rating = reviewDTO.Rating;
            review.Comment = reviewDTO.Comment;

            _unitOfWork.Review.Update(review);

            await _unitOfWork.CommitAsync();


        }

        public async Task DeleteReviewAsync(Guid reviewId, string creatorId, bool isAdmin)
        {
            var review = await _unitOfWork.Review.GetAsync(reviewId);

            if (review == null)
            {
                throw new ReviewException($"Unable able to delete rating as the application does not exist");
            }
            if(review.ReviewerId != creatorId && !isAdmin) throw new ReviewException($"Unable able to delete rating as you must be the owner of the review");

            await _unitOfWork.Review.DeleteAsync(review);
        }
    }
}

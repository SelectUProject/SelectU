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


        public async Task CreateScholarshipApplicationRatingAsync(UserRatingDTO userRatingDTO)
        {
            var scholarshipApplication = await _unitOfWork.ScholarshipApplications.GetAsync(userRatingDTO.ScholarshipApplicationId);

            if (scholarshipApplication == null) throw new ReviewException($"Unable able to add rating as the application does not exist");

            if (scholarshipApplication.Ratings.FirstOrDefault(x => x.ReviewerId == userRatingDTO.ReviewerId) == null) throw new ReviewException($"Unable able to create rating as the reviewer has an existing review");

            UserRating userRating = new UserRating()
            {
                ScholarshipApplicationId = userRatingDTO.ScholarshipApplicationId,
                Rating = userRatingDTO.Rating,
                Id = userRatingDTO.Id,
                ReviewerId = userRatingDTO.ReviewerId,
                ApplicantId = userRatingDTO.ApplicantId,
                Comment = userRatingDTO.Comment
            };

            scholarshipApplication.Ratings.Add(userRating);
            _unitOfWork.ScholarshipApplications.Update(scholarshipApplication);

            await _unitOfWork.CommitAsync();

        }

        public async Task UpdateScholarshipApplicationRatingAsync(UserRatingDTO userRatingDTO, bool isAdmin)
        {
            var scholarshipApplication = await _unitOfWork.ScholarshipApplications.GetAsync(userRatingDTO.ScholarshipApplicationId);

            if (scholarshipApplication == null)
            {
                throw new ReviewException($"Unable able to add rating as the application does not exist");
            }

            UserRating userRating = new UserRating()
            {
                ScholarshipApplicationId = userRatingDTO.ScholarshipApplicationId,
                Rating = userRatingDTO.Rating,
                Id = userRatingDTO.Id,
                ReviewerId = userRatingDTO.ReviewerId,
                ApplicantId = userRatingDTO.ApplicantId,
                Comment = userRatingDTO.Comment
            };
            var originalRating = scholarshipApplication.Ratings.FirstOrDefault(x => x.Id == userRating.Id && (x.ReviewerId == userRatingDTO.ReviewerId || isAdmin));
            if (originalRating == null) throw new ReviewException($"Unable able to update rating as you need to be the owner of the review");
            scholarshipApplication.Ratings.Remove(originalRating);
            scholarshipApplication.Ratings.Add(userRating);
            _unitOfWork.ScholarshipApplications.Update(scholarshipApplication);

            await _unitOfWork.CommitAsync();


        }

        public async Task DeleteScholarshipApplicationRatingAsync(Guid RatingId, string CreatorId, bool isAdmin)
        {
            var userRating = await _unitOfWork.UserRating.GetAsync(RatingId.ToString());

            if (userRating == null)
            {
                throw new ReviewException($"Unable able to delete rating as the application does not exist");
            }
            if(userRating.ReviewerId != CreatorId && !isAdmin) throw new ReviewException($"Unable able to delete rating as you must be the owner of the review");

            await _unitOfWork.UserRating.DeleteAsync(userRating);
        }
    }
}

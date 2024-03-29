﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelectU.Contracts.Constants;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Services;
using SelectU.Core.Exceptions;
using SelectU.Core.Extensions;
using System.Data;

namespace SelectU.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReviewController : Controller
    {
        private readonly ILogger<ReviewController> _logger;
        private readonly IReviewService _reviewService;

        public ReviewController(ILogger<ReviewController> logger,
            IReviewService reviewService)
        {
            _logger = logger;
            _reviewService = reviewService;
        }

        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.Reviewer}, {UserRoles.Admin}")]
        [HttpPost("")]
        public async Task<IActionResult> CreateReviewAsync(ReviewDTO reviewDTO)
        {
            try
            {
                reviewDTO.ReviewerId = HttpContext.GetUserId();

                await _reviewService.CreateReviewAsync(
                    reviewDTO);

                return Ok();
            }
            catch (ReviewException ex)
            {
                return BadRequest(new ResponseDTO { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Application Review, {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.Reviewer}, {UserRoles.Admin}")]
        [HttpPatch("")]
        public async Task<IActionResult> UpdateReviewAsync(ReviewDTO reviewDTO)
        {
            try
            {
                reviewDTO.ReviewerId = HttpContext.GetUserId();

                await _reviewService.UpdateReviewAsync(
                    reviewDTO);

                return Ok();
            }
            catch (ReviewException ex)
            {
                return BadRequest(new ResponseDTO { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Application Review, {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.Reviewer}, {UserRoles.Admin}")]
        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> DeleteReviewAsync([FromRoute] Guid reviewId)
        {
            try
            {
                await _reviewService.DeleteReviewAsync(
                    reviewId,
                    HttpContext.GetUserId(),
                    HttpContext.IsAdmin()
                    );

                return Ok();
            }
            catch (ReviewException ex)
            {
                return BadRequest(new ResponseDTO { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Application Review, {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.Reviewer}, {UserRoles.Admin}")]
        [HttpGet("average-rating/{applicationId}")]
        public async Task<IActionResult> GetAverageRating([FromRoute] Guid applicationId)
        {
            try
            {
                var avgRating = await _reviewService.GetAverageRating(applicationId);

                return Ok(avgRating);
            }
            catch (ReviewException ex)
            {
                return BadRequest(new ResponseDTO { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Application Review, {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.Reviewer}, {UserRoles.Admin}")]
        [HttpGet("application/{applicationId}")]
        public async Task<IActionResult> GetApplicationReview([FromRoute] Guid applicationId)
        {
            try
            {
                var reviews = await _reviewService.GetApplicationReviews(applicationId, HttpContext.GetUserId(), false);

                return Ok(reviews);
            }
            catch (ReviewException ex)
            {
                return BadRequest(new ResponseDTO { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Application Review, {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.Reviewer}, {UserRoles.Admin}")]
        [HttpGet("application/{applicationId}/mine")]
        public async Task<IActionResult> GetMyReview([FromRoute] Guid applicationId)
        {
            try
            {
                var reviews = await _reviewService.GetApplicationReviews(applicationId, HttpContext.GetUserId(), true);

                if (!reviews.Any()) return Ok();

                return Ok(reviews.First());
            }
            catch (ReviewException ex)
            {
                return BadRequest(new ResponseDTO { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Application Review, {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.Reviewer}, {UserRoles.Admin}")]
        [HttpGet("{reviewId}")]
        public async Task<IActionResult> GetReview([FromRoute] Guid reviewId)
        {
            try
            {
                var review = await _reviewService.GetReview(reviewId);

                return Ok(review);
            }
            catch (ReviewException ex)
            {
                return BadRequest(new ResponseDTO { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Application Review, {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}

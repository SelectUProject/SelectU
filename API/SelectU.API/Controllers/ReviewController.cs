using Microsoft.AspNetCore.Authorization;
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
                await _reviewService.UpdateReviewAsync(
                    reviewDTO, HttpContext.IsAdmin());

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
    }
}

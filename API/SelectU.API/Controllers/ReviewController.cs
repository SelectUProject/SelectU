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
        private readonly ILogger<ScholarshipApplicationController> _logger;
        private readonly IReviewService _reviewService;

        public ReviewController(ILogger<ScholarshipApplicationController> logger,
            IReviewService reviewService)
        {
            _logger = logger;
            _reviewService = reviewService;
        }

        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.Reviewer}, {UserRoles.Admin}")]
        [HttpPost("")]
        public async Task<IActionResult> CreateReviewAsync(UserRatingDTO userRatingDTO)
        {
            try
            {
                await _reviewService.CreateScholarshipApplicationRatingAsync(
                    userRatingDTO);

                return Ok();
            }
            catch (ScholarshipApplicationException ex)
            {
                return BadRequest(new ResponseDTO { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Scholarship Application Rating, {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.Reviewer}, {UserRoles.Admin}")]
        [HttpPatch("")]
        public async Task<IActionResult> UpdateReviewAsync(UserRatingDTO userRatingDTO)
        {
            try
            {
                await _reviewService.UpdateScholarshipApplicationRatingAsync(
                    userRatingDTO, HttpContext.IsAdmin());

                return Ok();
            }
            catch (ScholarshipApplicationException ex)
            {
                return BadRequest(new ResponseDTO { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Scholarship Application Rating, {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.Reviewer}, {UserRoles.Admin}")]
        [HttpDelete("")]
        public async Task<IActionResult> DeleteReviewAsync([FromQuery] Guid ratingId)
        {
            try
            {
                await _reviewService.DeleteScholarshipApplicationRatingAsync(
                    ratingId,
                    HttpContext.GetUserId(),
                    HttpContext.IsAdmin()
                    );

                return Ok();
            }
            catch (ScholarshipApplicationException ex)
            {
                return BadRequest(new ResponseDTO { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Scholarship Application Rating, {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}

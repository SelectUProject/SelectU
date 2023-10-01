using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelectU.Contracts.Constants;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Enums;
using SelectU.Contracts.Services;
using SelectU.Core.Exceptions;
using SelectU.Core.Extensions;
using SelectU.Core.Helpers;
using System.Data;

namespace SelectU.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScholarshipController : ControllerBase
    {
        private readonly ILogger<ScholarshipController> _logger;
        private readonly IScholarshipService _scholarshipService;

        public ScholarshipController(ILogger<ScholarshipController> logger,
            IScholarshipService scholarshipService)
        {
            _logger = logger;
            _scholarshipService = scholarshipService;
        }

        [Authorize]
        [HttpGet("details")]
        public async Task<IActionResult> GetScholarshipDetailsAsync(Guid id)
        {
            
            try
            {
                var scholarship = await _scholarshipService.GetScholarshipAsync(id);

                if (scholarship == null)
                {
                    return BadRequest("Scholarship not found");
                }

                return Ok(scholarship);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Scholarship {id}, {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("active")]
        public async Task<IActionResult> GetActiveScholarshipsAsync([FromBody] ScholarshipSearchDTO scholarshipSearchDTO)
        {
            try
            {
                var scholarships = await _scholarshipService.GetActiveScholarshipAsync(scholarshipSearchDTO);

                if (scholarships == null)
                {
                    return BadRequest("Scholarship not found");
                }

                return Ok(scholarships);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Scholarship, {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = UserRoles.Staff)]
        [HttpPost("list/creator")]
        public async Task<IActionResult> GetMyCreatedScholarshipsAsync([FromBody] ScholarshipSearchDTO scholarshipSearchDTO )
        {
            try
            {
                string userId = HttpContext.GetUserId();
                var scholarship = await _scholarshipService.GetMyCreatedScholarshipsAsync(scholarshipSearchDTO, userId);

                if (scholarship == null)
                {
                    return BadRequest("Scholarship not found");
                }

                return Ok(scholarship);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Scholarship, {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = UserRoles.Staff)]
        [HttpPost("create")]
        public async Task<IActionResult> CreateScholarshipAsync([FromBody] ScholarshipCreateDTO scholarshipCreateDTO)
        {
            try
            {
                string userId = HttpContext.GetUserId();
                var scholarship = await _scholarshipService.CreateScholarshipAsync(scholarshipCreateDTO, userId);

                if (scholarship == null)
                {
                    return BadRequest("Scholarship not found");
                }

                return Ok(scholarship);
            }
            catch (ScholarshipException ex)
            {
                return BadRequest(new ResponseDTO { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Scholarship, {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = UserRoles.Staff)]
        [HttpPost("update")]
        public async Task<IActionResult> UpdateScholarshipAsync([FromBody] ScholarshipUpdateDTO scholarshipUpdateDTO)
        {
            try
            {
                var response = await _scholarshipService.UpdateScholarshipsAsync(scholarshipUpdateDTO);

                return Ok(response);
            }
            catch (ScholarshipException ex)
            {
                return BadRequest(new ResponseDTO { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Scholarship, {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = UserRoles.Staff)]
        [HttpDelete("delete/{scholarshipId}")]
        public async Task<IActionResult> DeleteScholarshipAsync([FromRoute] Guid scholarshipId)
        {
            try
            {
                string userId = HttpContext.GetUserId();
                var scholarship = await _scholarshipService.GetScholarshipAsync(scholarshipId);
                if ((scholarship.ScholarshipCreatorId != userId) || HttpContext.User.IsInRole(UserRoles.Admin))
                {
                    var response = await _scholarshipService.DeleteScholarshipsAsync(scholarshipId);

                    return Ok(response);
                }
                return BadRequest($"You may only delete your own created scholarships unless you are a {UserRoles.Admin}");
            }
            catch (ScholarshipException ex)
            {
                return BadRequest(new ResponseDTO { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Scholarship, {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}

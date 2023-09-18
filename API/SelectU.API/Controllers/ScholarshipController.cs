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
        [HttpPost("active-scholarships")]
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

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("created-scholarships")]
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

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("create-scholarship")]
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
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Scholarship, {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}

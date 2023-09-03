using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelectU.Contracts.Constants;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Enums;
using SelectU.Contracts.Extensions;
using SelectU.Contracts.Services;
using SelectU.Core.Exceptions;
using SelectU.Core.Extensions;
using SelectU.Core.Helpers;

namespace SelectU.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScholarshipApplicationController : ControllerBase
    {
        private readonly ILogger<ScholarshipApplicationController> _logger;
        private readonly IScholarshipApplicationService _scholarshipApplicationService;

        public ScholarshipApplicationController(ILogger<ScholarshipApplicationController> logger,
            IScholarshipApplicationService scholarshipApplicationService)
        {
            _logger = logger;
            _scholarshipApplicationService = scholarshipApplicationService;
        }

        [Authorize]
        [HttpGet("details")]
        public async Task<IActionResult> GetScholarshipDetailsAsync(Guid id)
        {
            
            try
            {
                var scholarship = await _scholarshipApplicationService.GetScholarshipApplicationAsync(id);

                if (scholarship == null)
                {
                    return BadRequest("Scholarship application not found");
                }

                return Ok(scholarship);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Scholarship application {id}, {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = $"{UserRoles.Admin}, {UserRoles.User}")]
        [HttpPost("active-scholarship-applications")]
        public async Task<IActionResult> GetActiveScholarshipsAsync([FromBody] ScholarshipApplicationSearchDTO scholarshipApplicationSearchDTO)
        {

            try
            {
                var scholarships = await _scholarshipApplicationService.GetActiveScholarshipApplicationsAsync(
                    scholarshipApplicationSearchDTO, 
                    HttpContext.GetUserId(), 
                    UserRoleEnum.Admin == User.GetLoggedInUserRole()
                    );

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
    }
}

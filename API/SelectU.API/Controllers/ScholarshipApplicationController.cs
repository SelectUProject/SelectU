using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelectU.Contracts.Constants;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;
using SelectU.Contracts.Enums;
using SelectU.Contracts.Extensions;
using SelectU.Contracts.Services;
using SelectU.Core.Exceptions;
using SelectU.Core.Extensions;
using SelectU.Core.Helpers;
using System.Linq;

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
        public async Task<IActionResult> GetScholarshipApplicationDetailsAsync(Guid id)
        {
            
            try
            {
                var scholarship = await _scholarshipApplicationService.GetScholarshipApplicationAsync(id);

                if (scholarship == null)
                {
                    return BadRequest("Scholarship Application not found");
                }

                return Ok(scholarship);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Scholarship Application {id}, {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.User}, {UserRoles.Admin}")]
        [HttpPost("my-scholarship-applications")]
        public async Task<IActionResult> GetMyScholarshipApplicationsAsync([FromBody] ScholarshipApplicationSearchDTO scholarshipApplicationSearchDTO)
        {

            try
            {
                var scholarships = await _scholarshipApplicationService.GetMyScholarshipApplicationsAsync(
                    scholarshipApplicationSearchDTO, 
                    HttpContext.GetUserId(), 
                    UserRoleEnum.Staff == User.GetLoggedInUserRole()
                    );

                if (scholarships == null)
                {
                    return BadRequest("Scholarship Application not found");
                }

                return Ok(scholarships);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Scholarship Application, {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = $"{UserRoles.User}, {UserRoles.Admin}")]
        [HttpPost("create-scholarship-application")]
        public async Task<IActionResult> CreateScholarshipApplicationAsync(ScholarshipApplicationCreateDTO scholarshipApplicationCreateDTO)
        {
            try
            {
                var response = await _scholarshipApplicationService.CreateScholarshipApplicationAsync(
                    scholarshipApplicationCreateDTO,
                    HttpContext.GetUserId()
                    );

                if (response == null)
                {
                    return BadRequest("Scholarship Application failed to create");
                }

                return Ok(response);
            }
            catch(ScholarshipApplicationException ex)
            {
                return BadRequest(new ResponseDTO { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Scholarship Application, {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

    }
}

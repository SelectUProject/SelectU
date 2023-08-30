using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Enums;
using SelectU.Contracts.Services;
using SelectU.Core.Exceptions;
using SelectU.Core.Extensions;
using SelectU.Core.Helpers;

namespace SelectU.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScholarshipController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IScholarshipService _scholarshipService;

        public ScholarshipController(ILogger<UserController> logger,
            IScholarshipService scholarshipService)
        {
            _logger = logger;
            _scholarshipService = scholarshipService;
        }

        //[Authorize]
        [HttpGet("get")]
        public async Task<IActionResult> GetscholarshipDetailsAsync(Guid id)
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
    }
}

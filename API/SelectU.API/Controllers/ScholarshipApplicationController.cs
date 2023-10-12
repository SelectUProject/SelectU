using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SelectU.Contracts.Config;
using SelectU.Contracts.Constants;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;
using SelectU.Contracts.Enums;
using SelectU.Contracts.Extensions;
using SelectU.Contracts.Services;
using SelectU.Core.Exceptions;
using SelectU.Core.Extensions;
using SelectU.Core.Services;


namespace SelectU.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScholarshipApplicationController : ControllerBase
    {
        private readonly ILogger<ScholarshipApplicationController> _logger;
        private readonly IUserService _userService;
        private readonly IScholarshipApplicationService _scholarshipApplicationService;
        private readonly AzureBlobSettingsConfig _azureBlobSettingsConfig;
        private readonly IBlobStorageService _blobStorageService;

        public ScholarshipApplicationController(ILogger<ScholarshipApplicationController> logger,
            IScholarshipApplicationService scholarshipApplicationService,
            IUserService userService,
            IBlobStorageService blobStorageService,
            IOptions<AzureBlobSettingsConfig> azureBlobSettingsConfig)
        {
            _logger = logger;
            _blobStorageService = blobStorageService;
            _userService = userService;
            _azureBlobSettingsConfig = azureBlobSettingsConfig.Value;
            _scholarshipApplicationService = scholarshipApplicationService;
        }

        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.Admin}, {UserRoles.Reviewer}")]
        [HttpPost("{scholarshipId}")]
        public async Task<IActionResult> GetScholarshipApplicationsAsync([FromRoute] Guid scholarshipId, [FromBody] ScholarshipApplicationSearchDTO scholarshipApplicationSearchDTO)
        {

            try
            {
                var scholarships = await _scholarshipApplicationService.GetScholarshipApplicationsAsync(
                    scholarshipId,
                    scholarshipApplicationSearchDTO
                    );

                return Ok(scholarships);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Scholarship Application, {ex.Message}");
                return BadRequest(ex.Message);
            }
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

                return Ok(new ScholarshipApplicationUpdateDTO(scholarship));
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
                await _scholarshipApplicationService.CreateScholarshipApplicationAsync(
                    scholarshipApplicationCreateDTO,
                    HttpContext.GetUserId()
                    );

                return Ok();
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

        [Authorize(Roles = $"{UserRoles.User}, {UserRoles.Admin}")]
        [HttpPost("file-upload")]
        public async Task<IActionResult> FileUploadAsync([FromForm] IFormFile file)
        {
            try
            {
                var userId = HttpContext.GetUserId();
                var user = await _userService.GetUserAsync(userId);

                if (user == null)
                {
                    return BadRequest("User not found");
                }

                string fileUri = await _blobStorageService.UploadPhotoAsync(_azureBlobSettingsConfig.PhotoContainerName, file);

                //if (!user.ProfilePicID.IsNullOrEmpty())
                //{
                //    await _blobStorageService.DeleteFileAsync(_azureBlobSettingsConfig.PhotoContainerName, user.ProfilePicID);
                //}

                return Ok(new { Message = "File uploaded successfully", FileUri = fileUri });

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ScholarshipApplicationException ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.Admin}")]
        [HttpPost("select-application")]
        public async Task<IActionResult> SelectApplication(ScholarshipApplicationUpdateDTO scholarshipApplicationUpdateDTO)
        {
            try
            {
                var userId = HttpContext.GetUserId();
                var user = await _userService.GetUserAsync(userId);

                if (user == null)
                {
                    return BadRequest("User not found");
                }

                var response = await _scholarshipApplicationService.SelectApplication(
                   scholarshipApplicationUpdateDTO,
                   HttpContext.GetUserId()
                   );

                return BadRequest("Scholarship Application failed to be selected");

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ScholarshipApplicationException ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = $"{UserRoles.User}, {UserRoles.Admin}")]
        [HttpPost("file-download")]
        public async Task<IActionResult> FileDownloadAsync([FromQuery] string fileUri)
        {
            try
            {
                var userId = HttpContext.GetUserId();
                var user = await _userService.GetUserAsync(userId);

                if (user == null)
                {
                    return BadRequest("User not found");
                }

                var file = await _blobStorageService.DownloadFileAsync(_azureBlobSettingsConfig.PhotoContainerName, fileUri);

                //if (!user.ProfilePicID.IsNullOrEmpty())
                //{
                //    await _blobStorageService.DeleteFileAsync(_azureBlobSettingsConfig.PhotoContainerName, user.ProfilePicID);
                //}

                return Ok(new { Message = "File downloaded successfully", File = file });

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ScholarshipApplicationException ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.User}, {UserRoles.Admin}")]
        [HttpGet("next-reviewable/{scholarshipId}")]
        public async Task<IActionResult> GetNextReviewableApplicationAsync([FromRoute] Guid scholarshipId)
        {
            try
            {
                var application = await _scholarshipApplicationService.GetNextReviewableApplication(scholarshipId, HttpContext.GetUserId());

                if (application == null)
                {
                    return BadRequest("No applications to review");
                }

                return Ok(application);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Scholarship Application, {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

    }
}

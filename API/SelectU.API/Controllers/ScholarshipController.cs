using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SelectU.Contracts.Config;
using SelectU.Contracts.Constants;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Enums;
using SelectU.Contracts.Services;
using SelectU.Core.Exceptions;
using SelectU.Core.Extensions;
using SelectU.Core.Helpers;
using SelectU.Core.Services;
using System.Data;
using System.Drawing;

namespace SelectU.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScholarshipController : ControllerBase
    {
        private readonly ILogger<ScholarshipController> _logger;
        private readonly IScholarshipService _scholarshipService;
        private readonly AzureBlobSettingsConfig _azureBlobSettingsConfig;
        private readonly IBlobStorageService _blobStorageService;

        public ScholarshipController(ILogger<ScholarshipController> logger, 
            IBlobStorageService blobStorageService,
            IOptions<AzureBlobSettingsConfig> azureBlobSettingsConfig,
            IScholarshipService scholarshipService)
        {
            _logger = logger;
            _scholarshipService = scholarshipService;
            _blobStorageService = blobStorageService;
            _azureBlobSettingsConfig = azureBlobSettingsConfig.Value;
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

        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.Admin}")]
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
        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.Admin}")]
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
        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.Admin}")]
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

        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.Admin}")]
        [HttpPost("photo/upload/{scholarshipId}")]
        public async Task<IActionResult> UploadPic([FromRoute] Guid scholarshipId, [FromForm] IFormFile file)
        {
            try
            {
                if (!IsImage(file))
                {
                    return BadRequest("Uploaded File is not a vaild image");
                }
                var scholarship = await _scholarshipService.GetScholarshipAsync(scholarshipId);

                if (scholarship == null)
                {
                    return BadRequest("scholarship not found");
                }

                string imageURL = await _blobStorageService.UploadPhotoAsync(_azureBlobSettingsConfig.PhotoContainerName, file);
                scholarship.ImageURL = imageURL;
                await _scholarshipService.UpdateScholarshipsAsync(scholarship);
                if (!scholarship.ImageURL.IsNullOrEmpty())
                {
                    await _blobStorageService.DeleteFileAsync(_azureBlobSettingsConfig.FileContainerName, scholarship.ImageURL);
                }

                return Ok(new { Message = "File uploaded successfully", ImageURL = imageURL });

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.Admin}")]
        [HttpDelete("photo/delete/{scholarshipId}")]
        public async Task<IActionResult> DeletePic([FromRoute] Guid scholarshipId)
        {
            try
            {
                var scholarship = await _scholarshipService.GetScholarshipAsync(scholarshipId);

                if (scholarship == null)
                {
                    return BadRequest("User not found");
                }
                bool result = false;

                if (!scholarship.ImageURL.IsNullOrEmpty())
                {
                    result = await _blobStorageService.DeleteFileAsync(_azureBlobSettingsConfig.PhotoContainerName, scholarship.ImageURL);
                }
                else
                {
                    return BadRequest(new ResponseDTO { Success = result, Message = "Picture does not exist" });
                }

                return Ok(new ResponseDTO { Success = result, Message = "Picture Delete" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.Admin}")]
        [HttpGet("photo/download/{applicationId}")]
        public async Task<IActionResult> DownloadPic([FromRoute] Guid applicationId)
        {
            try
            {

                var scholarship = await _scholarshipService.GetScholarshipAsync(applicationId);

                if (scholarship == null)
                {
                    return BadRequest("scholarship not found");
                }

                if (!scholarship.ImageURL.IsNullOrEmpty())
                {
                    var stream = await _blobStorageService.DownloadFileAsync(_azureBlobSettingsConfig.FileContainerName, scholarship.ImageURL);
                    return Ok(stream);
                }
                else
                {
                    return BadRequest(new ResponseDTO { Success = false, Message = "Picture does not exist" });
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        private bool IsImage(IFormFile file)
        {
            // Get the content type of the file
            var contentType = file.ContentType;

            // Check if the content type starts with "image/"
            return contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase);
        }
    }
}

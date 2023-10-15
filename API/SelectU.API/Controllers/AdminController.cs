using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SelectU.Contracts.Config;
using SelectU.Contracts.Constants;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Services;
using SelectU.Core.Exceptions;
using SelectU.Core.Extensions;
using SelectU.Core.Services;
using System.Drawing;
using System.IO;

namespace SelectU.API.Controllers
{
    [ApiController]
    [Authorize(Roles = $"{UserRoles.Admin}")]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IUserService _userService;
        private readonly IValidator<UserDetailsDTO> _userDetailsValidator;
        private readonly IValidator<UpdateUserRolesDTO> _userRolesUpdateValidator;

        public AdminController(
            ILogger<AdminController> logger,
            IUserService userService,
            IValidator<UserDetailsDTO> userDetailsValidator,
            IValidator<UpdateUserRolesDTO> userRolesUpdateValidator
            )
        {
            _logger = logger;
            _userService = userService;
            _userDetailsValidator = userDetailsValidator;
            _userRolesUpdateValidator = userRolesUpdateValidator;
        }
        
        [HttpPatch("details/update")]
        public async Task<IActionResult> AdminUpdateUserDetailsAsync([FromBody] UserDetailsDTO userDetails)
        {
            try
            {
                if (userDetails.Id.IsNullOrEmpty())
                {
                    return BadRequest(new ResponseDTO { Success = false, Message = "User ID is required" });
                }

                var validationResult = await _userDetailsValidator.ValidateAsync(userDetails);

                if (validationResult.IsValid)
                {
                    await _userService.UpdateUserDetailsAsync(userDetails.Id, userDetails);

                    return Ok(new ResponseDTO { Success = true, Message = "User details updated successfully." });
                }
                return BadRequest(validationResult);
            }
            catch (UserUpdateException ex)
            {
                return BadRequest(new ResponseDTO { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"UserId {userDetails.Id}, {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO { Success = false, Message = ex.Message });
            }
        }

        [HttpDelete("delete/{userId}")]
        public async Task<IActionResult> AdminDeleteUserAsync([FromRoute] Guid userId)
        {
            try
            {

                await _userService.DeleteUserAsync(userId.ToString());

                return Ok(new ResponseDTO { Success = true, Message = "User deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"User {userId}, {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO { Success = false, Message = ex.Message });
            }

        }

        [HttpPatch("roles/update")]
        public async Task<IActionResult> AdminUpdateUserRolesAsync([FromBody] UpdateUserRolesDTO updateUserRoles)
        {
            try
            {
                var validationResult = await _userRolesUpdateValidator.ValidateAsync(updateUserRoles);

                if (validationResult.IsValid)
                {
                    if (!updateUserRoles.AddRoles.IsNullOrEmpty())
                        await _userService.AddRolesToUserAsync(updateUserRoles.UserId, updateUserRoles.AddRoles);


                    if (!updateUserRoles.RemoveRoles.IsNullOrEmpty())
                        await _userService.RemoveRolesFromUserAsync(updateUserRoles.UserId, updateUserRoles.RemoveRoles);

                    return Ok(new ResponseDTO { Success = true, Message = "User roles updated successfully." });
                }
                return BadRequest(validationResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to update user roles");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("roles/{userId}")]
        public async Task<IActionResult> AdminGetUserRolesAsync([FromRoute] Guid userId)
        {
            try
            {
                var roles = await _userService.GetUserRolesAsync(userId.ToString());

                return Ok(roles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get user roles");
                return BadRequest(ex.Message);
            }
        }
    }
}

using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Services;
using SelectU.Core.Exceptions;
using SelectU.Core.Extensions;
using System.ComponentModel.DataAnnotations;

namespace SelectU.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserManagementController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserManagementService _userManagementService;
        private readonly IValidator<UserUpdateDTO> _userDetailsValidator;
        private readonly IValidator<UpdateUserRolesDTO> _userRolesUpdateValidator;
        private readonly IValidator<UpdateUserProfileDTO> _userProfileUpdateValidator;

        public UserManagementController(ILogger<UserController> logger,
            IUserManagementService userService,
            IValidator<UserUpdateDTO> userDetailsValidator, IValidator<UpdateUserProfileDTO> userProfileUpdateValidator)
        {
            _logger = logger;
            _userManagementService = userService;
            _userDetailsValidator = userDetailsValidator;
            _userProfileUpdateValidator = userProfileUpdateValidator;

        }

        [Authorize]
        [HttpGet("details/{userId}")]
        public async Task<IActionResult> GetUserDetailsAsync([FromRoute] string userId)
        {
            try
            {
                var user = await _userManagementService.GetUserAsync(userId);

                if (user == null)
                {
                    return BadRequest("User not found");
                }

                var response = new UserUpdateDTO(user);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"User {userId}, {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("list")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            try
            {
                var users = await _userManagementService.GetAllUsersAsync();

                if (users == null)
                {
                    return BadRequest("Users not found");
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get list of users");
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPatch("update")]
        public async Task<IActionResult> UpdateUserDetailsAsync([FromBody] UserUpdateDTO userDetails)
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
                    await _userManagementService.UpdateUserDetailsAsync(userDetails.Id, userDetails);

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

        [Authorize]
        [HttpPatch("profile/update")]
        public async Task<IActionResult> UpdateUserProfileAsync([FromBody] UpdateUserProfileDTO userProfile)
        {
            try
            {
                if (userProfile.UserId.IsNullOrEmpty())
                {
                    return BadRequest(new ResponseDTO { Success = false, Message = "User ID is required" });
                }

                var validationResult = await _userProfileUpdateValidator.ValidateAsync(userProfile);

                if (validationResult.IsValid)
                {
                    await _userManagementService.UpdateUserProfileAsync(userProfile.UserId, userProfile);

                    return Ok(new ResponseDTO { Success = true, Message = "User profile updated successfully." });
                }
                return BadRequest(validationResult);
            }
            catch (UserUpdateException ex)
            {
                return BadRequest(new ResponseDTO { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"UserId {userProfile.UserId}, {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO { Success = false, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("delete/{userId}")]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] string userId)
        {
            try
            {
                if (userId == null)
                {
                    return BadRequest("User ID is required");
                }

                await _userManagementService.DeleteUserAsync(userId);

                return Ok(new ResponseDTO { Success = true, Message = "User deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"User {userId}, {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO { Success = false, Message = ex.Message });
            }

        }

        [Authorize]
        [HttpGet("roles/update")]
        public async Task<IActionResult> UpdateUserRolesAsync([FromBody] UpdateUserRolesDTO updateUserRoles)
        {
            try
            {
                var validationResult = await _userRolesUpdateValidator.ValidateAsync(updateUserRoles);

                if (validationResult.IsValid)
                {
                    if (updateUserRoles.AddRoles != null)
                        await _userManagementService.AddRolesToUserAsync(updateUserRoles.userId, updateUserRoles.AddRoles);


                    if (updateUserRoles.RemoveRoles != null)
                        await _userManagementService.RemoveRolesFromUserAsync(updateUserRoles.userId, updateUserRoles.RemoveRoles);

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
    }

}
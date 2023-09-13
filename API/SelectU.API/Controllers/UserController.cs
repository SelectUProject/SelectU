using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Services;
using SelectU.Core.Exceptions;
using SelectU.Core.Extensions;

namespace SelectU.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IValidator<UserUpdateDTO> _userDetailsValidator;
        private readonly IValidator<ChangePasswordDTO> _passwordValidator;
        private readonly IValidator<UserRegisterDTO> _userRegisterValidator;
        private readonly IValidator<UpdateUserProfileDTO> _userProfileUpdateValidator;
        private readonly IValidator<UpdateUserRolesDTO> _userRolesUpdateValidator;

        public UserController(ILogger<UserController> logger,
            IUserService userService,
            IValidator<UserUpdateDTO> userDetailsValidator,
            IValidator<ChangePasswordDTO> passwordValidator,
            IValidator<UserRegisterDTO> userRegisterValidator,
            IValidator<UpdateUserProfileDTO> userProfileUpdateValidator,
            IValidator<UpdateUserRolesDTO> userRolesUpdateValidator)
        {
            _logger = logger;
            _userService = userService;
            _userDetailsValidator = userDetailsValidator;
            _passwordValidator = passwordValidator;
            _userRegisterValidator = userRegisterValidator;
            _userProfileUpdateValidator = userProfileUpdateValidator;
            _userRolesUpdateValidator = userRolesUpdateValidator;
        }

        [Authorize]
        [HttpGet("details")]
        public async Task<IActionResult> GetUserDetailsAsync()
        {
            string userId = HttpContext.GetUserId();
            try
            {
                var user = await _userService.GetUserAsync(userId);

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
        [HttpPatch("details/update")]
        public async Task<IActionResult> UpdateUserDetailsAsync([FromBody] UserUpdateDTO userDetails)
        {
            string userId = HttpContext.GetUserId();
            try
            {
                var validationResult = await _userDetailsValidator.ValidateAsync(userDetails);

                if (validationResult.IsValid)
                {
                    await _userService.UpdateUserDetailsAsync(userId, userDetails);

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
                _logger.LogError(ex, $"UserId {userId}, {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO { Success = false, Message = ex.Message });
            }
        }
        [Authorize]
        [HttpPatch("profile/update")]
        public async Task<IActionResult> UpdateUserProfileAsync([FromBody] UpdateUserProfileDTO userProfile)
        {
            string userId = HttpContext.GetUserId();
            try { 

                var validationResult = await _userProfileUpdateValidator.ValidateAsync(userProfile);

                if (validationResult.IsValid)
                {
                    await _userService.UpdateUserProfileAsync(userId, userProfile);

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
        [HttpPatch("change-password")]
        public async Task<IActionResult> ChangeUserPasswordAsync([FromBody] ChangePasswordDTO passwordDTO)
        {
            string userId = HttpContext.GetUserId();
            try
            {
                var validationResult = await _passwordValidator.ValidateAsync(passwordDTO);

                if (validationResult.IsValid)
                {
                    await _userService.ChangePasswordAsync(userId, passwordDTO);

                    return Ok(new ResponseDTO { Success = true, Message = "Password updated successfully." });
                }

                return BadRequest(validationResult);
            }
            catch (ChangePasswordException ex)
            {
                return BadRequest(new ResponseDTO { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"User {userId}, {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO { Success = false, Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(UserRegisterDTO registerDTO)
        {
            try
            {
                var validationResult = await _userRegisterValidator.ValidateAsync(registerDTO);

                if (validationResult.IsValid)
                {
                    ResponseDTO response;

                    await _userService.RegisterUserAsync(registerDTO);

                    response = new ResponseDTO { Success = true, Message = "User created successfully." };

                    return Ok(response);
                }
                return BadRequest(validationResult);
            }
            catch (UserRegisterException ex)
            {
                return BadRequest(new ResponseDTO { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO { Success = false, Message = ex.Message });
            }
        }

        [HttpPost("validate")]
        public async Task<IActionResult> ValidateUniqueEmailAddressAsync([FromBody] ValidateUniqueEmailAddressRequestDTO userDetails)
        {

            try
            {
                var response = await _userService.ValidateUniqueEmailAddressAsync(userDetails.Email);

                return Ok(response);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO { Success = false, Message = ex.Message });
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            try
            {
                await _userService.ForgotPasswordAsync(forgotPasswordDto);

                return Ok(new ResponseDTO { Success = true, Message = "Thanks, check your email for instructions to reset your password" });
            }
            catch (Exception ex)
            {
                //Return Ok as we do not want to reveal legitimate email accounts within the system
                return Ok(new ResponseDTO { Success = true, Message = "Thanks, check your email for instructions to reset your password" });
            }
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            try
            {
                await _userService.ResetPasswordAsync(resetPasswordDto);

                return Ok(new ResponseDTO { Success = true, Message = "Password reset successfully." });
            }
            catch (ResetPasswordException ex)
            {
                return BadRequest(new ResponseDTO { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO { Success = false, Message = ex.Message });
            }
        }
        [Authorize]
        [HttpGet("details/{userId}")]
        public async Task<IActionResult> GetUserDetailsAsync([FromRoute] Guid userId)
        {
            try
        {
                var user = await _userService.GetUserAsync(userId.ToString());

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
                var users = await _userService.GetAllUsersAsync();

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
        [HttpPatch("admin/details/update")]
        public async Task<IActionResult> AdminUpdateUserDetailsAsync([FromBody] UserUpdateDTO userDetails)
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

        [Authorize]
        [HttpPatch("admin/profile/update")]
        public async Task<IActionResult> AdminUpdateUserProfileAsync([FromBody] UpdateUserProfileDTO userProfile)
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
                    await _userService.UpdateUserProfileAsync(userProfile.UserId, userProfile);

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
        [HttpDelete("admin/delete/{userId}")]
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

        [Authorize]
        [HttpPatch("admin/roles/update")]
        public async Task<IActionResult> AdminUpdateUserRolesAsync([FromBody] UpdateUserRolesDTO updateUserRoles)
        {
            try
            {
                var validationResult = await _userRolesUpdateValidator.ValidateAsync(updateUserRoles);

                if (validationResult.IsValid)
                {
                    if (updateUserRoles.AddRoles != null)
                        await _userService.AddRolesToUserAsync(updateUserRoles.UserId, updateUserRoles.AddRoles);


                    if (updateUserRoles.RemoveRoles != null)
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
        [Authorize]
        [HttpGet("admin/roles/details/{userId}")]
        public async Task<IActionResult> AdminGetUserRolesAsync([FromRoute] Guid userId)
        {
            try
            {
                var roles = await _userService.GetUserRolesAsync(userId.ToString());

                return Ok(new ResponseDTO { Success = true, Message = "User roles updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get user roles");
                return BadRequest(ex.Message);
            }
        }
    }
}

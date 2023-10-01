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
using System.IO;

namespace SelectU.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly AzureBlobSettingsConfig _azureBlobSettingsConfig;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IValidator<UserDetailsDTO> _userDetailsValidator;
        private readonly IValidator<ChangePasswordDTO> _passwordValidator;
        private readonly IValidator<UserRegisterDTO> _userRegisterValidator;
        private readonly IValidator<UpdateUserRolesDTO> _userRolesUpdateValidator;

        public UserController(ILogger<UserController> logger,
            IUserService userService,
            IBlobStorageService blobStorageService,
            IOptions<AzureBlobSettingsConfig> azureBlobSettingsConfig,
            IValidator<UserDetailsDTO> userDetailsValidator,
            IValidator<ChangePasswordDTO> passwordValidator,
            IValidator<UserRegisterDTO> userRegisterValidator,
            IValidator<UpdateUserRolesDTO> userRolesUpdateValidator)
        {
            _logger = logger;
            _userService = userService;
            _blobStorageService = blobStorageService;
            _azureBlobSettingsConfig = azureBlobSettingsConfig.Value;
            _userDetailsValidator = userDetailsValidator;
            _passwordValidator = passwordValidator;
            _userRegisterValidator = userRegisterValidator;
            _userRolesUpdateValidator = userRolesUpdateValidator;
            _blobStorageService = blobStorageService;
        }
        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.User}")]
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

                var response = new UserDetailsDTO(user);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"User {userId}, {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.User}")]
        [Authorize]
        [HttpPatch("details/update")]
        public async Task<IActionResult> UpdateUserDetailsAsync([FromBody] UserDetailsDTO userDetails)
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
        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.User}")]
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
        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.User}")]
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

        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.User}")]
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

        [Authorize(Roles = $"{UserRoles.Staff}")]
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

                var response = new UserDetailsDTO(user);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"User {userId}, {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = $"{UserRoles.Staff}")]
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
        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.User}")]
        [HttpPost("photo/{userId}/upload")]
        public async Task<IActionResult> UploadProfilePic([FromRoute] string userId, [FromBody] Stream file)
        {
            try
            {
                var user = await _userService.GetUserAsync(userId);

                if (user == null)
                {
                    return BadRequest("User not found");
                }

                string imageID = await _blobStorageService.UploadFileAsync(_azureBlobSettingsConfig.ProfilePicContainerName, file);
                var updateUser = new UserDetailsDTO(user);
                updateUser.ProfilePicID = imageID;
                await _userService.UpdateUserDetailsAsync(userId, updateUser);

                if (!user.ProfilePicID.IsNullOrEmpty())
                {
                    await _blobStorageService.DeleteFileAsync(_azureBlobSettingsConfig.ProfilePicContainerName, user.ProfilePicID);
                }

                return Ok(new { Message = "File uploaded successfully", ImageID = imageID });

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
        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.User}")]
        [HttpDelete("photo/{userId}/delete")]
        public async Task<IActionResult> DeleteProfilePic([FromRoute] string userId)
        {
            try
            {
                var user = await _userService.GetUserAsync(userId);

                if (user == null)
                {
                    return BadRequest("User not found");
                }
                bool result = false;

                if (!user.ProfilePicID.IsNullOrEmpty()) 
                {
                    result = await _blobStorageService.DeleteFileAsync(_azureBlobSettingsConfig.ProfilePicContainerName, user.ProfilePicID);
                }
                else
                {
                    return BadRequest(new ResponseDTO { Success = result, Message = "Profile Picture does not exist" });
                }

                return Ok(new ResponseDTO { Success = result, Message = "Profile Picture Delete" });
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
        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.User}")]
        [HttpGet("photo/{userId}/download")]
        public async Task<IActionResult> DownloadProfilePic([FromRoute] string userId)
        {
            try
            {

                var user = await _userService.GetUserAsync(userId);

                if (user == null)
                {
                    return BadRequest("User not found");
                }
                Stream stream = null;

                if (!user.ProfilePicID.IsNullOrEmpty())
                {
                    stream = await _blobStorageService.DownloadFileAsync(_azureBlobSettingsConfig.ProfilePicContainerName, user.ProfilePicID);
                }
                else
                {
                    return BadRequest(new ResponseDTO { Success = false, Message = "Profile Picture does not exist" });
                }

                return Ok(stream);
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


        [Authorize(Roles = $"{UserRoles.Admin}")]
        [Authorize]
        [HttpPatch("admin/details/update")]
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
        [Authorize(Roles = $"{UserRoles.Admin}")]
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
        [Authorize(Roles = $"{UserRoles.Admin}")]
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
        [Authorize(Roles = $"{UserRoles.Admin}")]
        [Authorize]
        [HttpGet("admin/roles/details/{userId}")]
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

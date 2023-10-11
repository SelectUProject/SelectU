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
        private readonly IValidator<UserInviteDTO> _userInviteValidator;
        private readonly IValidator<LoginExpiryUpdateDTO> _loginExpiryUpdateValidator;

        public UserController(ILogger<UserController> logger,
            IUserService userService,
            IBlobStorageService blobStorageService,
            IOptions<AzureBlobSettingsConfig> azureBlobSettingsConfig,
            IValidator<UserDetailsDTO> userDetailsValidator,
            IValidator<ChangePasswordDTO> passwordValidator,
            IValidator<UserRegisterDTO> userRegisterValidator,
            IValidator<UpdateUserRolesDTO> userRolesUpdateValidator,
            IValidator<UserInviteDTO> userInviteValidator,
            IValidator<LoginExpiryUpdateDTO> loginExpiryUpdateValidator)
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
            _userInviteValidator = userInviteValidator;
            _loginExpiryUpdateValidator = loginExpiryUpdateValidator;
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
        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.User}, {UserRoles.Admin}")]
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

        [HttpPost]
        [Route("google-register")]
        public async Task<IActionResult> GoogleRegister(GoogleAuthDTO authDTO)
        {
            try
            {
                ResponseDTO response;

                await _userService.RegisterGoogleUserAsync(authDTO);

                response = new ResponseDTO { Success = true, Message = "User created successfully." };

                return Ok(response);
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

        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.Admin}")]
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
        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.Admin}")]
        [Authorize]
        [HttpGet("list")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();

                List<UserDetailsDTO> response = new List<UserDetailsDTO>();

                foreach(var user in users)
                {
                    var userRoles = await _userService.GetUserRolesAsync(user.Id) as List<string>;
                    //double check if role is in this model 
                    response.Add(new UserDetailsDTO(user));
                }

                if (users == null)
                {
                    return BadRequest("Users not found");
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get list of users");
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.User}, {UserRoles.Admin}")]
        [HttpPost("photo/{userId}/upload")]
        public async Task<IActionResult> UploadProfilePic([FromRoute] string userId, [FromBody] Stream file)
        {
            try
            {
                if (!IsValidImage(file))
                {
                    return BadRequest("Uploaded File is not a vaild image");
                }
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

        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.User}, {UserRoles.Admin}")]
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

        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.User}, {UserRoles.Admin}")]
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

                if (!user.ProfilePicID.IsNullOrEmpty())
                {
                    var stream = await _blobStorageService.DownloadFileAsync(_azureBlobSettingsConfig.ProfilePicContainerName, user.ProfilePicID);
                    return Ok(stream);
                }
                else
                {
                    return BadRequest(new ResponseDTO { Success = false, Message = "Profile Picture does not exist" });
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

        private bool IsValidImage(Stream file)
        {
            try
            {
                using (Image newImage = Image.FromStream(file))
                { }
            }
            catch (OutOfMemoryException ex)
            {
                //The file does not have a valid image format.
                //-or- GDI+ does not support the pixel format of the file

                return false;
            }
            return true;
        }

        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.Admin}")]
        [HttpPost("invite")]
        public async Task<IActionResult> Invite(UserInviteDTO inviteDTO)
        {
            try
            {
                var validationResult = await _userInviteValidator.ValidateAsync(inviteDTO);

                if (validationResult.IsValid)
                {
                    ResponseDTO response;

                    await _userService.InviteUserAsync(inviteDTO);

                    response = new ResponseDTO { Success = true, Message = "User invited successfully." };

                    return Ok(response);
                }
                return BadRequest(validationResult);
            }
            catch (UserInviteException ex)
            {
                return BadRequest(new ResponseDTO { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO { Success = false, Message = ex.Message });
            }
        }

        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.Admin}")]
        [Authorize]
        [HttpPatch("login-expiry/{userId}")]
        public async Task<IActionResult> UpdateLoginExpiry([FromRoute] string userId, [FromBody] LoginExpiryUpdateDTO updateDTO)
        {
            try
            {
                if (userId.IsNullOrEmpty())
                {
                    return BadRequest(new ResponseDTO { Success = false, Message = "User ID is required" });
                }

                var validationResult = await _loginExpiryUpdateValidator.ValidateAsync(updateDTO);

                if (validationResult.IsValid)
                {
                    await _userService.UpdateLoginExpiryAsync(userId, updateDTO);

                    return Ok(new ResponseDTO { Success = true, Message = "User login expiry updated successfully." });
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
    }
}

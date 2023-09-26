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
    public class TempUserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly ITempUserService _tempUserService;
        private readonly AzureBlobSettingsConfig _azureBlobSettingsConfig;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IValidator<UserUpdateDTO> _userDetailsValidator;
        private readonly IValidator<ChangePasswordDTO> _passwordValidator;
        private readonly IValidator<UserRegisterDTO> _userRegisterValidator;
        private readonly IValidator<UpdateUserRolesDTO> _userRolesUpdateValidator;

        public TempUserController(ILogger<UserController> logger,
            IUserService userService,
            ITempUserService tempUserService,
            IBlobStorageService blobStorageService,
            IOptions<AzureBlobSettingsConfig> azureBlobSettingsConfig,
            IValidator<UserUpdateDTO> userDetailsValidator,
            IValidator<ChangePasswordDTO> passwordValidator,
            IValidator<UserRegisterDTO> userRegisterValidator,
            IValidator<UpdateUserRolesDTO> userRolesUpdateValidator)
        {
            _logger = logger;
            _userService = userService;
            _tempUserService = tempUserService;
            _blobStorageService = blobStorageService;
            _azureBlobSettingsConfig = azureBlobSettingsConfig.Value;
            _userDetailsValidator = userDetailsValidator;
            _passwordValidator = passwordValidator;
            _userRegisterValidator = userRegisterValidator;
            _userRolesUpdateValidator = userRolesUpdateValidator;
            _blobStorageService = blobStorageService;
        }

        [HttpPost("validate")]
        public async Task<IActionResult> ValidateUniqueEmailAddressAsync([FromBody] ValidateUniqueEmailAddressRequestDTO userDetails)
        {

            try
            {
                if (userDetails == null || userDetails.Email == null)
                {
                    return BadRequest(new ResponseDTO { Success = false, Message = "Invalid request" });
                }

                var response = await _tempUserService.ValidateUniqueEmailAddressAsync(userDetails.Email);

                return Ok(response);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO { Success = false, Message = ex.Message });
            }
        }
    }
}

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
        private readonly ILogger<TempUserController> _logger;
        private readonly IUserService _userService;
        private readonly ITempUserService _tempUserService;
        private readonly IValidator<TempUserInviteDTO> _tempUserInviteValidator;

        public TempUserController(
            ILogger<TempUserController> logger,
            IUserService userService,
            ITempUserService tempUserService,
            IValidator<TempUserInviteDTO> tempUserInviteValidator
            )
        {
            _logger = logger;
            _userService = userService;
            _tempUserService = tempUserService;
            _tempUserInviteValidator = tempUserInviteValidator;
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

        [HttpPost("invite")]
        public async Task<IActionResult> Register(TempUserInviteDTO inviteDTO)
        {
            try
            {
                var validationResult = await _tempUserInviteValidator.ValidateAsync(inviteDTO);

                if (validationResult.IsValid)
                {
                    ResponseDTO response;

                    await _tempUserService.InviteTempUserAsync(inviteDTO);

                    response = new ResponseDTO { Success = true, Message = "User created successfully." };

                    return Ok(response);
                }
                return BadRequest(validationResult);
            }
            catch (TempUserException ex)
            {
                return BadRequest(new ResponseDTO { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO { Success = false, Message = ex.Message });
            }
        }
    }
}

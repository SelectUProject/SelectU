﻿using FluentValidation;
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
        private readonly IValidator<TempUserUpdateDTO> _tempUserUpdateValidator;

        public TempUserController(
            ILogger<TempUserController> logger,
            IUserService userService,
            ITempUserService tempUserService,
            IValidator<TempUserInviteDTO> tempUserInviteValidator,
            IValidator<TempUserUpdateDTO> tempUserUpdateValidator
            )
        {
            _logger = logger;
            _userService = userService;
            _tempUserService = tempUserService;
            _tempUserInviteValidator = tempUserInviteValidator;
            _tempUserUpdateValidator = tempUserUpdateValidator;
        }

        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.Admin}")]
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

        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.Admin}")]
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

        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.Admin}")]
        [HttpGet("list")]
        public async Task<IActionResult> GetAllTempUsersAsync()
        {
            try
            {
                var tempUsers = await _tempUserService.GetTempUsersAsync();

                return Ok(tempUsers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get list of temp users");
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = $"{UserRoles.Staff}, {UserRoles.Admin}")]
        [Authorize]
        [HttpPatch("login-expiry")]
        public async Task<IActionResult> UpdateLoginExpiry(TempUserUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO.Id.IsNullOrEmpty())
                {
                    return BadRequest(new ResponseDTO { Success = false, Message = "User ID is required" });
                }

                var validationResult = await _tempUserUpdateValidator.ValidateAsync(updateDTO);

                if (validationResult.IsValid)
                {
                    await _tempUserService.UpdateTempUserExpiryAsync(updateDTO.Id, updateDTO);

                    return Ok(new ResponseDTO { Success = true, Message = "User details updated successfully." });
                }
                return BadRequest(validationResult);
            }
            catch (TempUserException ex)
            {
                return BadRequest(new ResponseDTO { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"UserId {updateDTO.Id}, {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO { Success = false, Message = ex.Message });
            }
        }
    }
}
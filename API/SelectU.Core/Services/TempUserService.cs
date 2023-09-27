using Microsoft.AspNetCore.Identity;
using SelectU.Contracts;
using SelectU.Contracts.Constants;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;
using SelectU.Contracts.Enums;
using SelectU.Contracts.Infrastructure;
using SelectU.Contracts.Services;
using SelectU.Core.Exceptions;
using SelectU.Core.Helpers;

namespace SelectU.Core.Services
{
    public class TempUserService : ITempUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailClient _emailclient;
        private readonly IUnitOfWork _unitOfWork;

        public TempUserService(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IEmailClient emailClient,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailclient = emailClient;
            _unitOfWork = unitOfWork;
        }

        public async Task<ValidateUniqueEmailAddressResponseDTO> ValidateUniqueEmailAddressAsync(string emailAddress)
        {
            ValidateUniqueEmailAddressResponseDTO response = new()
            {
                IsUnique = true
            };

            var user = await _userManager.FindByEmailAsync(emailAddress);
            if (user != null)
            {
                response.IsUnique = false;
                response.IsTempUser = _userManager.IsInRoleAsync(user, UserRoleEnum.Reviewer.ToString()).Result;
                if (response.IsTempUser == true)
                {
                    response.Message = "This email address is already registered as a temporary user.";
                }
                else
                {
                    response.Message = "This email address is already registered and cannot be invited as a reviewer";
                }
            }

            return response;
        }

        public async Task InviteTempUserAsync(TempUserInviteDTO inviteDTO)
        {
            var user = new User
            {
                Email = inviteDTO.Email,
                FirstName = inviteDTO.FirstName,
                LastName = inviteDTO.LastName,
                UserName = inviteDTO.Email,
                LoginExpiry = inviteDTO.Expiry,
                SecurityStamp = Guid.NewGuid().ToString(),
                DateCreated = DateTimeOffset.UtcNow,
                DateModified = DateTimeOffset.UtcNow
            };

            var password = PasswordHelper.GenerateRandomPassword(12, 1);

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                throw new TempUserException("Failed to create User.");
            }

            if (await _roleManager.RoleExistsAsync(UserRoles.Reviewer))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Reviewer);
            }

            await _emailclient.SendTempUserInviteEmailASync(inviteDTO, password);
        }

        Task ITempUserService.UpdateTempUserExpiryAsync()
        {
            throw new NotImplementedException();
        }

    }
}

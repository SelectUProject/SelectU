using Microsoft.AspNetCore.Identity;
using SelectU.Contracts;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;
using SelectU.Contracts.Enums;
using SelectU.Contracts.Infrastructure;
using SelectU.Contracts.Services;

namespace SelectU.Core.Services
{
    public class TempUserService : ITempUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailClient _emailclient;
        private readonly IUnitOfWork _unitOfWork;

        public TempUserService(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IEmailClient emailClient,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
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

        Task ITempUserService.InviteTempUserAsync()
        {
            throw new NotImplementedException();
        }

        Task ITempUserService.UpdateTempUserExpiryAsync()
        {
            throw new NotImplementedException();
        }

    }
}

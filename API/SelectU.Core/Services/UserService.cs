using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SelectU.Contracts;
using SelectU.Contracts.Config;
using SelectU.Contracts.Constants;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;
using SelectU.Contracts.Enums;
using SelectU.Contracts.Infrastructure;
using SelectU.Contracts.Services;
using SelectU.Core.Exceptions;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace SelectU.Core.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailClient _emailclient;
        private readonly IUnitOfWork _unitOfWork;
        private readonly GoogleConfig _googleConfig;

        public UserService(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IEmailClient emailClient,
            IUnitOfWork unitOfWork,
            IOptions<GoogleConfig> googleConfig)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailclient = emailClient;
            _unitOfWork = unitOfWork;
            _googleConfig = googleConfig.Value;
        }

        public async Task<User> GetUserAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<ValidateUniqueEmailAddressResponseDTO> ValidateUniqueEmailAddressAsync(string email)
        {
            ValidateUniqueEmailAddressResponseDTO response = new ValidateUniqueEmailAddressResponseDTO()
            {
                IsUnique = true
            };

            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                response.IsUnique = false;
            }

            return response;
        }

        public async Task RegisterUserAsync(UserRegisterDTO registerDTO)
        {
            if(registerDTO.Email == null) 
            {
                throw new UserRegisterException("Please provide an email.");
            }

            var existingUser = await _userManager.FindByNameAsync(registerDTO.Email);

            if (existingUser != null)
            {
                throw new UserRegisterException("An account already exists for provided email address.");
            }

            foreach (var validator in _userManager.PasswordValidators)
            {
                var validationResult = await validator.ValidateAsync(_userManager, null!, registerDTO.Password);

                if (!validationResult.Succeeded)
                {
                    throw new ChangePasswordException("Password does not meet the required criteria.");
                }
            }

            var user = new User
            {
                PhoneNumber = registerDTO.PhoneNumber,
                Email = registerDTO.Email,
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                DateOfBirth = registerDTO.DateOfBirth,
                Gender = registerDTO.Gender ?? GenderEnum.Other,
                State = registerDTO.State,
                Country = registerDTO.Country,
                UserName = registerDTO.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                DateCreated = DateTimeOffset.UtcNow,
                DateModified = DateTimeOffset.UtcNow
            };

            if (registerDTO.Password == null)
            {
                throw new UserRegisterException("Please provide a password.");
            }

            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded)
            {
                throw new UserRegisterException("Failed to create User.");
            }

            if (await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.User);
            }

            await _emailclient.SendRegistrationEmailASync(registerDTO);
        }

        public async Task RegisterGoogleUserAsync(GoogleAuthDTO authDTO)
        {
            ValidationSettings settings = new ValidationSettings();

            settings.Audience = new List<string>() { _googleConfig.ClientId };

            Payload payload = await ValidateAsync(authDTO.IdToken, settings);

            var existingUser = await _userManager.FindByNameAsync(payload.Email);

            if (existingUser != null)
            {
                throw new UserRegisterException("An account already exists for the email attached to this account.");
            }

            var user = new User
            {
                Email = payload.Email,
                FirstName = payload.GivenName,
                LastName = payload.FamilyName,
                UserName = payload.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                DateCreated = DateTimeOffset.UtcNow,
                DateModified = DateTimeOffset.UtcNow
            };

            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                throw new UserRegisterException("Failed to create User.");
            }

            if (await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.User);
            }

            await _emailclient.SendRegistrationEmailASync(new UserRegisterDTO(payload));
        }

        public async Task UpdateUserDetailsAsync(string id, UserUpdateDTO updateDTO)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                throw new UserUpdateException("User not found.");
            }

            user.Email = updateDTO.Email;
            user.FirstName = updateDTO.FirstName;
            user.LastName = updateDTO.LastName;
            user.DateOfBirth = updateDTO.DateOfBirth;
            user.Gender = updateDTO.Gender;
            user.PhoneNumber = updateDTO.PhoneNumber;
            user.Address = updateDTO.Address;
            user.Suburb = updateDTO.Suburb;
            user.Postcode = updateDTO.Postcode;
            user.State = updateDTO.State;
            user.UserName = updateDTO.Email;
            user.DateModified = DateTimeOffset.Now;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new UserUpdateException("Failed to update user details.");
            }
        }
        public async Task ChangePasswordAsync(string id, ChangePasswordDTO passwordDTO)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                throw new ChangePasswordException("User not found.");
            }

            foreach (var validator in _userManager.PasswordValidators)
            {
                var validationResult = await validator.ValidateAsync(_userManager, null!, passwordDTO.NewPassword);

                if (!validationResult.Succeeded)
                {
                    throw new ChangePasswordException("New password does not meet the required criteria.");
                }
            }

            var result = await _userManager.ChangePasswordAsync(user, passwordDTO.OldPassword, passwordDTO.NewPassword);

            if (!result.Succeeded)
            {
                throw new ChangePasswordException("Failed to change password.");
            }

            await _userManager.ResetAccessFailedCountAsync(user);
        }

        public async Task ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
            if (user == null)
            {
                throw new ForgotPasswordException($"Cannot find user with this email. {forgotPasswordDto.Email}");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var param = new Dictionary<string, string>
            {
                {"token", token },
                {"email", forgotPasswordDto.Email }
            };

            var callback = QueryHelpers.AddQueryString(forgotPasswordDto.ClientURI, param);

            //await _emailclient.SendUserResetPasswordEmailAsync(user, callback);
        }


        public async Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
            {
                throw new ResetPasswordException("Cannot find user.");
            }

            foreach (var validator in _userManager.PasswordValidators)
            {
                var validationResult = await validator.ValidateAsync(_userManager, null!, resetPasswordDto.Password);

                if (!validationResult.Succeeded)
                {
                    throw new ResetPasswordException("New password does not meet the required criteria.");
                }
            }

            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);

            if (!resetPassResult.Succeeded)
            {
                throw new ResetPasswordException("Failed to reset password.");
            }

            await _userManager.ResetAccessFailedCountAsync(user);
        }
        public async Task<ICollection<User>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }
        public async Task DeleteUserAsync(string userId)
        {
            if (userId.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null");
            }
            var user = await GetUserAsync(userId);
            if (user != null)
            {
                user = await GetUserAsync(userId);
                await _userManager.Users.Where(x => x.Equals(user)).ExecuteDeleteAsync();
            }
            else
                throw new NotFoundException("User not Found");
        }
        public async Task AddRolesToUserAsync(string userId, ICollection<string> roleNames)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await _userManager.AddToRolesAsync(user, roleNames);
            }
            else
            {
                throw new NotFoundException("User not Found");
            }
        }

        public async Task RemoveRolesFromUserAsync(string userId, ICollection<string> roleNames)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await _userManager.RemoveFromRolesAsync(user, roleNames);
            }
            else
            {
                throw new NotFoundException("User not Found");
            }
        }

        public async Task<ICollection<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                return await _userManager.GetRolesAsync(user);
            }
            else
            {
                throw new NotFoundException("User not Found");
            }
        }
    }
}

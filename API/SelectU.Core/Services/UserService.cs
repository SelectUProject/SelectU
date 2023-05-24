using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using SelectU.Contracts;
using SelectU.Contracts.Constants;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;
using SelectU.Contracts.Infrastructure;
using SelectU.Contracts.Services;
using SelectU.Core.Exceptions;

namespace SelectU.Core.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailClient _emailclient;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IEmailClient emailClient,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailclient = emailClient;
            _unitOfWork = unitOfWork;
        }

        public async Task<User> GetUserAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task RegisterUserAsync(UserRegisterDTO registerDTO)
        {
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
                Mobile = registerDTO.Mobile,
                Email = registerDTO.Email,
                FullName = registerDTO.FullName,
                DateOfBirth = registerDTO.DateOfBirth,
                Gender = registerDTO.Gender,
                Address = registerDTO.Address,
                Suburb = registerDTO.Suburb,
                Postcode = registerDTO.Postcode,
                State = registerDTO.State,
                UserName = registerDTO.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                DateCreated = DateTimeOffset.UtcNow,
                DateModified = DateTimeOffset.UtcNow,
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded)
            {
                throw new UserRegisterException("Failed to create User.");
            }

            if (await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.User);
            }

            //Send email
            //await _emailclient.SendRegistrationEmailASync(registerDTO);
        }

        public async Task UpdateUserDetailsAsync(string id, UserUpdateDTO updateDTO)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                throw new UserUpdateException("User not found.");
            }

            user.Email = updateDTO.Email;
            user.FullName = updateDTO.FullName;
            user.DateOfBirth = updateDTO.DateOfBirth;
            user.Gender = updateDTO.Gender;
            user.Mobile = updateDTO.Mobile;
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
    }
}

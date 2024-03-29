﻿using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;

namespace SelectU.Contracts.Services
{
    public interface IUserService
    {
        Task<User> GetUserAsync(string id);
        Task<ValidateUniqueEmailAddressResponseDTO> ValidateUniqueEmailAddressAsync(string email);
        Task RegisterUserAsync(UserRegisterDTO registerDTO);
        Task RegisterGoogleUserAsync(GoogleAuthDTO authDTO);
        Task UpdateUserDetailsAsync(string id, UserDetailsDTO updateDTO);
        Task ChangePasswordAsync(string id, ChangePasswordDTO passwordDTO);
        Task ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
        Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task<List<User>> GetAllUsersAsync();
        Task RemoveRolesFromUserAsync(string id, ICollection<string> roleNames);
        Task AddRolesToUserAsync(string id, ICollection<string> roleNames);
        Task<IList<string>> GetUserRolesAsync(string id);
        Task DeleteUserAsync(string id);
        Task InviteUserAsync(UserInviteDTO inviteDTO);
        Task UpdateLoginExpiryAsync(string id, LoginExpiryUpdateDTO updateDTO);

    }
}

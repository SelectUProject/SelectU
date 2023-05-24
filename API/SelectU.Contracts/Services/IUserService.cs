﻿using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;

namespace SelectU.Contracts.Services
{
    public interface IUserService
    {
        Task<User> GetUserAsync(string id);
        Task RegisterUserAsync(UserRegisterDTO registerDTO);
        Task UpdateUserDetailsAsync(string id, UserUpdateDTO updateDTO);
        Task ChangePasswordAsync(string id, ChangePasswordDTO passwordDTO);
        Task ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
        Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
    }
}

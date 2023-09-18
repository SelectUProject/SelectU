using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;

namespace SelectU.Contracts.Services
{
    public interface IUserService
    {
        Task<User> GetUserAsync(string id);
        Task<ValidateUniqueEmailAddressResponseDTO> ValidateUniqueEmailAddressAsync(string email);
        Task RegisterUserAsync(UserRegisterDTO registerDTO);
        Task UpdateUserDetailsAsync(string id, UserUpdateDTO updateDTO);
        Task ChangePasswordAsync(string id, ChangePasswordDTO passwordDTO);
        Task ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
        Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task<ICollection<User>> GetAllUsersAsync();
        Task RemoveRolesFromUserAsync(string id, ICollection<string> roleNames);
        Task AddRolesToUserAsync(string id, ICollection<string> roleNames);
        Task<ICollection<string>> GetUserRolesAsync(string id);
        Task DeleteUserAsync(string id);
    }
}

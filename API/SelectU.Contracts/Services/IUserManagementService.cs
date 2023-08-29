using Microsoft.AspNetCore.Identity;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;

namespace SelectU.Contracts.Services
{
    public interface IUserManagementService :IUserService
    {
        Task<ICollection<User>> GetAllUsersAsync();
        Task RemoveRolesFromUserAsync(string userId, ICollection<string> roleNames);
        Task AddRolesToUserAsync(string userId, ICollection<string> roleNames);
        Task DeleteUserAsync(string userId);
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SelectU.Contracts;
using SelectU.Contracts.Constants;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;
using SelectU.Contracts.Enums;
using SelectU.Contracts.Infrastructure;
using SelectU.Contracts.Services;
using SelectU.Core.Exceptions;
using SelectU.Core.Infrastructure;

namespace SelectU.Core.Services
{

    public class UserManagementService : UserService, IUserManagementService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailClient _emailclient;
        private readonly IUnitOfWork _unitOfWork;
        public UserManagementService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IEmailClient emailClient, IUnitOfWork unitOfWork) : base(userManager, roleManager, emailClient, unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailclient = emailClient;
            _unitOfWork = unitOfWork;
        }
        public async Task<ICollection<User>> GetAllUsersAsync()
        {
            return _userManager.Users.ToList();
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
            User user = await _userManager.FindByIdAsync(userId);
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
    }
}

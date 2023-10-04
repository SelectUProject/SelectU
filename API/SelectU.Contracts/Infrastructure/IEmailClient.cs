using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;

namespace SelectU.Contracts.Infrastructure
{
    public interface IEmailClient
    {
        Task SendRegistrationEmailAsync(UserRegisterDTO registerDto);
        Task SendUserInviteEmailAsync(UserInviteDTO inviteDTO, string password);
        //Task SendTestEmail();
        //Task SendUserResetPasswordEmailAsync(User user, string callback);
    }
}
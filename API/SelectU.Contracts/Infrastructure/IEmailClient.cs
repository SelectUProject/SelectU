using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;

namespace SelectU.Contracts.Infrastructure
{
    public interface IEmailClient
    {
        Task SendRegistrationEmailASync(UserRegisterDTO registerDto);
        //Task SendTestEmail();
        //Task SendUserResetPasswordEmailAsync(User user, string callback);
    }
}
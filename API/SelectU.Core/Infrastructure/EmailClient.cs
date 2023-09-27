using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Web;
using SelectU.Contracts.Config;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;
using SelectU.Contracts.Enums;
using SelectU.Contracts.Extensions;
using SelectU.Contracts.Infrastructure;


namespace SelectU.Core.Infrastructure
{
    public class EmailClient : IEmailClient
    {
        private readonly AppConfig _appConfig;
        private readonly SmtpConfig _smtpConfig;
        private readonly EmailConfig _emailConfig;

        public EmailClient(IOptions<AppConfig> appConfig, IOptions<SmtpConfig> smtpConfig, IOptions<EmailConfig> emailConfig)
        {
            _appConfig = appConfig.Value;
            _smtpConfig = smtpConfig.Value;
            _emailConfig = emailConfig.Value;
        }

        public async Task SendRegistrationEmailASync(UserRegisterDTO registerDto)
        {
            MailMessage mail = new MailMessage
            {
                Subject = _emailConfig.RegistrationEmailSubject
            };

            var loginUrl = _appConfig.PortalURL + "/login";

            var style = "<style>body{font-family:Arial,sans-serif;background-color:#f5f5f5;margin:0;padding:0;}.container{max-width:600px;margin:0auto;padding:20px;background-color:#ffffff;border-radius:5px;box-shadow:0px 2px 6px rgba(0, 0, 0, 0.1);}.header{text-align:center;padding:20px 0;}.logo{max-width:150px;height:auto;}.content{padding:20px 0;}.button{display:inline-block;padding:10px 20px;background-color:#007bff;color:#ffffff;text-decoration:none;border-radius:4px;font-weight:bold;}</style>";
            mail.Body = $"<html><body><div class=\"container\"><div class=\"header\"><img class=\"logo\" src=\"https://example.com/logo.png\" alt=\"Company Logo\"><h1>Welcome to SelectU!</h1></div><div class=\"content\"><p>Dear {registerDto.Email},</p><p>Thank you for registering with us. We're excited to have you on board!</p><p>To get started, please click the button below:</p><p><a class=\"button\" href=\"{loginUrl}\">Activate Your Account</a></p><p>Best regards,<br>Your Team at SelectU</p></div></div></body></html>";
            mail.To.Add(new MailAddress(registerDto.Email));
            await SendEmailAsync(mail);
        }

        public async Task SendTempUserInviteEmailASync(TempUserInviteDTO inviteDTO, string password)
        {
            MailMessage mail = new MailMessage
            {
                Subject = "Reviewer Invitation"
            };

            var loginUrl = _appConfig.PortalURL + "/login";

            var style = "<style>body{font-family:Arial,sans-serif;background-color:#f5f5f5;margin:0;padding:0;}.container{max-width:600px;margin:0auto;padding:20px;background-color:#ffffff;border-radius:5px;box-shadow:0px 2px 6px rgba(0, 0, 0, 0.1);}.header{text-align:center;padding:20px 0;}.logo{max-width:150px;height:auto;}.content{padding:20px 0;}.button{display:inline-block;padding:10px 20px;background-color:#007bff;color:#ffffff;text-decoration:none;border-radius:4px;font-weight:bold;}</style>";
            mail.Body = $"<html><body><div class=\"container\"><div class=\"header\"><img class=\"logo\" src=\"https://example.com/logo.png\" alt=\"Company Logo\"><h1>Welcome to SelectU!</h1></div><div class=\"content\"><p>Dear {inviteDTO.Email},</p><p>You've been invited as a temporary reviewer.</p><p><b>Access Until:</b> {inviteDTO.Expiry}</p><p><b>Username:</b> {inviteDTO.Email}</p><p><b>Password:</b> {password}</p><p>To get started, please click the button below:</p><p><a class=\"button\" href=\"{loginUrl}\">Login</a></p><p>Best regards,<br>Your Team at SelectU</p></div></div></body></html>";
            mail.To.Add(new MailAddress(inviteDTO.Email));
            await SendEmailAsync(mail);
        }

        public async Task SendTestEmail()
        {
            MailMessage mail = new MailMessage
            {
                Subject = "Test email"
            };

            //Apply template to body
            mail.Body = $"This is a test";
            mail.To.Add(new MailAddress("102580449@student.swin.edu.au"));
            await SendEmailAsync(mail);
        }

        //public async Task SendUserResetPasswordEmailAsync(User user, string callback)
        //{
        //    MailMessage mail = new MailMessage
        //    {
        //        Subject = "Reset Password"
        //    };

        //    //Apply template to body
        //    var style = "";
        //    mail.Body = $"";
        //    mail.To.Add(new MailAddress(user.Email));
        //    await SendEmailAsync(mail);
        //}

        private async Task SendEmailAsync(MailMessage mailMessage)
        {
            SmtpClient client = new SmtpClient();
            mailMessage.IsBodyHtml = true;
            mailMessage.From = new MailAddress(_emailConfig.FromEmail, "SelectU");
            mailMessage.Subject = string.Format("{0}", mailMessage.Subject);

            if (_appConfig.EmailDebugMode)
            {
                mailMessage.Body += string.Format("<br><br><small>Sent to {0}</small>", mailMessage.To.FirstOrDefault().Address);
                mailMessage.To.Clear();
                mailMessage.CC.Clear();
                //Add developers
                mailMessage.To.Add(new MailAddress(_emailConfig.DebugEmail));
            }

            client.Host = _smtpConfig.Host;
            client.Port = _smtpConfig.Port;

            //Username & password
            if (!string.IsNullOrEmpty(_smtpConfig.Username) && !string.IsNullOrEmpty(_smtpConfig.Password))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_smtpConfig.Username, _smtpConfig.Password);
                client.EnableSsl = true;
            }

            await client.SendMailAsync(mailMessage);
        }
    }
}

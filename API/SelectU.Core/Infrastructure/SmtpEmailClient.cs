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
    public class SmtpEmailClient : IEmailClient
    {
        private readonly AppConfig _appConfig;
        private readonly SmtpConfig _smtpConfig;
        private readonly string emailHtmlScript = "";
        //private readonly string emailHtmlScript = "<script type=\"text/javascript\">function getWidth(){return Math.max(document.body.scrollWidth,document.documentElement.scrollWidth,document.body.offsetWidth,document.documentElement.offsetWidth,document.documentElement.clientWidth)}function getHeight(){return Math.max(document.body.scrollHeight,document.documentElement.scrollHeight,document.body.offsetHeight,document.documentElement.offsetHeight,document.documentElement.clientHeight)}window.onload=function(){document.body,document.documentElement;var t=getHeight(),e=(getWidth(),new CustomEvent(\"emailIframeResize\",{detail:{height:t,width:800}}));window.parent.document.dispatchEvent(e)}</script>";

        public SmtpEmailClient(IOptions<AppConfig> appConfig, IOptions<SmtpConfig> smtpConfig)
        {
            _appConfig = appConfig.Value;
            _smtpConfig = smtpConfig.Value;
        }

        //public async Task SendRegistrationEmailASync(UserRegisterDTO registerDto)
        //{
        //    MailMessage mail = new MailMessage
        //    {
        //        Subject = _emailConfig.RegistrationEmailSubject
        //    };

        //    //Apply template to body
        //    var style = "";
        //    mail.Body = $"";
        //    mail.To.Add(new MailAddress(registerDto.Email));
        //    await SendEmailAsync(mail);
        //}

        //public async Task SendTestEmail()
        //{
        //    MailMessage mail = new MailMessage
        //    {
        //        Subject = "Test email"
        //    };

        //    //Apply template to body
        //    mail.Body = $"This is a test";
        //    mail.To.Add(new MailAddress("102580449@student.swin.edu.au"));
        //    await SendEmailAsync(mail);
        //}

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

        //private async Task SendEmailAsync(MailMessage mailMessage)
        //{
        //    SmtpClient client = new SmtpClient();
        //    mailMessage.IsBodyHtml = true;
        //    mailMessage.From = new MailAddress(_emailConfig.FromEmail, "SelectU");
        //    mailMessage.Subject = string.Format("{0}", mailMessage.Subject);

        //    if (_appConfig.EmailDebugMode)
        //    {
        //        mailMessage.Body += string.Format("<br><br><small>Sent to {0}</small>", mailMessage.To.FirstOrDefault().Address);
        //        mailMessage.To.Clear();
        //        mailMessage.CC.Clear();
        //        //Add developers
        //        mailMessage.To.Add(new MailAddress(_emailConfig.DebugEmail));
        //    }

        //    client.Host = _smtpConfig.Host;
        //    client.Port = _smtpConfig.Port;

        //    //Username & password
        //    if (!string.IsNullOrEmpty(_smtpConfig.Username) && !string.IsNullOrEmpty(_smtpConfig.Password))
        //    {
        //        client.UseDefaultCredentials = false;
        //        client.Credentials = new NetworkCredential(_smtpConfig.Username, _smtpConfig.Password);
        //        client.EnableSsl = true;
        //    }

        //    await client.SendMailAsync(mailMessage);
        //}
    }
}

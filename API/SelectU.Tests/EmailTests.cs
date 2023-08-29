using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Threading.Tasks;
using SelectU.Contracts.Config;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Infrastructure;
using SelectU.Core.Infrastructure;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace SelectU.Tests
{
    [TestFixture]
    public class EmailTests
    {
        private IConfigurationRoot _configuration;

        [SetUp]
        public void Setup()
        {
            var projectDir = Directory.GetCurrentDirectory();
            var configPath = Path.Combine(projectDir, "appsettings.json");

            _configuration = new ConfigurationBuilder()
                .AddJsonFile(configPath)
                .Build();
        }

        [Test]
        [Ignore("Ignore registration email test")]
        public async Task Test_SendRegistrationEmailAsync_NoException()
        {
            var appConfig = new AppConfig();
            _configuration.GetSection("Config").Bind(appConfig);

            var smtpConfig = new SmtpConfig();
            _configuration.GetSection("Smtp").Bind(smtpConfig);

            var emailConfig = new EmailConfig();
            _configuration.GetSection("Email").Bind(emailConfig);

            var emailClient = new EmailClient(
                Options.Create(appConfig),
                Options.Create(smtpConfig),
                Options.Create(emailConfig)
            );

            var registerDto = new UserRegisterDTO
            {
                Email = "test@example.com",
            };

            var mailMessage = new MailMessage();

            Assert.DoesNotThrowAsync(async () => await emailClient.SendRegistrationEmailASync(registerDto));
        }
    }
}

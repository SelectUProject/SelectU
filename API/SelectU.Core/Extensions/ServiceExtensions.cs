using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Email;
using System.Net;
using SelectU.Contracts;
using SelectU.Contracts.Config;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;
using SelectU.Contracts.Infrastructure;
using SelectU.Contracts.Services;
using SelectU.Core.Infrastructure;
using SelectU.Core.Services;
using SelectU.Core.Validators;
using SelectU.Migrations;
using Microsoft.VisualBasic.FileIO;

namespace SelectU.Core.Extensions
{
    public static class ServiceExtensions
    {
        public static WebApplicationBuilder AddSerilogLogging(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((hostingContext, loggerConfig) =>
                    loggerConfig.ReadFrom.Configuration(hostingContext.Configuration)
                );

            return builder;
        }
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddDbContext<SelectUContext>(options => options.UseSqlServer(configuration.GetConnectionString("SelectUContext"),
                sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)));
        }

        public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
        {
            // Add config
            services.Configure<SmtpConfig>(configuration.GetSection("Smtp"))
                    .AddSingleton(x => x.GetRequiredService<IOptions<SmtpConfig>>().Value);
            services.Configure<EmailConfig>(configuration.GetSection("Email"))
                    .AddSingleton(x => x.GetRequiredService<IOptions<EmailConfig>>().Value);
            services.Configure<ServiceBusConfig>(configuration.GetSection("ServiceBus"))
                    .AddSingleton(x => x.GetRequiredService<IOptions<ServiceBusConfig>>().Value);
            services.Configure<AppConfig>(configuration.GetSection("Config"))
                    .AddSingleton(x => x.GetRequiredService<IOptions<AppConfig>>().Value);
            services.Configure<AzureBlobSettingsConfig>(configuration.GetSection("AzureBlobSettings"))
                    .AddSingleton(x => x.GetRequiredService<IOptions<AzureBlobSettingsConfig>>().Value);

            services.AddMemoryCache();
            // Add services to the container.
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //infrastructure
            services.AddScoped<IServiceBusQueueClient, ServiceBusQueueClient>();
            services.AddScoped<IEmailClient, EmailClient>();
            services.AddScoped<ICache, InMemoryCache>();

            services.AddScoped<IBlobStorageService, BlobStorageService>();
            services.AddScoped<ICachingService, CachingService>();
            services.AddScoped<IScholarshipApplicationService, ScholarshipApplicationService>();
            services.AddScoped<IScholarshipService, ScholarshipService>();
            services.AddScoped<ITempUserService, TempUserService>();
            services.AddScoped<IUserService, UserService>();

            //Validators
            services.AddScoped<IValidator<ChangePasswordDTO>, ChangePasswordDTOValidator>();
            services.AddScoped<IValidator<TempUserInviteDTO>, TempUserInviteDTOValidator>();
            services.AddScoped<IValidator<UpdateUserRolesDTO>, UpdateUserRolesDTOValidator>();
            services.AddScoped<IValidator<UserRegisterDTO>, UserRegisterDTOValidator>();
            services.AddScoped<IValidator<UserUpdateDTO>, UserUpdateDTOValidator>();
            return services;
        }
        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            // For Identity
            services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
             .AddEntityFrameworkStores<SelectUContext>()
             .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Default Lockout settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(20);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            });

            return services;
        }

        /// <summary>
        /// Reads serilog config from appsettings
        /// </summary>
        /// <param name = "services" ></ param >
        /// < param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddSerilogToAPI(this IServiceCollection services, IConfiguration configuration)
        {
            EmailConnectionInfo emailConnection = GetEmailConnectionInfo(configuration);

            var logger = new LoggerConfiguration()
           .ReadFrom.Configuration(configuration)
           .WriteTo.Email(emailConnection, restrictedToMinimumLevel: LogEventLevel.Error, batchPostingLimit: 1)
           .CreateLogger();
            services.AddLogging(lb => lb.AddSerilog(logger, dispose: true));
            return services;
        }
        /// <summary>
        /// Serilog config written in code
        /// </summary>
        /// <param name = "services" ></ param >
        /// < param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddSerilogToFunction(this IServiceCollection services, IConfiguration configuration)
        {
            var errorfilepath = !string.IsNullOrEmpty(System.Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME")) ? @"D:\home\LogFiles\Application\errorlog.txt" : "Logs/Errorlog.txt";

            EmailConnectionInfo emailConnection = GetEmailConnectionInfo(configuration);

            Serilog.Debugging.SelfLog.Enable(Console.WriteLine);
            var logger = new LoggerConfiguration()
           .WriteTo.File(errorfilepath, rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Error)
           .WriteTo.Email(emailConnection, restrictedToMinimumLevel: LogEventLevel.Error, batchPostingLimit: 1)
           .CreateLogger();
            services.AddLogging(lb => lb.AddSerilog(logger, dispose: true));
            return services;
        }


        private static EmailConnectionInfo GetEmailConnectionInfo(IConfiguration configuration)
        {
            var emailConfig = configuration.GetSection("Email").Get<EmailConfig>();
            var stmpConfig = configuration.GetSection("Smtp").Get<SmtpConfig>();

            var emailConnection = new EmailConnectionInfo
            {
                FromEmail = emailConfig.FromEmail,
                ToEmail = emailConfig.DebugEmail,
                MailServer = stmpConfig.Host,
                Port = stmpConfig.Port,
                EmailSubject = emailConfig.ExceptionEmailSubject ?? "SelectU - Exception"
            };

            if (stmpConfig.UseSSL == true)
            {
                emailConnection.EnableSsl = false;
                emailConnection.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => true;

                emailConnection.SecureSocketOption = SecureSocketOptions.StartTls;
            }

            if (stmpConfig.Username != null)
            {
                emailConnection.NetworkCredentials = new NetworkCredential
                {
                    UserName = stmpConfig.Username,
                    Password = stmpConfig.Password
                };
            };
            return emailConnection;
        }
    }
}


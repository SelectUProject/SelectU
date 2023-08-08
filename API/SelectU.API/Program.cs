using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SelectU.Migrations;
using SelectU.Contracts.Entities;
using SelectU.Core.Extensions;
using SelectU.Contracts.Services;
using SelectU.Core.Services;

var builder = WebApplication.CreateBuilder(args);
//builder.AddSerilogLogging();

ConfigurationManager configuration = builder.Configuration;

var services = builder.Services;

//Add core services
services.AddPersistence(configuration);
services.AddCore(configuration);
services.AddIdentity();
services.AddSerilogToAPI(configuration);
services.AddTransient<IBlobStorageService>(provider => new AzureBlobStorageService(configuration.GetConnectionString("AzureBlobStorageConnectionString") ?? "", configuration));

// Adding Authentication
services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateLifetime = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };
});

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SelectUContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    //Same as the question
    if (db.Database.GetPendingMigrations().Any())
    {
        db.Database.Migrate();
    }

    BIHSeed.SeedBaseData(db, userManager, roleManager);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

string[] origins = configuration.GetValue<string>("AllowedOrigins")?.Split(";").ToArray() ?? new string[1] { "" };

app.UseCors(x => x
.AllowAnyMethod()
.AllowAnyHeader()
.WithOrigins(origins)
//.SetIsOriginAllowed(origin => true) // allow any origin
.AllowCredentials()); // allow credentials

app.UseHttpsRedirection();

// Authentication & Authorization
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

//Log.logger = new LoggerConfiguration()
// .WriteTo.Console()
//.CreateLogger();
//app.UseElmah();

app.Run();

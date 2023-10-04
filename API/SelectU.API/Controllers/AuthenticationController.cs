using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;
using Google.Apis.Auth;
using static Google.Apis.Auth.GoogleJsonWebSignature;
using SelectU.Contracts.Config;
using Microsoft.Extensions.Options;


namespace SelectU.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly GoogleConfig _googleConfig;

        public AuthenticateController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            IOptions<GoogleConfig> googleConfig)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _googleConfig = googleConfig.Value;
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var user = await _userManager.FindByNameAsync(loginDTO.Username!);
            if (user != null)
            {
                if(user.LoginExpiry != null && user.LoginExpiry < DateTimeOffset.UtcNow)
                {
                    return BadRequest(new ResponseDTO { Success = false, Message = "Your login has expired. Please contact your administrator." });
                }

                var result = await _signInManager.PasswordSignInAsync(user, loginDTO.Password!, true, false);

                if (result.Succeeded)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);

                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName!),
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var token = GetToken(authClaims);

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        role = userRoles.FirstOrDefault(),
                        expiration = token.ValidTo
                    });
                }
            }
            return BadRequest(new ResponseDTO { Success = false, Message = "Incorrect email or password" }); ;
        }

        [HttpPost]
        [Route("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleAuthDTO authDTO)
        {
            ValidationSettings settings = new ValidationSettings();

            settings.Audience = new List<string>() { _googleConfig.ClientId };

            Payload payload = await ValidateAsync(authDTO.IdToken, settings);

            var user = await _userManager.FindByNameAsync(payload.Email);
            if (user != null)
            {
                if (user.LoginExpiry != null && user.LoginExpiry < DateTimeOffset.UtcNow)
                {
                    return BadRequest(new ResponseDTO { Success = false, Message = "Your login has expired. Please contact your administrator." });
                }

                await _signInManager.SignInAsync(user, true);

                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = GetToken(authClaims);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    role = userRoles.FirstOrDefault(),
                    expiration = token.ValidTo
                });
            }

            return BadRequest(new ResponseDTO { Success = false, Message = "No user associated with this Google account." }); ;
        }

        [Authorize]
        [HttpGet("validate-token")]
        public IActionResult ValidateToken()
        {
            return Ok(new ResponseDTO { Success = true, Message = "Valid Token" });
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

    }
}

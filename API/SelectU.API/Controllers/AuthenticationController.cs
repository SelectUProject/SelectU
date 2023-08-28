using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;


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

        public AuthenticateController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var user = await _userManager.FindByNameAsync(loginDTO.Username!);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginDTO.Password!, true, true);

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

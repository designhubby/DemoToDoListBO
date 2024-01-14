using DemoToListBE.Configuration;
using DemoToListBE.Data.Authentication;
using DemoToListBE.Dto.Requests.Auth;
using DemoToListBE.Dto.Responses;
using DemoToListBE.Logic.Service.AppService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DemoToListBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthManagementController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtConfig _jwtConfig;

        public AuthManagementController(UserManager<ApplicationUser> userManager, IApplicationToDoListService applicationToDoListService, IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(user.Email);
                if(existingUser != null)
                {
                    return BadRequest(new ProblemDetails()
                    {
                        Title = "Email Already Taken",
                        Status = StatusCodes.Status406NotAcceptable,
                        Detail = "Registration Failed",
                    }); 
                }
                var newUser = new ApplicationUser()
                {
                    Email = user.Email,
                    UserName = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                };
                var isCreated = await _userManager.CreateAsync(newUser, user.Password);
                if (isCreated.Succeeded)
                {
                    var jwtToken = GenerateJwtToken(newUser);
                    Response.Cookies.Append("Todo_jwt_token", jwtToken, new CookieOptions { HttpOnly = true, IsEssential = true, SameSite = SameSiteMode.None, Secure = true, Expires = DateTime.UtcNow.AddHours(6) });
                    return Ok(new RegistrationResponse
                    {
                        Success = true,
                        Token = jwtToken,
                        Expiry = DateTime.UtcNow.AddHours(6),
                    });
                }
                else
                {
                    return BadRequest(new RegistrationResponse
                    {
                        Errors = isCreated.Errors.Select(x => x.Description).ToList(),
                        Success = false,
                    });
                }
            }
            return BadRequest();
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(UserLoginRequest user)
        {
            if (ModelState.IsValid)
            {
                var existinguser = await _userManager.FindByEmailAsync(user.Email);
                if(existinguser == null)
                {
                    return BadRequest("Invalid Login Request");
                }
                var pwdIsCorrect = await _userManager.CheckPasswordAsync(existinguser, user.Password);
                if (!pwdIsCorrect)
                {
                    return BadRequest("Invalid Login Request");
                }
                var jwt = GenerateJwtToken(existinguser);
                string toDoExpiry = DateTime.UtcNow.AddHours(6).ToString("yyyyMMddHHmmss");
                Response.Cookies.Append("Todo_jwt_token", jwt, new CookieOptions { HttpOnly = true, IsEssential = true, SameSite = SameSiteMode.None, Secure = true, Expires = DateTime.UtcNow.AddHours(6)});
                Response.Cookies.Append("Todo_ExpiryData", toDoExpiry, new CookieOptions { HttpOnly = false, IsEssential = true, SameSite = SameSiteMode.None, Secure = true, Expires = DateTime.UtcNow.AddHours(6)});
                return Ok(new RegistrationResponse
                {
                    Success = true,
                    Token = jwt,
                    Expiry = DateTime.UtcNow.AddHours(6),
                });
            }

            return BadRequest("InvalidPayload");
        }
        [HttpPost("AuthenticationStatus")]
        public ActionResult AuthenticationStatus()
        {
            if(User.Identity.IsAuthenticated == true)
            {
                return Ok(true);
            }
            return Ok(false);
        }
        [HttpGet("GetUser")]
        [Authorize]

        public async Task<ActionResult<UserRegistrationDto>> GetUser()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            UserRegistrationDto userRegistrationDto = new UserRegistrationDto()
            {

                Username = currentUser.UserName,
                Email = currentUser.Email,
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
            };
            return Ok(userRegistrationDto);

        }
        [HttpPost("EditUser")]
        [Authorize]
        public async Task<ActionResult<UserRegistrationDto>> EditUser(UserRegistrationDto userRegistrationDto)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);

                currentUser.Email = userRegistrationDto.Email;
                currentUser.FirstName = userRegistrationDto.FirstName;
                currentUser.LastName = userRegistrationDto.LastName;
                if (userRegistrationDto.Password != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(currentUser);
                    var result = await _userManager.ResetPasswordAsync(currentUser, token, userRegistrationDto.Password);
                }
                await _userManager.UpdateAsync(currentUser);
                var edittedUser = await _userManager.GetUserAsync(User);
                UserRegistrationDto edittedUserRegistrationDto = new UserRegistrationDto()
                {

                    Username = edittedUser.UserName,
                    Email = edittedUser.Email,
                    FirstName = edittedUser.FirstName,
                    LastName = edittedUser.LastName,
                };
                return Ok(edittedUserRegistrationDto);
            }

            return BadRequest("InvalidPayload");

        }

        [HttpPost("Logout")]
        [Authorize]
        public void Logout()
        {
            Response.Cookies.Delete("Todo_jwt_token", new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                IsEssential = true,
                Secure = true,
                Expires = new DateTimeOffset(DateTime.Now)
            }); 
            Response.Cookies.Delete("Todo_ExpiryData", new CookieOptions
            {
                HttpOnly = false,
                SameSite = SameSiteMode.None,
                IsEssential = true,
                Secure = true,
                Expires = new DateTimeOffset(DateTime.Now)
            });
        }
        

        public string GenerateJwtToken(ApplicationUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);
            return jwtToken;
        }




    }
}

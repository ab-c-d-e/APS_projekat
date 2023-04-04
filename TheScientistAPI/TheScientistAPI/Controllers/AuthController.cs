using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TheScientistAPI.DTOs;
using TheScientistAPI.Model;

namespace TheScientistAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userMenager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<ApplicationUser> userMenager, IConfiguration configuration)
        {
            _userMenager = userMenager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterDto userDto)
        {
            if(ModelState.IsValid)
            {
                var user_exists = await _userMenager.FindByEmailAsync(userDto.Email);
                if (user_exists != null) return BadRequest(new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Email alredy exists, try loging in!"
                    }
                });

                var user_new = new ApplicationUser()
                {
                    FirstName=userDto.FirstName,
                    LastName=userDto.LastName,
                    UserName=userDto.UserName,
                    Email=userDto.Email,
                    UserRoles=new List<UserRole>(),
                    Messages=new List<MessageUser>()
                };

                var created= await _userMenager.CreateAsync(user_new, userDto.Password);

                if(created.Succeeded)
                {
                    var token = GenerateJwtToken(user_new);
                    return Ok(new AuthResult()
                    {
                        Result = true,
                        Token = token
                    });
                }

                return BadRequest(new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Server Error"
                    }
                });
            }
            else return new JsonResult("Data you entered is incorrect") { StatusCode = 500 };
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDto userDto)
        {
            if (ModelState.IsValid)
            {
                var user_exists = await _userMenager.FindByEmailAsync(userDto.Email);
                if (user_exists == null)
                    return BadRequest(new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>()
                    {
                        "This email is not connected to any account, try singing up!"
                    }
                    });
                var combination_correct = await _userMenager.CheckPasswordAsync(user_exists, userDto.Password);
                if(!combination_correct)
                    return BadRequest(new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>()
                    {
                        "This email and password don't match!"
                    }
                    });
                var token = GenerateJwtToken(user_exists);

                return Ok(new AuthResult()
                {
                    Result = true,
                    Token = token
                });
            }
            else return new JsonResult("Data you entered is incorrect") { StatusCode = 500 };
        }

        string GenerateJwtToken(ApplicationUser user)
        {
            var jwtTokenHandeler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtSettings:SecretKey").Value);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Name, user.FirstName+" "+user.LastName),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString()),
                }),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256)
            };

            var token = jwtTokenHandeler.CreateToken(tokenDescriptor);
            var jwtToken= jwtTokenHandeler.WriteToken(token);

            return jwtToken;
        }
    }
}
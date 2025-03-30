    using core.Models;
    using core.Service;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;


    namespace apiProject.Controllers
    {
        [Route("api/auth/")]
        [ApiController]
        public class AuthController : ControllerBase
        {


            private readonly IUsersService _usersService;
            private readonly IConfiguration _configuration;


            public AuthController(IUsersService usersService, IConfiguration configuration)
            {
                _usersService = usersService;
                _configuration = configuration;
            }



            [HttpPost("signUp")]
            public IActionResult Register([FromBody] UserDto newUser)
            {
                if (_usersService.GetAll().Any(u => u.Name == newUser.Username))
                {
                    return BadRequest("User already exists.");
                }

                var user = new Users
                {
                    Name = newUser.Username,
                    Password = newUser.Password, // 👈 יש להצפין סיסמאות במערכת אמיתית!
                    Role = newUser.Role,
                    Phone = "",
                    Status = "Active",
                    Description = ""
                };

                _usersService.AddUser(user);
                return Ok(new { message = "User registered successfully." });
            }




            [HttpPost("login")]
            public IActionResult Login([FromBody] UserDto userDto)
            {
                var user = _usersService.GetAll().FirstOrDefault(u => u.Name == userDto.Username);

                if (user == null || user.Password != userDto.Password)
                {
                    return Unauthorized("Invalid username or password.");
                }

                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role.ToString()) 
        };

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var tokenOptions = new JwtSecurityToken(
                    issuer: _configuration["JWT:Issuer"],
                    audience: _configuration["JWT:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(6),
                    signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                //return Ok(new { Token = tokenString });
                    return Ok(new { message = $"Welcome {user.Name}", role = user.Role,Token=tokenString });

            }

        }


    }

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WeatherApi.Data;
using WeatherApi.Models;
using WeatherApi.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WeatherApi.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _userEmailStore;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public UsersController(SignInManager<ApplicationUser> signInManager, 
            AppDbContext context, IUserStore<ApplicationUser> userStore, 
            IUserEmailStore<ApplicationUser> userEmailStore,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _signInManager = signInManager;
            _context = context;
            _userStore = userStore;
            _userEmailStore = userEmailStore;
            _userManager = userManager;
            _configuration = configuration;
        }


        [HttpGet]
        public IActionResult Get()
        {
            return User.Identity.IsAuthenticated ? Ok("Hello World") : Unauthorized();
        }

        // GET api/<ValuesController>/5
        [HttpGet("login")]
        public async Task<IActionResult> Get(string login, string password, bool rememberMe = false)
        {
            if(string.IsNullOrEmpty(login) && string.IsNullOrEmpty(password))
            {
                return BadRequest();
            }
            var userName = login.Contains('@')?_context.Users.Where(x=>x.Email==login).Select(x=>x.UserName).FirstOrDefault() 
                : _context.Users.Where(x=>x.UserName==login).Select(x=>x.UserName).FirstOrDefault();
            if (userName == null)
            {
                return NotFound("User not found");
            }
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return NotFound("User not found");
            }
            var result = await _userManager.CheckPasswordAsync(user, password);
            if(result)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);  
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("username", user.UserName),
                        new Claim("email", user.Email),
                        new Claim("identifikator", user.Id),
                        new Claim("paid", user.PaidAccount.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddHours(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Ok(new { Token = tokenString });
            }
            return Unauthorized("User cannot be authorized");
        }

        // POST api/<ValuesController>
        [HttpPost("register")]
        public async Task<IActionResult> Post([FromBody] UserIM input)
        {
            var emailInDb = _context.Users.Where(x => x.Email == input.Email).FirstOrDefault();
            if (emailInDb != null)
            {
                return BadRequest("Email already exists");
            }

            var user = new ApplicationUser
            {
                Email = input.Email,
                NormalizedEmail = input.Email.ToUpper(),
                EmailConfirmed = true,
                UserName = input.UserName,
                NormalizedUserName = input.UserName.ToUpper(),
                PaidAccount = false,
                Id = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, input.Password);

            if (result.Succeeded)
            {
                if (user.PaidAccount)
                {
                    await _userManager.AddClaimAsync(user, new Claim("PaidAccount", "true"));
                }
                return Ok("User created");
            }
            else
            {
                return BadRequest(result.Errors);
            }

        }

        // PUT api/<ValuesController>/5
        [Authorize]
        [HttpPut("pay")]
        public void Pay(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

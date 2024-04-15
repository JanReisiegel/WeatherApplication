using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Weather.Data;
using Weather.Models;
using Weather.Services;
using Weather.ViewModels;

namespace Weather.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;
        private readonly UserServices _userService;
        public UsersController(IConfiguration config, AppDbContext context, UserServices userService)
        {
            _config = config;
            _context = context;
            _userService = userService;
        }  

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            var user = _userService.GetUserByEmail(model.Email);
            var verified = new PasswordHasher<ApplicationUser>().VerifyHashedPassword(null, user.PasswordHash, model.Password);
            if(verified == PasswordVerificationResult.Success && user != null)
            {
                var token = _userService.GenerateJwtToken(user);
                return Ok(new { Token = token });
            }
            return Unauthorized();
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Weather.Models;
using Weather.Services;
using Weather.ViewModels;

namespace Weather.Controllers
{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserServices _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        public UsersController(UserServices userService, UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _userManager = userManager;

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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                PaidAccount = false,
                NormalizedEmail = model.Email.ToUpper(),
                NormalizedUserName = model.UserName.ToUpper(),
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if(result.Succeeded)
            {
                return Ok("User was Created");
            }
            return BadRequest(result.Errors);
        }

        //[Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteUser()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var result = await _userManager.DeleteAsync(user);
            if(result.Succeeded)
            {
                return Ok("User was deleted");
            }
            return BadRequest(result.Errors);
        }

        //[Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UserVM model)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            user.UserName =String.IsNullOrEmpty(model.UserName) ? user.UserName : model.UserName;
            user.Email = String.IsNullOrEmpty(model.Email) ? user.UserName : model.Email;
            user.PhoneNumber = String.IsNullOrEmpty(model.PhoneNumber) ? user.UserName : model.PhoneNumber;
            user.SavedLocations =  model.Locations;
            user.PaidAccount = model.PaidAccount;
            var result = await _userManager.UpdateAsync(user);
            if(result.Succeeded)
            {
                return Ok("User was updated");
            }
            return BadRequest(result.Errors);
        }
        //[Authorize]
        [HttpGet("one")]
        public async Task<ActionResult<UserVM>> GetUser()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if(user == null)
            {
                return NotFound();
            }
            var result = new UserVM
            {
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Locations = user.SavedLocations,
                PaidAccount = user.PaidAccount
            };
            return Ok(result);
        }

        [HttpGet("all")]
        public ActionResult<List<UserVM>> GetAllUsers()
        {
            List<ApplicationUser> users = _userService.GetAllUsers().Result;
            List<UserVM> usersVM = new List<UserVM>();
            users.ForEach(user =>
            {
                usersVM.Add(new UserVM
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Locations = user.SavedLocations,
                    PaidAccount = user.PaidAccount
                });
            });
            return Ok(usersVM);
        }
    }
}

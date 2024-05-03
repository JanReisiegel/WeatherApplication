using Microsoft.AspNetCore.Authorization;
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
    [Route("api/Users")]
    [ApiController]
    public class UsersController: ControllerBase
    {

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await JsonFileService.GetUserAsync(model.Email);
            var verified = new PasswordHasher<ApplicationUser>().VerifyHashedPassword(null, user.PasswordHash, model.Password);
            if(verified == PasswordVerificationResult.Success && user != null)
            {
                var token = UserServices.GenerateJwtToken(user);
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
            IdentityResult result = null;
            try { result = await JsonFileService.AddUserAsync(model); }
            catch(Exception e) { return BadRequest(e.Message);}
            if(result.Succeeded)
            {
                return Ok("User was Created");
            }
            return BadRequest(result.Errors);
        }

        //[Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromHeader] string userToken, [FromBody]LoginModel loginModel)
        {
            var claims = UserServices.GetClaims(userToken);
            var user = await JsonFileService.GetUserAsync(claims["email"]);
            var result = await JsonFileService.DeleteUserAsync(loginModel);
            if(result.Succeeded)
            {
                return Ok("User was deleted");
            }
            return BadRequest(result.Errors);
        }

        //[Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromHeader]string userToken, [FromBody] UserVM model)
        {
            var claims = UserServices.GetClaims(userToken);
            var user = await JsonFileService.GetUserAsync(claims["email"]);
            var result = await JsonFileService.UpdateUserAsync(model);
            if(result.Succeeded)
            {
                return Ok("User was updated");
            }
            return BadRequest(result.Errors);
        }
        //[Authorize]
        [HttpGet("one")]
        public async Task<ActionResult<UserVM>> GetUserInfo([FromHeader]string userToken)
        {
            var user = await JsonFileService.GetUserAsync(UserServices.GetClaims(userToken)["email"]);
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
        public async Task<ActionResult<List<UserVM>>> GetAllUsers()
        {
            List<ApplicationUser> users = await JsonFileService.GetUsersAsync();
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

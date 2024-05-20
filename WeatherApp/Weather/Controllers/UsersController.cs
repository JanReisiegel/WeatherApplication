using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Weather.Models;
using Weather.MyExceptions;
using Weather.Services;
using Weather.ViewModels;

namespace Weather.Controllers
{
    [Route("api/Users")]
    [EnableCors]
    [ApiController]
    public class UsersController: ControllerBase
    {

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await JsonFileService.GetUserAsync(model.Email);
            if(user == null)
            {
                return Unauthorized("User not found");
            }
            var verified = new PasswordHasher<ApplicationUser>().VerifyHashedPassword(null, user.PasswordHash, model.Password);
            if(verified == PasswordVerificationResult.Success && user != null)
            {
                var token = UserServices.GenerateJwtToken(user);
                return Ok(new { Token = token });
            }
            return Unauthorized("Invalid Password");
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

        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromHeader] string? userToken, [FromBody]LoginModel loginModel)
        {
            IDictionary<string, string> claims = null;
            if (userToken == null)
            {
                return Unauthorized("You need to be logged in to delete your account");
            }
            try { claims = UserServices.GetClaims(userToken); }
            catch(Exception e) { return StatusCode(StatusCodes.Status406NotAcceptable,e.Message); }
            if (await UserServices.GetAuthenticate(claims["email"]) == false) return Unauthorized("You need to be logged in to delete your account");
            if (claims["email"] != loginModel.Email) return Unauthorized("You can only delete your own account");
            var user = await JsonFileService.GetUserAsync(claims["email"]);
            IdentityResult result = null;
            try
            {
                result = await JsonFileService.DeleteUserAsync(loginModel);
            }
            catch(Exception e)
            {
                return Unauthorized(e.Message);
            }
            if(result.Succeeded)
            {
                return Ok("User was deleted");
            }
            return BadRequest(result.Errors);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromHeader]string? userToken, [FromBody] UserVM model)
        {
            IDictionary<string, string> claims = null;
            if (userToken == null)
            {
                return Unauthorized("You need to be logged in to delete your account");
            }
            try { claims = UserServices.GetClaims(userToken); }
            catch (Exception e) { return Unauthorized(e.Message); }
            if (await UserServices.GetAuthenticate(claims["email"]) == false) return Unauthorized("You need to be logged in to delete your account");
            if (claims["email"] != model.Email) return Unauthorized("You can only update your own account");
            var user = await JsonFileService.GetUserAsync(claims["email"]);
            var result = await JsonFileService.UpdateUserAsync(model);
            if(result.Succeeded)
            {
                return Ok("User was updated");
            }
            return BadRequest(result.Errors);
        }

        [HttpGet("one")]
        public async Task<IActionResult> GetUserInfo([FromHeader]string? userToken)
        {
            if (userToken == null)
            {
                return Unauthorized("You need to be logged in to delete your account");
            }
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
        public async Task<IActionResult> GetAllUsers([FromHeader]string? userToken)
        {
            if (userToken == null || !(await UserServices.GetAuthenticate(UserServices.GetClaims(userToken)["email"])))
            {
                return Unauthorized("You need to be logged in to delete your account");
            }
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

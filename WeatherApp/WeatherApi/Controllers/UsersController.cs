using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
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

        public UsersController(SignInManager<ApplicationUser> signInManager, 
            AppDbContext context, IUserStore<ApplicationUser> userStore, 
            IUserEmailStore<ApplicationUser> userEmailStore,
            UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _context = context;
            _userStore = userStore;
            _userEmailStore = userEmailStore;
            _userManager = userManager;
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

            var result = await _signInManager.PasswordSignInAsync(userName, password, rememberMe, false);

            if (result.Succeeded)
            {
                return Ok("User authenticated");
            }
            if(result.IsLockedOut)
            {
                return StatusCode(423, "User is locked out");
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

            var user = Activator.CreateInstance<ApplicationUser>();

            user.Email = input.Email;
            user.UserName = input.UserName;
            user.FirstName = input.FirstName;
            user.LastName = input.LastName;
            user.PhoneNumber = input.PhoneNumber;

            await _userStore.UpdateAsync(user, CancellationToken.None);

            await _userStore.SetUserNameAsync(user, user.UserName, CancellationToken.None);
            await _userEmailStore.SetEmailAsync(user, user.Email, CancellationToken.None);
            var result = await _userManager.CreateAsync(user, input.Password);

            if (result.Succeeded)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                //přidat potvrzení mailu protože to není potřeba řešit
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmResult = await _userManager.ConfirmEmailAsync(user, code);
                return Ok("User created");
            }
            else
            {
                return BadRequest(result.Errors);
            }

        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

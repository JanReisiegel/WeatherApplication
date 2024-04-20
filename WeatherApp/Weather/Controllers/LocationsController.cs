using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Weather.Models;
using Weather.Services;

namespace Weather.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly LocationServices _locationServices;
        private readonly UserManager<ApplicationUser> _userManager;

        public LocationsController(UserManager<ApplicationUser> userManager)
        {
            _locationServices = new LocationServices();
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult GetLocation([FromQuery]string cityName)
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            if(user == null)
            {
                return Ok(_locationServices.GetLocation(cityName));
            }
            var location = _locationServices.GetLocation(cityName, user);
            if (location == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Cannot store data in database, please contact admin");
            }
            return Ok(location);
        }

        [Authorize]
        [HttpGet("all")]
        public IActionResult GetLocations()
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            var location = _locationServices.GetAllLocations(user);
            if (location == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Cannot store data in database, please contact admin");
            }
            return Ok(location);
        }
        [Authorize]
        [HttpPost]
        public  IActionResult SaveLocation([FromQuery]string cityName, [FromQuery] string customName)
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            Location location = _locationServices.StoreLocation(cityName, customName, user).Result;
            return Ok(location);
        }
    }
}

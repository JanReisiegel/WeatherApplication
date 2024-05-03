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
        private readonly LocationServices _locationServices = new LocationServices();

        [HttpGet]
        public async Task<IActionResult> GetLocation([FromHeader]string userToken, [FromQuery]string cityName)
        {
            var user = await JsonFileService.GetUserAsync(UserServices.GetClaims(userToken)["email"]);
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
        public async Task<IActionResult> GetLocations([FromHeader] string userToken)
        {
            var user = await JsonFileService.GetUserAsync(UserServices.GetClaims(userToken)["email"]);
            var location = _locationServices.GetAllLocations(user);
            if (location == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Cannot store data in database, please contact admin");
            }
            return Ok(location);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SaveLocation([FromHeader]string userToken, [FromQuery]string cityName, [FromQuery] string customName)
        {
            var user = await JsonFileService.GetUserAsync(UserServices.GetClaims(userToken)["email"]);
            Location location = _locationServices.StoreLocation(cityName, customName, user).Result;
            return Ok(location);
        }
    }
}

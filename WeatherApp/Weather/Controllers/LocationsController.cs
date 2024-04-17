using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using Weather.Data;
using Weather.Models;
using Weather.Services;

namespace Weather.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly LocationServices _locationServices;
        private readonly UserManager<ApplicationUser> _userManager;

        public LocationsController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _locationServices = new LocationServices(context);
            _userManager = userManager;
        }

        [HttpPost]
        public  IActionResult SaveLocation([FromQuery] double latitude, [FromQuery] double longitude)
        {
            Location location = _context.Locations.Where(x => x.Latitude == latitude && x.Longitude == longitude).FirstOrDefault() ?? _locationServices.StoreLocation(latitude, longitude);
            if (location == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Cannot store data in database, please contact admin");
            }
            var user = _userManager.GetUserAsync(HttpContext.User).Result;

            SavedLocation savedLocation = new SavedLocation
            {
                Location = location,
                LocationId = location.Id,
                User = user,
                UserId = user.Id,
            };
            _context.SavedLocations.Add(savedLocation);
            _context.SaveChanges();
            return Ok(savedLocation);
        }
    }
}

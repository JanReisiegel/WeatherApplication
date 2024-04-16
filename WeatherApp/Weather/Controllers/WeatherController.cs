using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Weather.Services;

namespace Weather.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get([FromQuery] double latitude, [FromQuery] double longitude)
        {
            var weather = new WeatherServices().GetActualWeather(latitude, longitude);

            return Ok(weather);
        }
    }
}

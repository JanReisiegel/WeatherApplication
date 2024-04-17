using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Weather.Data;
using Weather.Services;

namespace Weather.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly WeatherServices _weatherServices;

        public WeatherController(AppDbContext context)
        {
            _context = context;
            _weatherServices = new WeatherServices(context);
        }

        [HttpGet]
        public IActionResult Get([FromQuery] double latitude, [FromQuery] double longitude)
        {
            var weather = _weatherServices.GetActualWeather(latitude, longitude);
            if (weather == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Cannot store data in database, please contact admin");
            }
            return Ok(weather);
        }
        [HttpGet("history")]
        public IActionResult GetHistory([FromQuery] double latitude, [FromQuery] double longitude, string from, string to)
        {
            var weather = _weatherServices.GetWeatherHistoryAsync(latitude, longitude, DateTime.Parse(from), DateTime.Parse(to));
            if (weather == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Cannot store data in database, please contact admin");
            }
            return Ok(weather);
        }
        [HttpGet("forecast")]
        public IActionResult GetForecast([FromQuery] double latitude, [FromQuery] double longitude)
        {
            var weather = _weatherServices.GetWeatherForecast5Days(latitude, longitude);
            if (weather == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Cannot store data in database, please contact admin");
            }
            return Ok(weather);
        }
    }
}

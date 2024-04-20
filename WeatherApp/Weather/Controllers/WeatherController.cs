using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Weather.Services;

namespace Weather.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherServices _weatherServices;

        public WeatherController()
        {
            _weatherServices = new WeatherServices();
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string cityName)
        {
            var weather = _weatherServices.GetActualWeather(cityName);
            if (weather == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Cannot store data in database, please contact admin");
            }
            return Ok(weather);
        }
        [HttpGet("forecast")]
        public IActionResult GetForecast([FromQuery] string cityName)
        {
            var weather = _weatherServices.GetWeatherForecast5Days(cityName);
            if (weather == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Cannot store data in database, please contact admin");
            }
            return Ok(weather);
        }
    }
}

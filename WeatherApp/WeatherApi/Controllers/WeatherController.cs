using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeatherApi.Services;

namespace WeatherApi.Controllers
{
    [Route("/")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherService _weatherService;
        public WeatherController()
        {
            _weatherService = new WeatherService();
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello World");
        }

        [HttpGet("weather")]
        public IActionResult GetWeather([FromQuery]double latitude, [FromQuery]double longitude)
        {
            var rweather = _weatherService.GetWeather(latitude, longitude);
            return Ok(rweather);
        }

        [HttpGet("weather")]
        public IActionResult GetWeather([FromQuery]string location)
        {
            var rweather = _weatherService.GetWeather(location);
            return Ok(rweather);
        }
    }
}

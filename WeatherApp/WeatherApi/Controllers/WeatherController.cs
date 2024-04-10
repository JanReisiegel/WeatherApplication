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
        public IActionResult GetWeather([FromQuery] string address)
        {
            var rweather = _weatherService.GetWeather(address);
            return Ok(rweather);
        }
    }
}

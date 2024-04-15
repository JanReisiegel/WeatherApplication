using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeatherApi.Data;
using WeatherApi.Models;
using WeatherApi.Services;

namespace WeatherApi.Controllers
{
    [Route("/")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherService _weatherService;
        private readonly AppDbContext _context;
        public WeatherController(AppDbContext context)
        {
            _context = context;
            _weatherService = new WeatherService(context);
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello World");
        }

        [HttpGet("Actual/ByCoordinates")]
        public IActionResult GetWeather([FromQuery]double latitude, [FromQuery]double longitude)
        {
            var rweather = _weatherService.GetActualWeather(latitude, longitude);
            return Ok(rweather);
        }
        [HttpGet("Actual/ByCityName")]
        public IActionResult GetWeather([FromQuery]string CityName)
        {
            var coordinates = LocationTransformation.CityNameToCoordinates(CityName).Result;
            var rweather = _weatherService.GetActualWeather(coordinates[0], coordinates[1]);
            return Ok(rweather);
        }

        [HttpGet("Forecast")]
        public ActionResult<MyWeatherForecast> GetWeatherForcast([FromQuery]double latitude, [FromQuery]double longitude)
        {   
            MyWeatherForecast rweather = _weatherService.GetWeatherForcast5Days(latitude, longitude).Result;
            return Ok(rweather);
        }
    }
}

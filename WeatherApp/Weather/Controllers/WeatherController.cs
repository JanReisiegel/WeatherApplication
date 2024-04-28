using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Weather.Models;
using Weather.Services;
using Weather.ViewModels;

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

        [HttpGet("actual")]
        public async Task<ActionResult<MyWeatherInfo>> GetActualWeather([FromQuery] string cityName)
        {
            var weather = await _weatherServices.GetActualWeather(cityName);
            if (weather == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Cannot store data in database, please contact admin");
            }
            return Ok(weather);
        }
        [HttpGet("forecast")]
        public async Task<ActionResult<MyWeatherForecast>> GetForecast([FromQuery] string cityName)
        {
            var weather = await _weatherServices.GetWeatherForecast5Days(cityName);
            if (weather == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Cannot store data in database, please contact admin");
            }
            return Ok(weather);
        }
    }
}

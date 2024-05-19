using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Weather.Models;
using Weather.MyExceptions;
using Weather.Services;
using Weather.ViewModels;

namespace Weather.Controllers
{
    [Route("api/Weather")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherServices _weatherServices = new WeatherServices();
        private readonly LocationTransformation _locationTransformation = new LocationTransformation();

        [HttpGet("actual")]
        public async Task<ActionResult<MyWeatherInfo>> GetActualWeather([FromQuery] string cityName, [FromQuery]string country)
        {
            MyWeatherInfo weather;
            try { weather = await _weatherServices.GetActualWeather(cityName, country); }
            catch (LocationException e) { return NotFound(e.Message); }
            catch (Exception e) { return StatusCode(StatusCodes.Status500InternalServerError, e.Message); }
            if (weather == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Cannot store data in database, please contact admin");
            }
            return Ok(weather);
        }
        [HttpGet("forecast")]
        public async Task<ActionResult<MyWeatherForecast>> GetForecast([FromQuery] string cityName, [FromQuery] string country)
        {
            MyWeatherForecast weather;
            try
            {
                weather = await _weatherServices.GetWeatherForecast5Days(cityName, country);
            }
            catch (LocationException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            
            if (weather == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Cannot store data in database, please contact admin");
            }
            return Ok(weather);
        }

        [HttpGet("history")]
        public async Task<ActionResult<MyWeatherForecast>> GetHistory([FromHeader]string userToken, [FromQuery]string cityName, [FromQuery]string country)
        {
            if (userToken == null || !(await UserServices.GetAuthenticate(UserServices.GetClaims(userToken)["email"])))
            {
                return Unauthorized("You must be logged in");
            }
            Location location; 
            try
            {
                location = await _locationTransformation.GetCoordinates(cityName, country);
            } 
            catch (LocationException e)
            {
                return NotFound(e.Message);
            }
            if (location == null)
            {
                return NotFound("Location not found");
            }
            HistoryWeather history;
            try
            {
                history = await _weatherServices.GetWeatherHistory(location);
            }
            catch (HistoryException e)
            {
                return BadRequest(e.Message);
            }
            return Ok(history);
        }
    }
}

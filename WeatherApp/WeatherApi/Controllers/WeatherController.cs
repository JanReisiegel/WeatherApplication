using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WeatherApi.Controllers
{
    [Route("/")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello World");
        }
    }
}

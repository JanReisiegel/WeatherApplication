using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Weather.Controllers;

namespace Test
{
    [TestClass]
    public class WeatherTests
    {
        private readonly WeatherController _controller;
        public WeatherTests()
        {
            _controller = new WeatherController();
        }

        [TestMethod]
        public void TestGetActualWeather()
        {
            var response = _controller.GetActualWeather("London").Result.Result as ObjectResult;
            //var result = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(StatusCodes.Status200OK, response.StatusCode);
        }
        [TestMethod]
        public void TestGetActualWeatherNotFound()
        {
            var response = _controller.GetActualWeather("AGUGUDG").Result.Result as ObjectResult;
            //var result = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(StatusCodes.Status404NotFound, response.StatusCode);
        }
        [TestMethod]
        public void TestGetForecastNotFound()
        {
            var response = _controller.GetForecast("AGUGUDG").Result.Result as ObjectResult;
            //var result = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(StatusCodes.Status404NotFound, response.StatusCode);
        }
        [TestMethod]
        public void TestGetForecastWeather()
        {
            var response = _controller.GetForecast("Praga").Result.Result as ObjectResult; //Nevím proè nefunguje London
            //var result = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(StatusCodes.Status200OK, response.StatusCode);
        }
    }
}
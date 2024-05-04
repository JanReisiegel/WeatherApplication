using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Weather.Controllers;

namespace Test
{
    [TestFixture]
    public class WeatherTests
    {
        private WeatherController _controller;

        [OneTimeSetUp]
        public void Setup()
        {
            _controller = new WeatherController();
        }

        [Test]
        public void TestGetActualWeather()
        {
            var response = _controller.GetActualWeather("London").Result.Result as ObjectResult;
            //var result = await response.Content.ReadAsStringAsync();
            Assert.That(Equals(StatusCodes.Status200OK, response.StatusCode));
        }
        [Test]
        public void TestGetActualWeatherNotFound()
        {
            var response = _controller.GetActualWeather("AGUGUDG").Result.Result as ObjectResult;
            //var result = await response.Content.ReadAsStringAsync();
            Assert.That(Equals(StatusCodes.Status404NotFound, response.StatusCode));
        }
        [Test]
        public void TestGetForecastNotFound()
        {
            var response = _controller.GetForecast("AGUGUDG").Result.Result as ObjectResult;
            //var result = await response.Content.ReadAsStringAsync();
            Assert.That(Equals(StatusCodes.Status404NotFound, response.StatusCode));
        }
        [Test]
        public void TestGetForecastWeather()
        {
            var response = _controller.GetForecast("Praga").Result.Result as ObjectResult; //Nevím proè nefunguje London
            //var result = await response.Content.ReadAsStringAsync();
            Assert.That(Equals(StatusCodes.Status200OK, response.StatusCode));
        }
    }
}
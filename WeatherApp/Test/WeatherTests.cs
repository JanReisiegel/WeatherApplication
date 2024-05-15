using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Weather.Controllers;
using Weather.ViewModels;

namespace Test
{
    [TestFixture]
    public class WeatherTests
    {
        private WeatherController _controller;
        private UsersController _usersController;
        private LoginModel _loginModel = new LoginModel
        {
            Email = "test@user2.com",
            Password = "Test1234!"
        };

        [OneTimeSetUp]
        public void Setup()
        {
            _controller = new WeatherController();
            _usersController = new UsersController();
            var result = _usersController.Register(new RegisterModel
            {
                Email = _loginModel.Email,
                Password = _loginModel.Password,
                PhoneNumber = "123456789",
                UserName = "TestUser2"
            }).Result;
            var login = _usersController.Login(_loginModel).Result as ObjectResult;
            var token = tokenParse(login.Value.ToString());
        }

        [Test]
        public void TestGetActualWeather()
        {
            var response = _controller.GetActualWeather("London", "United Kingdom").Result.Result as ObjectResult;
            //var result = await response.Content.ReadAsStringAsync();
            Assert.That(Equals(StatusCodes.Status200OK, response.StatusCode));
        }
        [Test]
        public void TestGetActualWeatherNotFound()
        {
            var response = _controller.GetActualWeather("AGUGUDG", "France").Result.Result as ObjectResult;
            //var result = await response.Content.ReadAsStringAsync();
            Assert.That(Equals(StatusCodes.Status404NotFound, response.StatusCode));
        }
        [Test]
        public void TestGetForecastNotFound()
        {
            var response = _controller.GetForecast("AGUGUDG", "Japan").Result.Result as ObjectResult;
            //var result = await response.Content.ReadAsStringAsync();
            Assert.That(Equals(StatusCodes.Status404NotFound, response.StatusCode));
        }
        [Test]
        public void TestGetForecastWeather()
        {
            var response = _controller.GetForecast("Praga", "Czechia").Result.Result as ObjectResult; //Nevím proè nefunguje London
            //var result = await response.Content.ReadAsStringAsync();
            Assert.That(Equals(StatusCodes.Status200OK, response.StatusCode));
        }

        [Test]
        public void TestGetHistoryOK()
        {
            var login = _usersController.Login(_loginModel).Result as ObjectResult;
            var token = tokenParse(login.Value.ToString());
            var response = _controller.GetHistory(token, "London", "United Kingdom", DateTime.Now.AddYears(-1).ToString()).Result.Result as ObjectResult;
            Assert.That(Equals(StatusCodes.Status200OK, response.StatusCode));
        }

        [Test]
        public void TestGetHistoryUnauthorized()
        {
            var response = _controller.GetHistory(null, "London", "United Kingdom", DateTime.Now.AddYears(-1).ToString()).Result.Result as ObjectResult;
            Assert.That(Equals(StatusCodes.Status401Unauthorized, response.StatusCode));
        }

        [Test]
        public void TestGetHistoryNotFound()
        {
            var login = _usersController.Login(_loginModel).Result as ObjectResult;
            var token = tokenParse(login.Value.ToString());
            var response = _controller.GetHistory(token, "AGUGUDG", "France", DateTime.Now.AddYears(-1).ToString()).Result.Result as ObjectResult;
            Assert.That(Equals(StatusCodes.Status404NotFound, response.StatusCode));
        }

        [Test]
        public void TestGetHistoryBadDate()
        {
            var login = _usersController.Login(_loginModel).Result as ObjectResult;
            var token = tokenParse(login.Value.ToString());
            var response = _controller.GetHistory(token, "London", "United Kingdom", DateTime.Now.AddYears(-20).ToString()).Result.Result as ObjectResult;
            Assert.That(Equals(StatusCodes.Status400BadRequest, response.StatusCode));
            Assert.That(Equals("Date must be after 1st Jan 2010", response.Value));
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            var login = _usersController.Login(_loginModel).Result as ObjectResult;
            var token = tokenParse(login.Value.ToString());
            var result = _usersController.DeleteUser(token, _loginModel).Result;
        }

        private string tokenParse(string token)
        {
            List<string> tokenList = token.Replace(" ", "").Replace("{", "").Replace("}", "").Split("=").ToList();
            return tokenList.Last();
        }
    }
}
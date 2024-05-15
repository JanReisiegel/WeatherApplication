using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weather.Controllers;
using Weather.Models;
using Weather.Services;
using Weather.ViewModels;

namespace Test
{
    [TestFixture]
    public class LocationsTests
    {
        private LocationsController _controller;
        private UsersController _usersController;
        private LoginModel _loginModel = new LoginModel
        {
            Email = "test@user2.com",
            Password = "Test1234!"
        };
        private LocationTransformation _locationTransformation = new LocationTransformation();

        [OneTimeSetUp]
        public void Setup()
        {
            _controller = new LocationsController();
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
            var locationResult = _controller.SaveLocation(token, "Praha", "Czech Republic","Prazička").Result;
        }

        [Test]
        [Order(1)]
        public async Task TestGetLocationOK()
        {
            var login = _usersController.Login(_loginModel).Result as ObjectResult;
            TestContext.WriteLine(login.Value);
            var token = tokenParse(login.Value.ToString());
            TestContext.WriteLine(token);
            var result = await _controller.GetSavedLocation(token, "Prazička") as ObjectResult;
            TestContext.WriteLine(result.Value);
            Assert.That(Equals(StatusCodes.Status200OK, result.StatusCode));
        }
        [Test]
        [Order(2)]
        public async Task TestGetLocationUnauthorized()
        {
            var result = await _controller.GetSavedLocation(null, "Praga") as ObjectResult;
            Assert.That(Equals(StatusCodes.Status401Unauthorized, result.StatusCode));
        }
        [Test]
        [Order(3)]
        public async Task TestGetLocationNotFound()
        {
            var login = _usersController.Login(_loginModel).Result as ObjectResult;
            var token = tokenParse(login.Value.ToString());
            var result = await _controller.GetSavedLocation(token, "NewYork") as ObjectResult;
            TestContext.WriteLine(result.StatusCode);
            Assert.That(Equals(StatusCodes.Status204NoContent, result.StatusCode));
        }

        [Test]
        [Order(4)]
        public async Task TestSaveLocationUnauthorized()
        {
            var result = await _controller.SaveLocation(null, "Brno","Czech Republic", "Prazička") as ObjectResult;
            Assert.That(Equals(StatusCodes.Status401Unauthorized, result.StatusCode));
        }

        [Test]
        [Order(5)]
        public async Task TestSaveLocationOK()
        {
            var login = _usersController.Login(_loginModel).Result as ObjectResult;
            var token = tokenParse(login.Value.ToString());
            var result = await _controller.SaveLocation(token, "London","United Kingdom", "Londýnek") as ObjectResult;
            Assert.That(Equals(StatusCodes.Status200OK, result.StatusCode));
        }

        [Test]
        [Order(6)]
        public async Task TestGetAllLocationsUnauthorized()
        {
            var result = await _controller.GetSavedLocations(null) as ObjectResult;
            Assert.That(Equals(StatusCodes.Status401Unauthorized, result.StatusCode));
        }

        [Test]
        [Order(7)]
        public async Task TestGetAllLocationsOK()
        {
            var login = _usersController.Login(_loginModel).Result as ObjectResult;
            var token = tokenParse(login.Value.ToString());
            var result = await _controller.GetSavedLocations(token) as ObjectResult;
            Assert.That(Equals(StatusCodes.Status200OK, result.StatusCode));
        }

        [Test]
        [Order(8)]
        public async Task TestDeleteLocationUnauthorized()
        {
            var result = await _controller.DeleteLocation(null, "Nový Jork") as ObjectResult;
            Assert.That(Equals(StatusCodes.Status401Unauthorized, result.StatusCode));
        }

        [Test]
        [Order(9)]
        public async Task TestDeleteLocationNotFound()
        {
            var login = _usersController.Login(_loginModel).Result as ObjectResult;
            var token = tokenParse(login.Value.ToString());
            TestContext.WriteLine(token);
            var result = await _controller.DeleteLocation(token, "Brníčko") as ObjectResult;
            Assert.That(Equals(StatusCodes.Status404NotFound, result.StatusCode));
        }

        [Test]
        [Order(10)]
        public async Task TestDeleteLocationOK()
        {
            var login = _usersController.Login(_loginModel).Result as ObjectResult;
            var token = tokenParse(login.Value.ToString());
            var result = await _controller.DeleteLocation(token, "Prazička") as ObjectResult;
            TestContext.WriteLine(token);
            Assert.That(Equals(StatusCodes.Status200OK, result.StatusCode));
        }

        [Test]
        [Order(11)]
        public async Task TestGetCodeBritain()
        {
            var result = _locationTransformation.GetCountryCode("United Kingdom");
            Assert.That(Equals(result, "gb"));
        }
        [Test]
        [Order(12)]
        public async Task TestGetCodeChina()
        {
            var result = _locationTransformation.GetCountryCode("China");
            Assert.That(Equals(result, "cn"));
        }
        [Test]
        [Order(13)]
        public async Task TestGetCodeDenmark()
        {
            var result = _locationTransformation.GetCountryCode("Denmark");
            Assert.That(Equals(result, "dk"));
        }
        [Test]
        [Order(14)]
        public async Task TestGetCodeAustralia()
        {
            var result = _locationTransformation.GetCountryCode("Australia");
            Assert.That(Equals(result, "au"));
        }
        [Test]
        [Order(15)]
        public async Task TestGetCodeFinland()
        {
            var result = _locationTransformation.GetCountryCode("Finland");
            Assert.That(Equals(result, "fi"));
        }
        [Test]
        [Order(16)]
        public async Task TestGetCodeFrance()
        {
            var result = _locationTransformation.GetCountryCode("France");
            Assert.That(Equals(result, "fr"));
        }
        [Test]
        [Order(17)]
        public async Task TestGetCodeMorocco()
        {
            var result = _locationTransformation.GetCountryCode("Morocco");
            Assert.That(Equals(result, "ma"));
        }
        [Test]
        [Order(18)]
        public async Task TestGetCodeNetherlands()
        {
            var result = _locationTransformation.GetCountryCode("Netherlands");
            Assert.That(Equals(result, "nl"));
        }
        [Test]
        [Order(19)]
        public async Task TestGetCodeZealand()
        {
            var result = _locationTransformation.GetCountryCode("New Zealand");
            Assert.That(Equals(result, "nz"));
        }
        [Test]
        [Order(20)]
        public async Task TestGetCodeNorway()
        {
            var result = _locationTransformation.GetCountryCode("Norway");
            Assert.That(Equals(result, "no"));
        }
        [Test]
        [Order(21)]
        public async Task TestGetCodeUSA()
        {
            var result = _locationTransformation.GetCountryCode("United States");
            Assert.That(Equals(result, "us"));
        }
        [Test]
        [Order(22)]
        public async Task TestGetCodeCzechRepublic()
        {
            var result = _locationTransformation.GetCountryCode("Czech Republic");
            Assert.That(Equals(result, "Czech Republic"));
        }

        [Test]
        [Order(23)]
        public async Task TestGetLocationFromCoordsOK()
        {
            var result = await _controller.GetLocation(50.0755, 14.4378) as ObjectResult;
            Assert.That(Equals((result.Value as Location).CityName, "Prague"));
            Assert.That(Equals((result.Value as Location).Country, "Czechia"));
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

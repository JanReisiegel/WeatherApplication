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
            var locationResult = _controller.SaveLocation(token, "Praha", "Prazička").Result;
        }

        [Test]
        [Order(1)]
        public async Task TestGetLocationOK()
        {
            var login = _usersController.Login(_loginModel).Result as ObjectResult;
            TestContext.WriteLine(login.Value);
            var token = tokenParse(login.Value.ToString());
            TestContext.WriteLine(token);
            var result = await _controller.GetSavedLocation(token, "Praha") as ObjectResult;
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
            TestContext.WriteLine(result.Value);
            Assert.That(Equals(StatusCodes.Status200OK, result.StatusCode));
        }

        [Test]
        [Order(4)]
        public async Task TestSaveLocationUnauthorized()
        {
            var result = await _controller.SaveLocation(null, "Brno", "Prazička") as ObjectResult;
            Assert.That(Equals(StatusCodes.Status401Unauthorized, result.StatusCode));
        }

        [Test]
        [Order(5)]
        public async Task TestSaveLocationOK()
        {
            var login = _usersController.Login(_loginModel).Result as ObjectResult;
            var token = tokenParse(login.Value.ToString());
            var result = await _controller.SaveLocation(token, "London", "Londýnek") as ObjectResult;
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
            Assert.That(Equals(StatusCodes.Status404NotFound, result.StatusCode));
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

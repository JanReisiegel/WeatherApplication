using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Weather.Controllers;
using Weather.Models;
using Weather.ViewModels;
using Xunit;

namespace Test
{
    [TestClass]
    public class UsersTests: IClassFixture<ApiWebApplicationFactory>
    {
        private static HttpClient _client;
        private static Mock<UserManager<ApplicationUser>> _userManagerMock;
        private static UsersController _controller;
        public string Token { get; set; }

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                new LoggerFactory().CreateLogger<UserManager<ApplicationUser>>()
                );

            _controller = new UsersController(_userManagerMock.Object);

            //var factory = new ApiWebApplicationFactory();
            //_client = factory.CreateClient();
        }

        [TestMethod]
        public void TestGetAllUsers()
        {
            //var response = _client.GetAsync("/api/Users/all").Result;
            var response = _controller.GetAllUsers().Result as ObjectResult;
            Assert.AreEqual(StatusCodes.Status200OK, response.StatusCode);
        }

        [TestMethod]
        public void TestRegister()
        {
            var response = _controller.Register(new RegisterModel
            {
                Email = "tes@user.jedna",
                Password = "password",
                PhoneNumber = "123456789",
                UserName = "testuser1"
            }).Result as ObjectResult;
            Assert.AreEqual(StatusCodes.Status200OK, response.StatusCode);
        }
        [TestMethod]
        public void TestLogin()
        {
            var response = _controller.Login(new LoginModel
            {
                Email = "test@user.jedna",
                Password = "password"
            }) as ObjectResult;
            Assert.AreEqual(StatusCodes.Status200OK, response.StatusCode);
            Token = response.Value.ToString();
            
            Assert.IsNotNull(Token);
        }

        [TestMethod]
        public void TestUpdateUser()
        {
            var response = _controller.UpdateUser(new UserVM
            {
                UserName = "testuser1",
                Email = "test@user.jedna",
                PhoneNumber = "123456789",
                Password = "password",
                PaidAccount = true,
            }).Result as ObjectResult;
            Assert.AreEqual(StatusCodes.Status200OK, response.StatusCode);
        }

        [TestMethod]
        public void TestDeleteUser()
        {
            var response = _controller.DeleteUser().Result as ObjectResult;
            Assert.AreEqual(StatusCodes.Status200OK, response.StatusCode);
        }

        /*[ClassCleanup]
        public static void Cleanup()
        {
            _client?.Dispose();
        }*/
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Weather.Controllers;
using Weather.ViewModels;

namespace Test
{
    [TestFixture]
    [Order(1)]
    public class UsersTests
    {
        private UsersController _controller;
        private readonly RegisterModel User = new RegisterModel
        {
            Email = "test@user.cz",
            Password = "Test1234",
            PhoneNumber = "123456789",
            UserName = "TestUser"
        };

        [OneTimeSetUp]
        public void Setup()
        {
            _controller = new UsersController();
        }

        [Test]
        [Order(1)]
        public async Task TestGetAllUsersNullToken()
        {
            var response = await _controller.GetAllUsers(null) as ObjectResult;
            Assert.That(Equals(StatusCodes.Status401Unauthorized, response.StatusCode));
        }

        [Test]
        [Order(2)]
        public async Task TestRegisterUserOK()
        {
            var user = new RegisterModel
            {
                Email = User.Email,
                Password = User.Password,
                PhoneNumber = User.PhoneNumber,
                UserName = User.UserName
            };
            var response = await _controller.Register(user) as ObjectResult;
            Console.WriteLine(response.Value);
            Assert.That(Equals(StatusCodes.Status200OK, response.StatusCode));
        }
        [Test]
        [Order(3)]
        public async Task TestGetAllUsersOK()
        {
            var userLogin = new LoginModel
            {
                Email = User.Email,
                Password = User.Password
            };
            var tokenResponse = await _controller.Login(userLogin) as ObjectResult;
            string token = tokenParse(tokenResponse.Value.ToString());
            var response = await _controller.GetAllUsers(token) as ObjectResult;
            Assert.That(Equals(StatusCodes.Status401Unauthorized, response.StatusCode));
        }
        [Test]
        [Order(4)]
        public async Task TestRegisterUserRegistered()
        {
            var user = new RegisterModel
            {
                Email = User.Email,
                Password = User.Password,
                PhoneNumber = User.PhoneNumber,
                UserName = User.UserName
            };
            var response = await _controller.Register(user) as ObjectResult;
            Assert.That(Equals(StatusCodes.Status400BadRequest, response.StatusCode));
            Assert.That(Equals("User already exists", response.Value));
        }
        [Test]
        [Order(5)]
        public async Task TestLoginUserNotFoundUser()
        {
            var user = new LoginModel
            {
                Email = "twst@user.cz",
                Password = "Test1234"
            };
            var response = await _controller.Login(user) as ObjectResult;
            Assert.That(Equals(StatusCodes.Status401Unauthorized, response.StatusCode));
            Assert.That(Equals("User not found", response.Value));
        }
        [Test]
        [Order(6)]
        public async Task TestLoginUserBadPassword()
        {
            var user = new LoginModel
            {
                Email = "test@user.cz",
                Password = "Test12345"
            };
            var response = await _controller.Login(user) as ObjectResult;
            Assert.That(Equals(StatusCodes.Status401Unauthorized, response.StatusCode));
            Assert.That(Equals("Invalid Password", response.Value));
        }

        [Test]
        [Order(7)]
        public async Task TestLoginUserOK()
        {
            var user = new LoginModel
            {
                Email = "test@user.cz",
                Password = "Test1234"
            };
            var response = await _controller.Login(user) as ObjectResult;
            Assert.That(Equals(StatusCodes.Status200OK, response.StatusCode));
        }

        [Test]
        [Order(8)]
        public async Task TestGetUserInfo()
        {
            var userLogin = new LoginModel
            {
                Email = User.Email,
                Password = User.Password
            };
            var tokenResponse = await _controller.Login(userLogin) as ObjectResult;
            string token = tokenParse(tokenResponse.Value.ToString());
            var response = await _controller.GetUserInfo(token) as ObjectResult;
            Assert.That(Equals(StatusCodes.Status200OK, response.StatusCode));
            
        }

        [Test]
        [Order(9)]
        public async Task TestUpdateUserUnauthorized()
        {
            var userLogin = new LoginModel
            {
                Email = User.Email,
                Password = User.Password
            };
            var tokenResponse = await _controller.Login(userLogin) as ObjectResult;
            string token = tokenParse(tokenResponse.Value.ToString());
            var userResponse = await _controller.GetUserInfo(token) as ObjectResult;
            UserVM user = userResponse.Value as UserVM;
            user.Email = "bad@email.com";
            user.Password = "Test1234";
            user.PhoneNumber = "987654321";
            var response = await _controller.UpdateUser(token, user) as ObjectResult;
            TestContext.WriteLine(response.Value);
            Assert.That(Equals(StatusCodes.Status401Unauthorized, response.StatusCode));
            Assert.That(Equals("You can only update your own account", response.Value));
        }

        [Test]
        [Order(10)]
        public async Task TestUpdateUserOK()
        {
            var userLogin = new LoginModel
            {
                Email = User.Email,
                Password = User.Password
            };
            var tokenResponse = await _controller.Login(userLogin) as ObjectResult;
            string token = tokenParse(tokenResponse.Value.ToString());
            var userResponse = await _controller.GetUserInfo(token) as ObjectResult;
            UserVM user = userResponse.Value as UserVM;
            user.Password = "Test1234";
            user.PhoneNumber = "987654321";
            var response = await _controller.UpdateUser(token, user) as ObjectResult;
            Assert.That(Equals(StatusCodes.Status200OK, response.StatusCode));
        }

        [Test]
        [Order(11)]
        public async Task TestUpdateUserNullToken()
        {
            UserVM user = new UserVM();
            user.Password = "Test1234";
            user.PhoneNumber = "987654321";
            var response = await _controller.UpdateUser(null, user) as ObjectResult;
            Assert.That(Equals(StatusCodes.Status401Unauthorized, response.StatusCode));
        }

        [Test]
        [Order(12)]
        public async Task TestDeleteUserNotOwn()
        {
            var userLogin = new LoginModel
            {
                Email = User.Email,
                Password = User.Password
            };
            var tokenResponse = await _controller.Login(userLogin) as ObjectResult;
            string token = tokenParse(tokenResponse.Value.ToString());
            var response = await _controller.DeleteUser(token, new LoginModel { Email = "test2@user.cz", Password = "heslo123545" }) as ObjectResult;
            Assert.That(Equals(StatusCodes.Status401Unauthorized, response.StatusCode));
            Assert.That(Equals("You can only delete your own account", response.Value));
        }

        [Test]
        [Order(13)]
        public async Task TestDeleteUserBadPassword()
        {
            var userLogin = new LoginModel
            {
                Email = User.Email,
                Password = User.Password
            };
            var tokenResponse = await _controller.Login(userLogin) as ObjectResult;
            string token = tokenParse(tokenResponse.Value.ToString());
            var response = await _controller.DeleteUser(token, new LoginModel { Email = "test@user.cz", Password="spatneheslo123"}) as ObjectResult;
            Assert.That(Equals(StatusCodes.Status401Unauthorized, response.StatusCode));
            Assert.That(Equals("Invalid password", response.Value));
        }

        [Test]
        [Order(14)]
        public async Task TestDeleteUserOK()
        {
            var userLogin = new LoginModel
            {
                Email = User.Email,
                Password = User.Password
            };
            var tokenResponse = await _controller.Login(userLogin) as ObjectResult;
            string token = tokenParse(tokenResponse.Value.ToString());
            var response = await _controller.DeleteUser(token, new LoginModel { Email = "test@user.cz", Password = "Test1234" }) as ObjectResult;
            TestContext.WriteLine(response.Value);
            Assert.That(Equals(StatusCodes.Status200OK, response.StatusCode));           
        }

        private string tokenParse(string token)
        {
            return token.Replace("{ ", "").Replace(" }", "").Split(" = ")[1];
        }
    }
}

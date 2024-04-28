using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weather;

namespace TestWeather
{
    [TestClass]
    public class UsersTest
    {

        private readonly TestServer _testServer;
        private readonly HttpClient _client;
        public UsersTest()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Logging.ClearProviders();
            var app = builder.Build();
            _testServer = new TestServer(app.Services);
        }
        [TestMethod]
        public void TestRegister()
        {

        }
    }
}

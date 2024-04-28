using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestWeather
{
    [TestClass]
    public class WeatherTest
    {
        private readonly TestServer _testServer;
        private readonly HttpClient _client;
        public WeatherTest()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Logging.ClearProviders();
            var app = builder.Build();
            _testServer = new TestServer(app.Services);
            _client = _testServer.CreateClient();
        }

        [TestMethod]
        public void TestGetActualWeather()
        {
            var response = _client.GetAsync("/api/weather/actual?cityName=London").Result;
            //var result = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}

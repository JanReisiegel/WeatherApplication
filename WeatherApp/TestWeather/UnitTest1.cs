using Microsoft.AspNetCore.Mvc;
using WeatherApi.Controllers;

namespace TestWeather
{
    [TestClass]
    public class UnitTest1
    {
        private readonly WeatherController weatherController = new WeatherController();
        [TestMethod]
        public void Test1()
        {
            var result = weatherController.Get() as ObjectResult;
            Assert.AreEqual("Hello World", result.Value);
        }
    }
}
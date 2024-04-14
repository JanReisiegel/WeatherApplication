using Newtonsoft.Json;
using OpenWeatherMap.Models;

namespace WeatherApi.Models
{
    public class MyWeatherForecast : WeatherForecastBase
    {
        [JsonProperty("list")]
        public ICollection<WeatherForecastItem> Items { get; set; } = new List<WeatherForecastItem>();
        public DateTime AcquiredDate { get; set; }

    }
}

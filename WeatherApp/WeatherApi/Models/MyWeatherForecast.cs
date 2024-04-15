using Newtonsoft.Json;
using OpenWeatherMap.Models;

namespace WeatherApi.Models
{
    public class MyWeatherForecast
    {
        public string CityName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public ICollection<WeatherForecastItem> Items { get; set; } = new List<WeatherForecastItem>();
        public DateTime AcquiredDate { get; set; }

    }
}

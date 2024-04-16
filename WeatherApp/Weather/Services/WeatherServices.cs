using OpenWeatherMap;
using OpenWeatherMap.Models;
using Weather.Models;

namespace Weather.Services
{
    public class WeatherServices
    {
        private OpenWeatherMapOptions openWeatherMapOptions = new OpenWeatherMapOptions
        {
            ApiKey = Constants.OpenWeatherMapApiKey,
            ApiEndpoint = "https://api.openweathermap.org/data/2.5/",
            Language = "cs",
            UnitSystem = "metric",
        };

        public WeatherInfo GetActualWeather(double latitude, double longitude)
        {
            var openWeatherMapService = new OpenWeatherMapService(openWeatherMapOptions);
            return openWeatherMapService.GetCurrentWeatherAsync(latitude, longitude).Result;
        }

        public WeatherForecast GetWeatherForecast5Days(double latitude, double longitude)
        {
            var openWeatherMapService = new OpenWeatherMapService(openWeatherMapOptions);
            return openWeatherMapService.GetWeatherForecast5Async(latitude, longitude).Result;
        }
    }
}

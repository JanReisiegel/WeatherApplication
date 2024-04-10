using OpenWeatherMap;
using OpenWeatherMap.Models;

namespace WeatherApi.Services
{
    public class WeatherService
    {
        private static OpenWeatherMapOptions openWeatherMapOptions = new OpenWeatherMapOptions
        {
            ApiKey = WeatherConstants.OpenWeatherMapOptions,
            ApiEndpoint = "https://api.openweathermap.org/data/2.5/",
            Language = "cs",
            UnitSystem = "metric"
        };
        private static IOpenWeatherMapService openWeatherMapService = new OpenWeatherMapService(openWeatherMapOptions);

        private readonly LocationFormatTransform _locationFormatTransform;

        public WeatherService()
        {
            _locationFormatTransform = new LocationFormatTransform();
        }

        public WeatherInfo GetWeather(string address)
        {
            var point = _locationFormatTransform.TransformAdressToPoint(address);
            return openWeatherMapService.GetCurrentWeatherAsync(point.Latitude, point.Longitude).Result;
        }
    }
}

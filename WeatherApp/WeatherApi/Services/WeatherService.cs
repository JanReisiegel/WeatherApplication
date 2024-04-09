using OpenWeatherMap;

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

        public WeatherService()
        {
        }

        public string GetWeather(string city)
        {
            var weather = openWeatherMapService.
        }
    }
}

using OpenWeatherMap;

namespace WeatherApi.Services
{
    public class WeatherService
    {
        private static OpenWeatherMapOptions openWeatherMapOptions = new OpenWeatherMapOptions
        {
            ApiKey = WeatherConstants.OpenWeatherMapOptions
        };
        private static IOpenWeatherMapService openWeatherMapService = new OpenWeatherMapService(openWeatherMapOptions);
    }
}

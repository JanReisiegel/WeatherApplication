using OpenWeatherMap;
using OpenWeatherMap.Models;
using WeatherApi.Data;

namespace WeatherApi.Services
{
    public class WeatherService
    {
        private readonly AppDbContext _context;

        public WeatherService(AppDbContext context)
        {
            _context = context;
        }

        private static OpenWeatherMapOptions openWeatherMapOptions = new OpenWeatherMapOptions
        {
            ApiKey = WeatherConstants.OpenWeatherMapOptions,
            ApiEndpoint = "https://api.openweathermap.org/data/2.5/",
            Language = "cs",
            UnitSystem = "metric",
            
        };
        private static IOpenWeatherMapService openWeatherMapService = new OpenWeatherMapService(openWeatherMapOptions);

        public WeatherInfo GetWeather(double latitude, double longitude)
        {
            return openWeatherMapService.GetCurrentWeatherAsync(latitude, longitude).Result;
        }
        public async Task<WeatherForecast> GetWeatherForcast5Days(double latitude, double longitude)
        {
            //WeatherForecast result = _context.WeatherForecasts.Where(x=> x.Latitude == latitude && x.Longitude == longitude).FirstOrDefault();
            
            var result = openWeatherMapService.GetWeatherForecast5Async(latitude,longitude).Result;
            return result;
        }
    }
}

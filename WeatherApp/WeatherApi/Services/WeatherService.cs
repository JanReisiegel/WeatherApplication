using OpenWeatherMap;
using OpenWeatherMap.Models;
using WeatherApi.Data;
using WeatherApi.Models;

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

        public async Task<WeatherInfo> GetActualWeather(double latitude, double longitude)
        {
            var weatherInfo = _context.WeatherInfos.Where(x=>x.Coordinates.Latitude == latitude && x.Coordinates.Longitude == longitude && IsTimeInTolerance(x.Date)).FirstOrDefault();
            if(weatherInfo != null)
            {
                return weatherInfo;
            }
            var result = openWeatherMapService.GetCurrentWeatherAsync(latitude, longitude).Result;
            _context.WeatherInfos.Add(result);
            await _context.SaveChangesAsync();
            return result;
        }
        private bool IsTimeInTolerance(DateTime date)
        {
            return date.AddHours(0.5) > DateTime.Now || date.AddHours(-0.5) > DateTime.Now;
        }
        public async Task<MyWeatherForecast> GetWeatherForcast5Days(double latitude, double longitude)
        {
            //var weatherForcast = _context.WeatherForecasts.Where(x => x.Latitude == latitude && x.Longitude == longitude && x.AcquiredDate.Date == DateTime.Now.Date).FirstOrDefault();
            /*if (weatherForcast != null)
            {
                return weatherForcast;
            }*/
            var result = openWeatherMapService.GetWeatherForecast5Async(latitude,longitude).Result;
            MyWeatherForecast myWeatherForcast = new MyWeatherForecast
            {
                CityName = result.City.Name,
                Latitude = result.City.Coordinates.Latitude,
                Longitude = result.City.Coordinates.Longitude,
                AcquiredDate = DateTime.Now
            };
            result.Items.ToList().ForEach(x => myWeatherForcast.Items.Add(x));
            //_context.WeatherForecasts.Add(myWeatherForcast);
            //await _context.SaveChangesAsync();
            return myWeatherForcast;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using OpenWeatherMap;
using OpenWeatherMap.Models;
using Weather.Data;
using Weather.Models;

namespace Weather.Services
{
    public class WeatherServices
    {
        private readonly AppDbContext _context;
        private readonly OpenWeatherMapService _openWeatherMapService;
        private readonly LocationTransformation _locationTransformation = new LocationTransformation();
        private readonly LocationServices _locationServices;

        public WeatherServices(AppDbContext context)
        {
            _context = context;
            _openWeatherMapService = new OpenWeatherMapService(openWeatherMapOptions);
            _locationServices = new LocationServices(context);
        }

        private OpenWeatherMapOptions openWeatherMapOptions = new OpenWeatherMapOptions
        {
            ApiKey = Constants.OpenWeatherMapApiKey,
            ApiEndpoint = "https://api.openweathermap.org/data/2.5/",
            Language = "cs",
            UnitSystem = "metric",
        };


        public async Task<MyWeatherInfo> GetActualWeather(double latitude, double longitude)
        {
            Location location = _context.Locations.Where(x => x.Latitude == latitude && x.Longitude == longitude).FirstOrDefault() ?? _locationServices.StoreLocation(latitude, longitude);
            
            var resultWeather = _context.MyWeatherInfos.Include(x=>x.Location).Where(x=>x.Location == location && x.AcquireDateTime>=DateTime.Now.AddHours(-1)).FirstOrDefault();
            if(resultWeather != null)
            {
                return resultWeather;
            }
            var weather = _openWeatherMapService.GetCurrentWeatherAsync(latitude, longitude).Result;
            MyWeatherInfo myWeather = new MyWeatherInfo
            {
                Temperature = weather.Main.Temperature.DegreesCelsius,
                FeelsTemperature = weather.Main.FeelsLike.DegreesCelsius,
                Pressure = weather.Main.Pressure.Hectopascals,
                Humidity = weather.Main.Humidity.Value,
                WindSpeed = weather.Wind.Speed.MetersPerSecond,
                Directory = weather.Wind.Direction.Value,
                CloudsNow = weather.Clouds.All.Value,
                Sunrise = weather.AdditionalInformation.Sunrise,
                Sunset = weather.AdditionalInformation.Sunset,
                Condition = (ConditionGroup)weather.Weather[0].Main,
                AcquireDateTime = DateTime.Now,
                Location = location,
                LocationId = location.Id,
            };
            _context.MyWeatherInfos.Add(myWeather);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                return null;
            }
            return myWeather;
        }

        public MyWeatherForecast GetWeatherForecast5Days(double latitude, double longitude)
        {
            Location location = _context.Locations.Where(x => x.Latitude == latitude && x.Longitude == longitude).FirstOrDefault() ?? _locationServices.StoreLocation(latitude, longitude);
            var result = _context.MyWeatherForecasts.Include(x=>x.Location).Where(x=>x.Location == location && x.AcquireDateTime>=DateTime.Now.AddHours(-1)).FirstOrDefault();
            if(result != null)
            {
                return result;
            }
            var weatherForecast =  _openWeatherMapService.GetWeatherForecast5Async(latitude, longitude).Result;
            MyWeatherForecast myWeatherForecast = new MyWeatherForecast
            {
                AcquireDateTime = DateTime.Now,
                Location = location,
                LocationId = location.Id,
                ForecastItems = new List<MyForecastItem>(),
            };
            throw new NotImplementedException();
        }

        
    }
}

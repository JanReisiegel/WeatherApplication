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

        public WeatherServices(AppDbContext context)
        {
            _context = context;
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
            var resultWeather = _context.MyWeatherInfos.Include(x=>x.Location).Where(x=>x.Location.Latitude == latitude && x.Location.Longitude == longitude && x.AcquireDateTime>=DateTime.Now.AddHours(-1)).FirstOrDefault();
            if(resultWeather != null)
            {
                return resultWeather;
            }
            var openWeatherMapService = new OpenWeatherMapService(openWeatherMapOptions);
            var weather = openWeatherMapService.GetCurrentWeatherAsync(latitude, longitude).Result;
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

        public WeatherForecast GetWeatherForecast5Days(double latitude, double longitude)
        {
            //var openWeatherMapService = new OpenWeatherMapService(openWeatherMapOptions);
            //return openWeatherMapService.GetWeatherForecast5Async(latitude, longitude).Result;
            throw new NotImplementedException();
        }

        internal object GetWeatherHistory(double latitude, double longitude)
        {
            throw new NotImplementedException();
        }

        internal object GetWeatherForecastHistory(double latitude, double longitude)
        {
            throw new NotImplementedException();
        }
    }
}

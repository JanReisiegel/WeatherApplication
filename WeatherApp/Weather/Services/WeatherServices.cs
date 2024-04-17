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

        public WeatherServices(AppDbContext context)
        {
            _context = context;
            _openWeatherMapService = new OpenWeatherMapService(openWeatherMapOptions);
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
            Location location = _context.Locations.Where(x => x.Latitude == latitude && x.Longitude == longitude).FirstOrDefault() ?? StoreLocation(latitude, longitude);
            
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
            Location location = _context.Locations.Where(x => x.Latitude == latitude && x.Longitude == longitude).FirstOrDefault() ?? StoreLocation(latitude, longitude);
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

        private Location StoreLocation(double latitude, double longitude)
        {
            Location location = new Location
            {
                Latitude = latitude,
                Longitude = longitude,
            };
            _locationTransformation.GetCityName(ref location);
            _context.Locations.Add(location);
            _context.SaveChanges();
            return location;
        }

        public async Task<string> GetWeatherHistoryAsync(double latitude, double longitude, DateTime from, DateTime to)
        {
            string dateFormatUrl = "yyyy-MM-dd";
            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://archive-api.open-meteo.com/v1/archive")
            };
            var url =
                $"?latitude={latitude}&longitude={longitude}" +
                $"&start_date={from.ToString(dateFormatUrl)}&end_date={to.ToString(dateFormatUrl)}" +
                $"&hourly=temperature_2m,relative_humidity_2m,apparent_temperature,precipitation,rain,snowfall,snow_depth,weather_code,surface_pressure,cloud_cover,wind_speed_10m,wind_direction_10m,sunshine_duration" +
                $"&daily=weather_code,temperature_2m_mean,apparent_temperature_mean,sunrise,sunset,daylight_duration,sunshine_duration,precipitation_sum,rain_sum,snowfall_sum,precipitation_hours,wind_speed_10m_max,wind_direction_10m_dominant&format=flatbuffers";

            using HttpResponseMessage response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string text = await response.Content.ReadAsStringAsync();
            var neco = new OpenMeteo.OpenMeteoClient().GetWeather(new Uri("https://archive-api.open-meteo.com/v1/archive" + url));
            return text;
        }

    }
}

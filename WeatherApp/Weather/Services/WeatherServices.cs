﻿using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OpenWeatherMap;
using OpenWeatherMap.Models;
using System.Globalization;
using Weather.Models;
using Weather.ViewModels;

namespace Weather.Services
{
    public class WeatherServices
    {
        private readonly OpenWeatherMapService _openWeatherMapService;
        private readonly LocationTransformation _locationTransformation = new LocationTransformation();

        public WeatherServices()
        {
            _openWeatherMapService = new OpenWeatherMapService(openWeatherMapOptions);
        }

        private OpenWeatherMapOptions openWeatherMapOptions = new OpenWeatherMapOptions
        {
            ApiKey = Constants.OpenWeatherMapApiKey,
            ApiEndpoint = "https://api.openweathermap.org/data/2.5/",
            Language = "cs",
            UnitSystem = "metric",
        };


        public async Task<MyWeatherInfo> GetActualWeather(string cityName, string country)
        {
            Location location;
            try
            {
                location = await _locationTransformation.GetCoordinates(cityName, country);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            var weather = _openWeatherMapService.GetCurrentWeatherAsync(location.Latitude, location.Longitude).Result;

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
            };
            return myWeather;
        }

        public async Task<MyWeatherForecast> GetWeatherForecast5Days(string cityName, string country)
        {
            Location location;
            try
            {
                location = await _locationTransformation.GetCoordinates(cityName, country);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            WeatherForecast weatherForecast;
            try 
            {
                weatherForecast = _openWeatherMapService.GetWeatherForecast5Async(location.Latitude, location.Longitude).Result; 
            } 
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            MyWeatherForecast myWeatherForecast = new MyWeatherForecast
            {
                AcquireDateTime = DateTime.Now,
                Location = location,
                ForecastItems = new List<MyForecastItem>(),
            };
            myWeatherForecast.ForecastItems = new List<MyForecastItem>();
            weatherForecast.Items.ToList().ForEach(x =>
            {
                myWeatherForecast.ForecastItems.Add(new MyForecastItem
                {
                    DateTime = x.DateTime,
                    Temperature = x.Main.Temperature.DegreesCelsius,
                    FeelsTemperature = x.Main.FeelsLike.DegreesCelsius,
                    Pressure = x.Main.Pressure.Hectopascals,
                    Humidity = x.Main.Humidity.Value,
                    WindSpeed = x.Wind.Speed.MetersPerSecond,
                    Directory = x.Wind.Direction.Value,
                    Condition = (ConditionGroup)x.WeatherConditions[0].Main,
                });
            });
            return myWeatherForecast;
        }

        public async Task<HistoryWeather> GetWeatherHistory(Location location)
        {
            var client = new HttpClient();
            var url = $"{Constants.HistoryWeatherEndpoint}?key={Constants.HistoryKey}&q={location.Latitude.ToString(CultureInfo.InvariantCulture)},{location.Longitude.ToString(CultureInfo.InvariantCulture)}&dt={DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd")}&end_dt={DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd")}";
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var text = await response.Content.ReadAsStringAsync();
                var historyWeather = JsonConvert.DeserializeObject<HistoryWeather>(text);
                return historyWeather;
            }
            else
            {
                throw new Exception("history not found");
            }
        }
    }
}

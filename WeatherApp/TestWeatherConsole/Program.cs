// See https://aka.ms/new-console-template for more information

using Weather.Services;

WeatherServices _weather = new WeatherServices();
var result = _weather.GetActualWeather(50.08804, 14.42076);

Console.WriteLine($"Weather in Prague: ");
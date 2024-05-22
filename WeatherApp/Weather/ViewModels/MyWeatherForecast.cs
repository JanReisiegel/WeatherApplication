using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UnitsNet;
using Weather.Models;

namespace Weather.ViewModels
{
    public class MyWeatherForecast
    {
        public Location? Location { get; set; }
        public DateTime AcquireDateTime { get; set; }

        public List<MyForecastItem>? ForecastItems { get; set; }
    }

    public class MyForecastItem
    {
        public DateTime? DateTime { get; set; }
        //Teplota
        public double? Temperature { get; set; }
        public double? FeelsTemperature { get; set; }
        public double? Pressure { get; set; }
        public double? Humidity { get; set; }
        //vítr
        public double? WindSpeed { get; set; }
        public double? Directory { get; set; }
        //icon
        public ConditionGroup? Condition { get; set; }
    }
}

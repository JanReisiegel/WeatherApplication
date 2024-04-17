using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UnitsNet;

namespace Weather.Models
{
    public class MyWeatherForecast
    {
        [Key]
        public int Id { get; set; }
        public int LocationId { get; set; }
        [ForeignKey("LocationId")]
        public Location? Location { get; set; }
        public DateTime AcquireDateTime { get; set; }

        public ICollection<MyForecastItem>? ForecastItems { get; set; }
    }

    public class MyForecastItem
    {
        [Key]
        public int Id { get; set; }
        public int ForecastId { get; set; }
        [ForeignKey("ForecastId")]
        public MyWeatherForecast? Forecast { get; set; }
        public DateTime? DateTime { get; set; }
        //srážky
        public double? Rain1h { get; set; }
        public double? Rain3h { get; set; }
        //Teplota
        public double? Temperature { get; set; }
        public double? FeelsTemperature { get; set; }
        public double? MinTemperature { get; set; }
        public double? MaxTemperature { get; set; }
        public double? Pressure { get; set; }
        public double? Humidity { get; set; }
        //vítr
        public double? WindSpeed { get; set; }
        public double? Directory { get; set; }
        //icon
        public ConditionGroup? Condition { get; set; }
    }
}

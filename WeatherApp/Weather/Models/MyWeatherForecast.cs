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
        public DateTime DateTime { get; set; }

        public ICollection<MyForecastItem>? ForecastItems { get; set; }
    }

    public class MyForecastItem
    {
        [Key]
        public int Id { get; set; }
        public int ForecastId { get; set; }
        [ForeignKey("ForecastId")]
        public MyWeatherForecast? Forecast { get; set; }
        public DateTime DateTime { get; set; }
        //srážky
        public Length Rain { get; set; }
        //Teplota
        public Temperature Temperature { get; set; }
        public Temperature FeelsTemperature { get; set; }
        public Temperature MinTemperature { get; set; }
        public Temperature MaxTemperature { get; set; }
        public Pressure Pressure { get; set; }
        public RelativeHumidity Humidity { get; set; }
        //vítr
        public Speed WindSpeed { get; set; }
        public Angle Directory { get; set; }
        //icon
        public ConditionGroup Condition { get; set; }
    }
}

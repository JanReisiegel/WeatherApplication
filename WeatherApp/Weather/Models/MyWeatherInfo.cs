using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UnitsNet;

namespace Weather.Models
{
    public class MyWeatherInfo
    {
        [Key]
        public long Id { get; set; }
        public int LocationId { get; set; }
        [ForeignKey("LocationId")]
        public Location? Location { get; set; }
        //teplota
        public double Temperature { get; set; }
        public double FeelsTemperature { get; set; }
        public double Pressure { get; set; }
        public double Humidity { get; set; }
        //vítr
        public double WindSpeed { get; set; } //m/s
        public double Directory { get; set; } //°
        //mraky
        public double CloudsNow { get; set; } //procenta
        //slunce
        public DateTime Sunrise { get; set; }
        public DateTime Sunset { get; set; }
        public ConditionGroup Condition { get; set; }
    }
}

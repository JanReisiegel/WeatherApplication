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
        public Temperature Temperature { get; set; }
        public Temperature FeelsTemperature { get; set; }
        public Pressure Pressure { get; set; }
        //vítr
        public Speed WindSpeed { get; set; }
        public Angle Directory { get; set; }
        //mraky
        public Ratio CloudsNow { get; set; }
        public Ratio CloudsToday { get; set; }
        //slunce
        public DateTime Sunrise { get; set; }
        public DateTime Sunset { get; set; }
        public ConditionGroup Condition { get; set; }
    }
}

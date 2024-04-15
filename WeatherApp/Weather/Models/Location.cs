using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Weather.Models
{
    public class Location
    {
        [Required] //přidat uniq
        public int Id { get; set; }
        [Key]
        public double Latitude { get; set; }
        [Key]
        public double Longitude { get; set; }
        [Required]
        public string? CityName { get; set; }
        [Required]
        public double LocationName { get; set; }

        public ICollection<SavedLocation> SavedLocations { get; set; }
        public ICollection<MyWeatherForecast> MyWeatherForecasts { get; set;}
        public ICollection<MyWeatherInfo> MyWeatherInfos { get; set; }
    }
}

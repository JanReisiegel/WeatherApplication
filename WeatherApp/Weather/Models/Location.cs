using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Weather.Models
{
    public class Location
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }
        [Required]
        public string? CityName { get; set; }

        public ICollection<SavedLocation> SavedLocations { get; set; }
        public ICollection<MyWeatherForecast> MyWeatherForecasts { get; set;}
        public ICollection<MyWeatherInfo> MyWeatherInfos { get; set; }
    }
}

using OpenWeatherMap.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherApi.Models
{
    public class SavedLocation
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; } = "";
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
        [Required]
        public double Latitude { get; set; }
        [Required]
        public string Longitude { get; set; } = "";
        [Required]
        public string LocationName { get; set; } = "";
    }
}

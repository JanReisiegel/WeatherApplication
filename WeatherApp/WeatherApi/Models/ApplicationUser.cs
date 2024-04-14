using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WeatherApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public bool PaidAccount { get; set; } = false;
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }

        public ICollection<SavedLocation>? SavedLocations { get; set; }
    }
}

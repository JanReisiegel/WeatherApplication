using Microsoft.AspNetCore.Identity;

namespace Weather.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool PaidAccount { get; set; } = false;
        public List<Location> SavedLocations { get; set; } =new List<Location>();
    }
}

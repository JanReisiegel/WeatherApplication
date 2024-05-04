using Microsoft.AspNetCore.Identity;

namespace Weather.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool PaidAccount { get; set; } = false;
        public ICollection<SavedLocation>? SavedLocations { get; set; }
    }
}

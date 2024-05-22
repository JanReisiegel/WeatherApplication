using Microsoft.AspNetCore.Identity;

namespace Weather.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool PaidAccount { get; set; } = false;
        public List<Location> SavedLocations { get; set; } =new List<Location>();
    }
}

/*
{
  "user_id": "c4abc90c-d794-4104-a30c-6126d0144b01",
  "email": "jan3@tul.cz",
  "paid_account": "True",
  "username": "JanReiUz",
  "nbf": "1714740317",
  "exp": "1714747517",
  "iat": "1714740317"
}
*/
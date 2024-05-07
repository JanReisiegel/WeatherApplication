using Weather.Models;

namespace Weather.ViewModels
{
    public class UserVM
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public List<Location> Locations { get; set; } = new List<Location>();
        public bool PaidAccount { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Weather.Data;
using Weather.Models;

namespace Weather.Services
{
    public class LocationServices
    {
        private readonly LocationTransformation _locationTransformation;
        private AppDbContext _context;

        public LocationServices(AppDbContext context)
        {
            _context = context;
            _locationTransformation = new LocationTransformation();
        }

        public Location StoreLocation(double latitude, double longitude)
        {
            Location location = new Location
            {
                Latitude = latitude,
                Longitude = longitude,
            };
            _locationTransformation.GetCityName(ref location);
            _context.Locations.Add(location);
            _context.SaveChanges();
            return location;
        }
    }
}

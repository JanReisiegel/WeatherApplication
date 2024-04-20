using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Weather.Models;

namespace Weather.Services
{
    public class LocationServices
    {
        private readonly LocationTransformation _locationTransformation;
        private readonly UserServices _userService;

        public LocationServices()
        {
            _locationTransformation = new LocationTransformation();
            _userService = new UserServices();
        }

        public async Task<Location> StoreLocation(double latitude, double longitude, string customName, ApplicationUser userInput)
        {
            var user = _userService.GetUserByEmail(userInput .Email);
            Location location = new Location
            {
                Latitude = latitude,
                Longitude = longitude,
                CustomName = customName
            };
            _locationTransformation.GetCityName(ref location);
            user.SavedLocations.Add(location);
            
            await _userService.UpdateUser(user);
            return location;
        }
        public async Task<Location> StoreLocation(string cityName, string customName, ApplicationUser userInput)
        {
            var user = _userService.GetUserByEmail(userInput.Email);
            Location location = await _locationTransformation.GetCoordinates(cityName);
            user.SavedLocations.Add(location);

            await _userService.UpdateUser(user);
            return location;
        }
        public Location GetLocation(double latitude, double longitude, ApplicationUser user)
        {
            var locations = _userService.GetUserByEmail(user.Email).SavedLocations;
            return locations.FirstOrDefault(x => x.Latitude == latitude && x.Longitude == longitude) ?? null;
        }
        public Location GetLocation(string cityName, ApplicationUser user)
        {
            var locations = _userService.GetUserByEmail(user.Email).SavedLocations;
            return locations.FirstOrDefault(x =>x.CityName == cityName) ?? null;
        }
        public async Task<Location> GetLocation(string cityName)
        {
            var location = await _locationTransformation.GetCoordinates(cityName);
            return location;
        }
        public List<Location> GetAllLocations(ApplicationUser user)
        {
            var locations = _userService.GetUserByEmail(user.Email).SavedLocations;
            return locations.ToList();
        }
    }
}

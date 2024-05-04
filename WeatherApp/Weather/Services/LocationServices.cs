using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Weather.Models;
using Weather.MyExceptions;
using Weather.ViewModels;

namespace Weather.Services
{
    public class LocationServices
    {
        private readonly LocationTransformation _locationTransformation = new LocationTransformation();

        public async Task<Location> StoreLocation(double latitude, double longitude, string customName, ApplicationUser userInput)
        {
            var existUser = await JsonFileService.GetUserAsync(userInput.Email) ?? throw new UserException("User not found");

            Location location = new Location
            {
                Latitude = latitude,
                Longitude = longitude,
                CustomName = customName
            };
            _locationTransformation.GetCityName(ref location);
            UserVM user = new UserVM
            {
                Email = existUser.Email,
                Locations = existUser.SavedLocations,
                UserName = existUser.UserName,
                PhoneNumber = existUser.PhoneNumber,
                PaidAccount = existUser.PaidAccount,
                Password = existUser.PasswordHash
            };
            user.Locations.Add(location);
            
            
            await JsonFileService.UpdateUserAsync(user);
            return location;
        }
        public async Task<Location> StoreLocation(string cityName, string customName, ApplicationUser userInput)
        {
            var user = await JsonFileService.GetUserAsync(userInput.Email) ?? throw new UserException("User not found");
            Location location = await _locationTransformation.GetCoordinates(cityName);
            user.SavedLocations.Add(location);
            var result = await JsonFileService.UpdateUserAsync(user);
            if (result.Succeeded) { return location; }
            return null;
        }
        public async Task<Location> GetLocation(double latitude, double longitude, ApplicationUser user)
        {
            var existUser = await JsonFileService.GetUserAsync(user.Email);
            return existUser.SavedLocations.FirstOrDefault(x => x.Latitude == latitude && x.Longitude == longitude);
        }
        public async Task<Location> GetLocation(ApplicationUser user, string customName)
        {
            var existUser = await JsonFileService.GetUserAsync(user.Email);
            return existUser.SavedLocations.FirstOrDefault(x => x.CustomName == customName);
        }
        public async Task<Location> GetLocation(string cityName, ApplicationUser user)
        {
            var existUser = await JsonFileService.GetUserAsync(user.Email);
            return existUser.SavedLocations.FirstOrDefault(x =>x.CityName == cityName) ?? null;
        }
        public async Task<Location> GetLocation(string cityName)
        {
            var location = await _locationTransformation.GetCoordinates(cityName);
            return location;
        }
        public async Task<List<Location>> GetAllLocations(ApplicationUser user)
        {
            var existUser = await JsonFileService.GetUserAsync(user.Email);
            return existUser.SavedLocations.ToList();
        }
    }
}

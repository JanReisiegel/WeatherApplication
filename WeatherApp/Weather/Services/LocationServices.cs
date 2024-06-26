﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Weather.Models;
using Weather.ViewModels;

namespace Weather.Services
{
    public class LocationServices
    {
        private readonly LocationTransformation _locationTransformation = new LocationTransformation();
        public async Task<Location> StoreLocation(string cityName, string country, string customName, ApplicationUser userInput)
        {
            var user = await JsonFileService.GetUserAsync(userInput.Email) ?? throw new Exception("User not found");
            Location location = await _locationTransformation.GetCoordinates(cityName, country);
            location.CustomName = customName;
            user.SavedLocations.Add(location);
            var result = await JsonFileService.UpdateUserAsync(user);
            if (result.Succeeded) { return location; }
            return null;
        }
        public async Task<Location> GetLocation(ApplicationUser user, string customName)
        {
            var existUser = await JsonFileService.GetUserAsync(user.Email);
            return existUser.SavedLocations.FirstOrDefault(x => x.CustomName == customName);
        }
        public async Task<Location> GetLocation(string cityName, ApplicationUser user)
        {
            var existUser = await JsonFileService.GetUserAsync(user.Email);
            Location result = existUser.SavedLocations.FirstOrDefault(x => x.CityName == cityName) ?? throw new Exception("Location not found");
            return result;
        }
        public async Task<Location> GetLocation(string cityName, string country)
        {
            var location = await _locationTransformation.GetCoordinates(cityName, country);
            return location;
        }
        public async Task<List<Location>> GetAllLocations(ApplicationUser user)
        {
            var existUser = await JsonFileService.GetUserAsync(user.Email);
            return existUser.SavedLocations.ToList();
        }
    }
}

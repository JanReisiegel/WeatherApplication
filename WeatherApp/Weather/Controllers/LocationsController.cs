﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Weather.Models;
using Weather.Services;

namespace Weather.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly LocationServices _locationServices = new LocationServices();

        [HttpGet]
        public async Task<IActionResult> GetSavedLocation([FromHeader]string? userToken, [FromQuery]string cityName)
        {
            if (userToken == null || !(await UserServices.GetAuthenticate(UserServices.GetClaims(userToken)["email"])))
            {
                return Unauthorized("You must be logged in");
            }
            var user = await JsonFileService.GetUserAsync(UserServices.GetClaims(userToken)["email"]);
            var location = _locationServices.GetLocation(cityName, user);
            if (location == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "I can't find this city");
            }
            return Ok(location);
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetSavedLocations([FromHeader] string? userToken)
        {
            if (userToken == null || !(await UserServices.GetAuthenticate(UserServices.GetClaims(userToken)["email"])))
            {
                return Unauthorized("You must be logged in");
            }
            var user = await JsonFileService.GetUserAsync(UserServices.GetClaims(userToken)["email"]);
            var location = _locationServices.GetAllLocations(user);
            if (location == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Cannot load data from database, please contact admin");
            }
            return Ok(location);
        }
        [HttpPost]
        public async Task<IActionResult> SaveLocation([FromHeader]string? userToken, [FromQuery]string cityName, [FromQuery] string customName)
        {
            if (userToken == null || !(await UserServices.GetAuthenticate(UserServices.GetClaims(userToken)["email"])))
            {
                return Unauthorized("You must be logged in");
            }
            var user = await JsonFileService.GetUserAsync(UserServices.GetClaims(userToken)["email"]);
            Location location = _locationServices.StoreLocation(cityName, customName, user).Result;
            return Ok(location);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteLocation([FromHeader]string? userToken, [FromQuery]string customName)
        {
            if (userToken == null || !(await UserServices.GetAuthenticate(UserServices.GetClaims(userToken)["email"])))
            {
                return Unauthorized("You must be logged in");
            }
            var user = await JsonFileService.GetUserAsync(UserServices.GetClaims(userToken)["email"]);
            var location = await _locationServices.GetLocation(user, customName);
            if (location == null)
            {
                return NotFound("Location not found");
            }
            user.SavedLocations.Remove(location);
            var result = await JsonFileService.UpdateUserAsync(user);
            if (result.Succeeded)
            {
                return Ok("Location deleted");
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "Cannot store data in database, please contact admin");
        }
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenCage.Geocode;
using Weather.Models;
using Weather.ViewModels;

namespace Weather.Services
{
    public class LocationTransformation
    {
        public async Task<Models.Location> GetCoordinates(string cityName, string country)
        {
            var gc = new Geocoder(Constants.OpenGeoCodeApiKey);
            var response = gc.Geocode(cityName, countrycode: GetCountryCode(country));

            if (response.Status.Code is 200)
            {
                double latitude;
                double longitude;
                try
                {
                    latitude = response.Results.Average(x =>x.Geometry.Latitude);
                    longitude = response.Results.Average(x => x.Geometry.Longitude);
                }
                catch (InvalidOperationException e)
                {
                    throw new Exception("Město neexistuje");
                }
                catch (ArgumentNullException e)
                {
                    throw new Exception("Zadejte název města");
                }
                
                var location = new Models.Location();
                location.CityName = cityName;
                location.Latitude = longitude;
                location.Longitude = latitude;
                location.Country = country;
                return location;
            }
            throw new Exception("Město neexistuje");
        }
        public async Task<Models.Location> GetLocationFromCoords(double latitude, double longitude)
        {
            var gc = new Geocoder(Constants.OpenGeoCodeApiKey);
            var response = gc.ReverseGeocode(latitude, longitude);

            if (response.Status.Code is 200)
            {
                var location = new Models.Location();
                location.CityName = response.Results[0].Components.City;
                location.Latitude = latitude;
                location.Longitude = longitude;
                location.Country = response.Results[0].Components.Country;
                return location;
            }
            throw new Exception("Město neexistuje");
        }
        public string GetCountryCode(string country)
        {
            return country.ToLower() switch
            {
                "australia" => "au",
                "china" => "cn",
                "denmark" => "dk",
                "finland" => "fi",
                "france" => "fr",
                "morocco" => "ma",
                "netherlands" => "nl",
                "new zealand" => "nz",
                "norway" => "no",
                "united kingdom" => "gb",
                "united states" => "us",
                _ => country,
            };
        }
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Weather.Models;
using Weather.MyExceptions;
using Weather.ViewModels;

namespace Weather.Services
{
    public class LocationTransformation
    {
        public async Task<Location> GetCoordinates(string cityName)
        {
            var httpClient = new HttpClient();
            var url = $"https://api.opencagedata.com/geocode/v1/json?q={cityName}&key={Constants.OpenGeoCodeApiKey}";
            var response = await httpClient.GetAsync(url);

            if(response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var res = JsonConvert.DeserializeObject<GeocodingClass>(json);
                double latitude;
                double longitude;
                try
                {
                    latitude = res.Results.Average(x => x.Geometry.latitude);
                    longitude = res.Results.Average(x => x.Geometry.longitude);
                }
                catch (InvalidOperationException e)
                {
                    throw new LocationException("Město neexistuje");
                }
                catch (ArgumentNullException e)
                {
                    throw new LocationException("Zadejte název města");
                }
                
                var location = new Location();
                location.CityName = cityName;
                location.Latitude = longitude;
                location.Longitude = latitude;
                return location;
            }
            throw new LocationException("Město neexistuje");
        }
    }
}

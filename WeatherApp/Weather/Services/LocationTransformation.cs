using Newtonsoft.Json.Linq;
using Weather.Models;

namespace Weather.Services
{
    public class LocationTransformation
    {
        public void GetCityName(ref Location location)
        {
            var httpClient = new HttpClient();
            
            var url = $"https://nominatim.openstreetmap.org/reverse?format=json&lat={location.Latitude}&lon={location.Longitude}";
            var response = httpClient.GetAsync(url).Result;

            if(response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                var weather = JObject.Parse(content);

                string cityName = weather["address"]["city"].ToString() ?? weather["address"]["town"].ToString() ?? weather["address"]["village"].ToString() ?? weather["address"]["county"].ToString();
                location.CityName = cityName;
            }
        }

        public void GetCoordinates(ref Location location)
        {
            var httpClient = new HttpClient();
            
            var url = $"https://nominatim.openstreetmap.org/search?format=json&q={location.CityName}";
            var response = httpClient.GetAsync(url).Result;

            if(response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                var weather = JArray.Parse(content);

                location.Latitude = double.Parse(weather[0]["lat"].ToString());
                location.Longitude = double.Parse(weather[0]["lon"].ToString());
            }
        }
    }
}

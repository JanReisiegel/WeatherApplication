using Newtonsoft.Json.Linq;

namespace WeatherApi.Services
{
    public class LocationTransformation
    {
        public async static Task<double[]> CityNameToCoordinates(string cityName)
        {
            using (var httpClient = new HttpClient())
            {
                var url = $"https://nominatim.openstreetmap.org/search?format=json&q={cityName}";
                var response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var data = JArray.Parse(content);

                    if (data.Count > 0)
                    {
                        string latitude = data[0]["lat"].ToString();
                        string longitude = data[0]["lon"].ToString();

                        return new double[] { double.Parse(latitude), double.Parse(longitude) };
                    }
                    else
                    {
                        return new double[2];
                    }
                }
                else
                {
                    return new double[2];
                }
            }
        }
    }
}

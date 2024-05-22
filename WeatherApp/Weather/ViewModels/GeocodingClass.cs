using Newtonsoft.Json;

namespace Weather.ViewModels
{
    public class GeocodingClass
    {
        public List<Result> Results { get; set; }
    }
    public class Result
    {
        [JsonProperty("geometry")]
        public Geometry Geometry { get; set; }
        [JsonProperty("components")]
        public Component Component { get; set; }
    }
    public class Component
    {
        [JsonProperty("city")]
        public string City { get; set; }
    }
    public class Geometry
    {
        [JsonProperty("lat")]
        public double longitude { get; set; }
        [JsonProperty("lng")]
        public double latitude { get; set; }
    }
}

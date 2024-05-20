using Newtonsoft.Json;

namespace Weather.ViewModels
{
    public class HistoryWeather
    {
        [JsonProperty("forecast")]
        public Forecast Forecast { get; set; }
    }

    public class Forecast
    {
        [JsonProperty("forecastday")]
        public List<Forecastday> Forecastday { get; set; }
    }

    public class Forecastday
    {
        [JsonProperty("date")]
        public string Date { get; set; }
        [JsonProperty("day")]
        public Day Day { get; set; }
    }

    public class Day
    {
        [JsonProperty("avgtemp_c")]
        public double AVGTempC { get; set; }
        [JsonProperty("maxtemp_c")]
        public double MaxTempC { get; set; }
        [JsonProperty("mintemp_c")]
        public double MinTempC { get; set; }
        [JsonProperty("condition")]
        public Condition Condition { get; set; }
    }

    public class Condition
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("icon")]
        public string Icon { get; set; }
        [JsonProperty("code")]
        public int Code { get; set; }
    }
}

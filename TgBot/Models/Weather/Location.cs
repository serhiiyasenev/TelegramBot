using Newtonsoft.Json;

namespace TgBot.Models.Weather
{
    public class Location
    {
        [JsonProperty("woeid")]
        public int WhereOnEarthId { get; set; }
    }
}

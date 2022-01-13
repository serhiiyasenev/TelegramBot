using System.Collections.Generic;

namespace TgBot.Models.Weather
{
    public class WeatherInfo
    {
        public string Title { get; set; }
        public IEnumerable<Weather> ConsolidatedWeather { get; set; }
    }
}

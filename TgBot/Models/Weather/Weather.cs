using System;

namespace TgBot.Models.Weather
{
    public class Weather
    {
        public DateTime ApplicableDate { get; set; }
        public decimal MaxTemp { get; set; }
        public decimal MinTemp { get; set; }
        public decimal TheTemp { get; set; }
    }
}

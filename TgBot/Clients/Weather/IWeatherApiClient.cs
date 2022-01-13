using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TgBot.Models.Weather;

namespace TgBot.Clients.Weather
{
    public interface IWeatherApiClient
    {
        Task<IEnumerable<Location>> GetLocationsByQuery(string query, CancellationToken cancellationToken = default);

        Task<WeatherInfo> GetWeatherByWhereOnEarthId(int whereOnEarthId, CancellationToken cancellationToken = default);
    }
}

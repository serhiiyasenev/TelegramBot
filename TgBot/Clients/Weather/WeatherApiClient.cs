using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TgBot.Models.Weather;

namespace TgBot.Clients.Weather
{
    public class WeatherApiClient : IWeatherApiClient
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerSettings _settings;

        public WeatherApiClient(HttpClient client, JsonSerializerSettings settings)
        {
            _client = client;
            _settings = settings;
        }

        public async Task<IEnumerable<Location>> GetLocationsByQuery(string query, CancellationToken cancellationToken = default)
        {
            var response = await _client.GetAsync($"/api/location/search/?query={query}", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Weather API returned {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonConvert.DeserializeObject<IEnumerable<Location>>(content, _settings);

            return result;
        }

        public async Task<WeatherInfo> GetWeatherByWhereOnEarthId(int whereOnEarthId, CancellationToken cancellationToken = default)
        {
            var response = await _client.GetAsync($"/api/location/{whereOnEarthId}", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Weather API returned {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonConvert.DeserializeObject<WeatherInfo>(content, _settings);

            return result;
        }
    }
}

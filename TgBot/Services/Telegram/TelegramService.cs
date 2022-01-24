﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgBot.Clients.Weather;

namespace TgBot.Services.Telegram
{
    public class TelegramService : ITelegramService
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly IWeatherApiClient _weatherApiClient;

        public TelegramService(ITelegramBotClient telegramBotClient, IWeatherApiClient weatherApiClient)
        {
            _telegramBotClient = telegramBotClient;
            _weatherApiClient = weatherApiClient;
        }

        public async Task HandleMessage(Update update, CancellationToken cancellationToken = default)
        {
            try
            {
                if (update.Type != UpdateType.Message)
                {
                    await _telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, $"I cannot handle {update.Type}", cancellationToken: cancellationToken);
                    return;
                }

                if (update.Message.Text.Equals("/start") || update.Message.Text.Equals("start"))
                {
                    await _telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, $"Hi there! {update.Message.Chat.FirstName}", cancellationToken: cancellationToken);
                    return;
                }

                var query = update.Message.Text;
                var locations = await _weatherApiClient.GetLocationsByQuery(query, cancellationToken);
                var location = locations?.FirstOrDefault();

                if (location == null)
                {
                    await _telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, $"I cannot find location by query '{query}'", cancellationToken: cancellationToken);
                    return;
                }

                var weatherInfo = await _weatherApiClient.GetWeatherByWhereOnEarthId(location.WhereOnEarthId, cancellationToken);
                var weather = weatherInfo?.ConsolidatedWeather?.OrderBy(cw => cw.ApplicableDate).FirstOrDefault();

                if (weather != null)
                {
                    await _telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, $"Date: {weather.ApplicableDate.ToLongDateString()}. " +
                        $"The weather in {weatherInfo.Title} is between {weather.MinTemp:F1} and {weather.MaxTemp:F1} (current weather is {weather.TheTemp:F1})",
                        cancellationToken: cancellationToken);
                    return;
                }

                await _telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, $"I cannot find weather in location by query '{query}'",
                    cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Telegram Bot HandleMessage Exception: '{ex.Message}'");
            }
        }
    }
}

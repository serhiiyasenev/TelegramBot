using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using Telegram.Bot;
using TgBot.Clients.Weather;
using TgBot.Services;
using TgBot.Services.Telegram;

namespace TgBot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson();

            var botConfiguration = Configuration.Get<BotConfiguration>();

            services.AddSingleton(botConfiguration);

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            };

            services.AddHttpClient("weatherapi", client =>
                {
                    client.BaseAddress = new Uri("https://www.metaweather.com");
                    client.Timeout = TimeSpan.FromSeconds(15);
                })
                .AddTypedClient<IWeatherApiClient>(client => new WeatherApiClient(client, settings));

            services.AddHttpClient("tgclient")
                .AddTypedClient<ITelegramBotClient>(client =>
                    new TelegramBotClient(botConfiguration.BotAccessToken, client));

            services.AddTransient<ITelegramService, TelegramService>();

            services.AddHostedService<InitService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, BotConfiguration botConfiguration)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                var token = botConfiguration.BotAccessToken;

                endpoints.MapControllerRoute("tgwebhook", $"bot/{token}", new {controller = "Bot", action = "Post"});

                endpoints.MapControllers();
            });
        }
    }
}

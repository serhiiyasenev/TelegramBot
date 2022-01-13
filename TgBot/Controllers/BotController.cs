using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TgBot.Services.Telegram;

namespace TgBot.Controllers
{
    public class BotController : ControllerBase
    {
        private readonly ITelegramService _telegramService;

        public BotController(ITelegramService telegramService)
        {
            _telegramService = telegramService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Update update, CancellationToken cancellationToken = default)
        {
            await _telegramService.HandleMessage(update, cancellationToken);
            return Ok();
        }
    }
}

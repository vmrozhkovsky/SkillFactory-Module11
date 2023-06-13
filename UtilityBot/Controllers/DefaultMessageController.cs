using Telegram.Bot;
using Telegram.Bot.Types;
using UtilityBot.Configurations;

namespace UtilityBot.Controllers
{
    public class DefaultMessageController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly AppSettings _appSettings;

        public DefaultMessageController(ITelegramBotClient telegramBotClient, AppSettings appSettings)
        {
            _telegramClient = telegramBotClient;
            _appSettings = appSettings;
        }
        public async Task Handle(Message message, CancellationToken ct)
        {
            Console.WriteLine($"Контроллер {GetType().Name} получил сообщение.");
            await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Получено сообщение не поддерживаемого формата", cancellationToken: ct);
        }
    }
}
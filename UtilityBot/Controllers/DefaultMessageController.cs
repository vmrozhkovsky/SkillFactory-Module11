using Telegram.Bot;
using Telegram.Bot.Types;
using UtilityBot.Services;

namespace UtilityBot.Controllers
{
    public class DefaultMessageController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly IStorage _memoryStorage;

        public DefaultMessageController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
        }
        
        public async Task Handle(Message message, CancellationToken ct)
        {
            Console.WriteLine($"Контроллер {GetType().Name} получил сообщение.");
            await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Получено сообщение не поддерживаемого формата.", cancellationToken: ct);
        }
    }
}
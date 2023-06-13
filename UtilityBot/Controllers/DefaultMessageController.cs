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
            switch (_memoryStorage.GetSession(message.Chat.Id).UserFunction)
            {
                case "plus":
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Введите числа через пробел и получите их сумму.", cancellationToken: ct);
                    break;
                case "minus":
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Введите числа через пробел и получите их разницу.", cancellationToken: ct);
                    break;
                case "letter":
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Введите любое выражение и получите количество символов в нем.", cancellationToken: ct);
                    break;
            }
        }
    }
}
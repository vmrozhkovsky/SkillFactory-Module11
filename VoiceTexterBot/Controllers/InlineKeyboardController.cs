 
using Telegram.Bot;
using Telegram.Bot.Types;
using VoiceTexterBot.Services;

namespace VoiceTexterBot.Controllers
{
    public class InlineKeyboardController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly IStorage _memoryStorage;

        public InlineKeyboardController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
        }
        public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
        {
            _memoryStorage.GetSession(callbackQuery.From.Id).LanguageCode = callbackQuery.Data;
            Console.WriteLine($"Контроллер {GetType().Name} обнаружил нажатие на кнопку");
            await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id, $"Обнаружено нажатие на кнопку", cancellationToken: ct);
        }
    }
}
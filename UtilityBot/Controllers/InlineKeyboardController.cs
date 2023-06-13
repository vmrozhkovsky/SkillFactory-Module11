 
using Telegram.Bot;
using Telegram.Bot.Types;
using UtilityBot.Services;

namespace UtilityBot.Controllers
{
    
    // Класс контроллера нажатий кнопок
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
            Console.WriteLine($"Контроллер {GetType().Name} обнаружил нажатие на кнопку, {callbackQuery.Data}");
            _memoryStorage.GetSession(callbackQuery.From.Id).UserFunction = callbackQuery.Data;
            switch (callbackQuery.Data)
            {
                case "plus":
                    await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id, $"Введите числа через пробел и получите их сумму.{Environment.NewLine}Числа могут быть целыми или дробными.{Environment.NewLine}Разделитель в дробном числе - точка или запятая.", cancellationToken: ct);
                    break;
                case "minus":
                    await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id, $"Введите числа через пробел и получите их разницу.{Environment.NewLine}Числа могут быть целыми или дробными.{Environment.NewLine}Разделитель в дробном числе - точка или запятая.", cancellationToken: ct);
                    break;
                case "letter":
                    await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id, $"Введите любое выражение и получите количество символов в нем.", cancellationToken: ct);
                    break;
            }
        }
    }
}
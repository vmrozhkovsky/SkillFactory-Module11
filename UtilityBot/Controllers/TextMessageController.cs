using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using UtilityBot.Services;

namespace UtilityBot.Controllers;

public class TextMessageController
{
    private readonly ITelegramBotClient _telegramClient;
    private readonly IStorage _memoryStorage;
    private readonly ITextHandler _textHandler;

    public TextMessageController(ITelegramBotClient telegramBotClient, IStorage memoryStorage, ITextHandler textHandler)
    {
        _telegramClient = telegramBotClient;
        _memoryStorage = memoryStorage;
        _textHandler = textHandler;
    }
    
    public async Task Handle(Message message, CancellationToken ct)
    {
        Console.WriteLine($"Контроллер {GetType().Name} получил сообщение {message.Text}.");
        switch (message.Text)
        {
            case "/start":
                var buttons = new List<InlineKeyboardButton[]>();
                buttons.Add(new[]
                {
                    InlineKeyboardButton.WithCallbackData($" Сложение чисел" , $"plus"),
                    InlineKeyboardButton.WithCallbackData($" Вычитание чисел" , $"minus"),
                    InlineKeyboardButton.WithCallbackData($" Подсчет букв" , $"letter"),
                });
                await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b>  Выберите необходимое действие.</b>", cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));
                break;
            default:
                var userFunction = _memoryStorage.GetSession(message.Chat.Id).UserFunction;
                if (userFunction == "null")
                {
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, "Отправьте /start, чтобы получить инструкции.", cancellationToken: ct);
                }
                else
                {
                    var strMassive = _textHandler.MessageParse(message, userFunction, out bool error);
                    if (error)
                    {
                        await _telegramClient.SendTextMessageAsync(message.Chat.Id, "Введенные символы не соответствуют выбранному функционалу.", cancellationToken: ct);
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
                    else
                    {
                        var result = _textHandler.Process(strMassive, _memoryStorage.GetSession(message.Chat.Id).UserFunction);
                        await _telegramClient.SendTextMessageAsync(message.Chat.Id, result.ToString(), cancellationToken: ct);
                    }
                }
                break;
        }
    }
}
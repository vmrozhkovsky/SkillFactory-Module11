using System.Diagnostics.Eventing.Reader;
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
                var UserFunction = _memoryStorage.GetSession(message.Chat.Id).UserFunction;
                if (UserFunction == "null")
                {
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, "Отправьте /start, чтобы получить инструкции.", cancellationToken: ct);
                }
                else
                {
                    
                    var strMassive = _textHandler.MessageParse(message, UserFunction, out bool error);
                    if (error)
                    {
                        await _telegramClient.SendTextMessageAsync(message.Chat.Id, "Введенные символы не соответствуют выбранному функционалу.", cancellationToken: ct);
                    }
                    else
                    {
                        var Result = _textHandler.Process(strMassive, _memoryStorage.GetSession(message.Chat.Id).UserFunction);
                        await _telegramClient.SendTextMessageAsync(message.Chat.Id, Result.ToString(), cancellationToken: ct);
                    }
                }
                break;
        }
        Console.WriteLine($"Контроллер {GetType().Name} получил сообщение {message.Text}.");
    }
}
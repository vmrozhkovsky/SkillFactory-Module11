using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace VoiceTexterBot.Controllers;

public class TextMessageController
{
    private readonly ITelegramBotClient _telegramClient;

    public TextMessageController(ITelegramBotClient telegramBotClient)
    {
        _telegramClient = telegramBotClient;
    }
    
    public async Task Handle(Message message, CancellationToken ct)
    {
        //Console.WriteLine($"Контроллер {GetType().Name} получил сообщение");
        //await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Получено текстовое сообщение", cancellationToken: ct);
        switch (message.Text)
        {
            case "/start":

                // Объект, представляющий кнопки
                var buttons = new List<InlineKeyboardButton[]>();
                buttons.Add(new[]
                {
                    InlineKeyboardButton.WithCallbackData($" Русский" , $"ru"),
                    InlineKeyboardButton.WithCallbackData($" English" , $"en"),
                    InlineKeyboardButton.WithUrl("Орёл", "https://na-orel.ru")
                });

                // передаем кнопки вместе с сообщением (параметр ReplyMarkup)
                await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b>  Бот превращает аудио в текст.</b> {Environment.NewLine}" +
                                                                            $"{Environment.NewLine}Можно записать сообщение.{Environment.NewLine}", cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));

                break;
            default:
                await _telegramClient.SendTextMessageAsync(message.Chat.Id, "Отправьте аудио для превращения в текст.", cancellationToken: ct);
                break;
        }
        Console.WriteLine($"Контроллер {GetType().Name} получил сообщение {message.Text}");
    }
}
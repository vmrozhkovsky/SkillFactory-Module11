﻿using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using UtilityBot.Services;

namespace UtilityBot.Controllers
{
    
    // Класс контроллера входящих текстовых сообщений в зависимости от выбранного функционала
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
                case "/plus":
                    _memoryStorage.GetSession(message.Chat.Id).UserFunction = "plus";
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Введите числа через пробел и получите их сумму.{Environment.NewLine}Числа могут быть целыми или дробными.{Environment.NewLine}Разделитель в дробном числе - точка или запятая.", cancellationToken: ct);
                    break;
                case "/minus":
                    _memoryStorage.GetSession(message.Chat.Id).UserFunction = "minus";
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Введите числа через пробел и получите их разницу.{Environment.NewLine}Числа могут быть целыми или дробными.{Environment.NewLine}Разделитель в дробном числе - точка или запятая.", cancellationToken: ct);
                    break;
                case "/letter":
                    _memoryStorage.GetSession(message.Chat.Id).UserFunction = "letter";
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Введите любое выражение и получите количество символов в нем.", cancellationToken: ct);
                    break;
                default:
                    var userFunction = _memoryStorage.GetSession(message.Chat.Id).UserFunction;
                    if (userFunction == "null")
                    {
                        await _telegramClient.SendTextMessageAsync(message.Chat.Id, "Отправьте /start, чтобы получить инструкции.", cancellationToken: ct);
                    }
                    else
                    {
                        var result = _textHandler.Process(message, _memoryStorage.GetSession(message.Chat.Id).UserFunction, out bool error);
                        if (error)
                        {
                            await _telegramClient.SendTextMessageAsync(message.Chat.Id, "Введенные символы не соответствуют выбранному функционалу.", cancellationToken: ct);
                            switch (_memoryStorage.GetSession(message.Chat.Id).UserFunction)
                            {
                                case "plus":
                                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Введите числа через пробел и получите их сумму.{Environment.NewLine}Числа могут быть целыми или дробными.{Environment.NewLine}Разделитель в дробном числе - точка или запятая.", cancellationToken: ct);
                                    break;
                                case "minus":
                                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Введите числа через пробел и получите их разницу.{Environment.NewLine}Числа могут быть целыми или дробными.{Environment.NewLine}Разделитель в дробном числе - точка или запятая.", cancellationToken: ct);
                                    break;
                                case "letter":
                                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Введите любое выражение и получите количество символов в нем.", cancellationToken: ct);
                                    break;
                            }
                        }
                        else
                        {
                            await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Результат операции: {result.ToString()}", cancellationToken: ct);
                        }
                    }
                    break;
            }
        }
    }
}
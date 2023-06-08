﻿using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.Hosting;
using Telegram.Bot.Polling;
using VoiceTexterBot.Controllers;

namespace VoiceTexterBot;

public class Bot : BackgroundService
{
    private ITelegramBotClient _telegramClient;
    private InlineKeyboardController _inlineKeyboardController;
    private TextMessageController _textMessageController;
    private VoiceMessageController _voiceMessageController;
    private DefaultMessageController _defaultMessageController;
 
    public Bot(
        ITelegramBotClient telegramClient,
        InlineKeyboardController inlineKeyboardController,
        TextMessageController textMessageController,
        VoiceMessageController voiceMessageController,
        DefaultMessageController defaultMessageController)
    {
        _telegramClient = telegramClient;
        _inlineKeyboardController = inlineKeyboardController;
        _textMessageController = textMessageController;
        _voiceMessageController = voiceMessageController;
        _defaultMessageController = defaultMessageController;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _telegramClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            // Здесь выбираем, какие обновления хотим получать. В данном случае разрешены все
            new ReceiverOptions() { AllowedUpdates = { } }, 
            cancellationToken: stoppingToken);
 
        Console.WriteLine("Бот запущен");
    }
    async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        //  Обрабатываем нажатия на кнопки  из Telegram Bot API: https://core.telegram.org/bots/api#callbackquery
        if (update.Type == UpdateType.CallbackQuery)
        {
            await _inlineKeyboardController.Handle(update.CallbackQuery, cancellationToken); 
            // await _telegramClient.SendTextMessageAsync(update.Message.Chat.Id, "Вы нажали кнопку", cancellationToken: cancellationToken);
            return;
        }
 
        // Обрабатываем входящие сообщения из Telegram Bot API: https://core.telegram.org/bots/api#message
        if (update.Type == UpdateType.Message)
        {
            //Console.WriteLine($"Получено сообщение {update.Message.Text}");
            //await _telegramClient.SendTextMessageAsync(update.Message.Chat.Id, $"Вы отправили сообщение {update.Message.Text}", cancellationToken: cancellationToken);
            switch (update.Message!.Type)
            {
                case MessageType.Voice:
                    await _voiceMessageController.Handle(update.Message, cancellationToken);
                    return;
                case MessageType.Text:
                    await _textMessageController.Handle(update.Message, cancellationToken);
                    return;
                default:
                    await _defaultMessageController.Handle(update.Message, cancellationToken);
                    return;
            }
        }
    }
    Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        // Задаем сообщение об ошибке в зависимости от того, какая именно ошибка произошла
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };
 
        // Выводим в консоль информацию об ошибке
        Console.WriteLine(errorMessage);
 
        // Задержка перед повторным подключением
        Console.WriteLine("Ожидаем 10 секунд перед повторным подключением.");
        Thread.Sleep(10000);
 
        return Task.CompletedTask;
    }
}
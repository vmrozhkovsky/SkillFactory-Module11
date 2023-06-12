using Telegram.Bot;
using Telegram.Bot.Types;
using VoiceTexterBot.Configurations;
using VoiceTexterBot.Services;
using File = System.IO.File;


namespace VoiceTexterBot.Controllers
{
    public class VoiceMessageController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly AppSettings _appSettings;
        private readonly IFileHandler _audioFileHandler;

        public VoiceMessageController(ITelegramBotClient telegramBotClient, AppSettings appSettings, IFileHandler audioFileHandler)
        {
            _telegramClient = telegramBotClient;
            _appSettings = appSettings;
            _audioFileHandler = audioFileHandler;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            //await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Получено голосовое сообщение", cancellationToken: ct);
            var fileId = message.Voice?.FileId;
            if (fileId == null)
                return;
            Console.WriteLine($"Контроллер {GetType().Name} получил сообщение c ID {message.Voice?.FileId}");
            await _audioFileHandler.Download(fileId, ct, message);
            _audioFileHandler.Process("", message);
            await _telegramClient.SendTextMessageAsync(message.Chat.Id, "Голосовое сообщение загружено и сконвентировано.", cancellationToken: ct);
        }
    }
}
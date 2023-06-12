using Telegram.Bot;
using Telegram.Bot.Types;
using VoiceTexterBot.Configurations;
using VoiceTexterBot.Utilities;
using File = System.IO.File;

namespace VoiceTexterBot.Services;

public class AudioFileHandler : IFileHandler
{
    private readonly AppSettings _appSettings;
    private readonly ITelegramBotClient _telegramBotClient;

    public AudioFileHandler(ITelegramBotClient telegramBotClient, AppSettings appSettings)
    {
        _appSettings = appSettings;
        _telegramBotClient = telegramBotClient;
    }

    public async Task Download(string fileId, CancellationToken ct, Message message)
    {
        // Генерируем полный путь файла из конфигурации
        string inputAudioFilePath = Path.Combine(_appSettings.DownloadsFolder, $"{_appSettings.AudioFileName}-{message.Chat.Id}-{message.MessageId}.{_appSettings.InputAudioFormat}");

        using (FileStream destinationStream = File.Create(inputAudioFilePath))
        {
            // Загружаем информацию о файле
            var file = await _telegramBotClient.GetFileAsync(fileId, ct);
            if (file.FilePath == null)
                return;

            // Скачиваем файл
            await _telegramBotClient.DownloadFileAsync(file.FilePath, destinationStream, ct);
        }
    }

    public string Process(string languageCode, Message message)
    {
        string inputAudioPath = Path.Combine(_appSettings.DownloadsFolder, $"{_appSettings.AudioFileName}-{message.Chat.Id}-{message.MessageId}.{_appSettings.InputAudioFormat}");
        string outputAudioPath = Path.Combine(_appSettings.DownloadsFolder, $"{_appSettings.AudioFileName}-{message.Chat.Id}-{message.MessageId}.{_appSettings.OutputAudioFormat}");

        Console.WriteLine("Начинаем конвертацию...");
        AudioConverter.TryConvert(inputAudioPath, outputAudioPath);
        Console.WriteLine("Файл конвертирован");

        return "Конвертация успешно завершена";
    }
}
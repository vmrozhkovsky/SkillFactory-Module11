using Telegram.Bot.Types;

namespace VoiceTexterBot.Services;

public interface IFileHandler
{
    Task Download(string fileId, CancellationToken ct, Message message);
    string Process(string param);
}
using Telegram.Bot.Types;

namespace UtilityBot.Services;

public interface ITextHandler
{
    string[] MessageParse(Message message, string userFunction, out bool error);
    int Process(string[] massive, string userFunction);
}
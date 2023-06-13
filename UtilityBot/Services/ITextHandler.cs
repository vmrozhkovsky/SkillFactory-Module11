using Telegram.Bot.Types;

namespace UtilityBot.Services
{
    
    // Интерфейс для классов обработки входящих текстовых сообщений
    public interface ITextHandler
    {
        string[] MessageParse(Message message, string userFunction, out bool error);
        int Process(string[] massive, string userFunction);
    }
}
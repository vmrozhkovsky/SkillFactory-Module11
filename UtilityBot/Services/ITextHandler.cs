using Telegram.Bot.Types;

namespace UtilityBot.Services
{
    
    // Интерфейс для классов обработки входящих текстовых сообщений
    public interface ITextHandler
    {
        decimal Process(Message message, string userFunction, out bool error);
    }
}
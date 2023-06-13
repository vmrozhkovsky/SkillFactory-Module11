using UtilityBot.Models;

namespace UtilityBot.Services
{
    
    // Интерфейс для классов хранения настроек пользовательской сессии
    public interface IStorage
    {
        Session GetSession(long chatId);
    }
}
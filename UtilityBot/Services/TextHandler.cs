using Telegram.Bot;
using Telegram.Bot.Types;
using UtilityBot.Configurations;

namespace UtilityBot.Services
{
    
    // Класс обработки входящих текстовых сообщений
    public class TextHandler : ITextHandler
    {
        private readonly AppSettings _appSettings;
        private readonly ITelegramBotClient _telegramBotClient;

        public TextHandler(ITelegramBotClient telegramBotClient, AppSettings appSettings)
        {
            _appSettings = appSettings;
            _telegramBotClient = telegramBotClient;
        }

        public decimal Process(Message message, string userFunction, out bool error)
        {
            error = false;
            string[] massive = message.Text.Split(" ");
            if (userFunction == "plus" || userFunction == "minus")
            {
                try
                {
                    return TextActions.ActionsOnNumbers(massive, userFunction);
                }
                catch
                {
                    error = true;
                }
            }
            return TextActions.ActionsOnLetters(massive);
        }
    }
}
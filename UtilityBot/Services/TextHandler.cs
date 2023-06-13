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

        public string[] MessageParse(Message message, string userFunction, out bool error)
        {
            error = false;
            if (userFunction == "plus" || userFunction == "minus")
            {
                foreach (char a in message.Text)
                {
                    if (!(Char.IsDigit(a) || Char.ToString(a) == "." || Char.ToString(a) == "," || Char.IsSeparator(a)))
                    {
                        error = true;
                    }
                }
            }

            if (message.Text.StartsWith(".") || message.Text.StartsWith(","))
            {
                error = true;
            }

            return message.Text.Split(" ");
        }
        
        public decimal Process(string[] massive, string userFunction)
        {
            if (userFunction == "plus" || userFunction == "minus")
            {
                return TextActions.ActionsOnNumbers(massive, userFunction);
            }
            return TextActions.ActionsOnLetters(massive);
        }
    }
}
using Telegram.Bot;
using Telegram.Bot.Types;
using UtilityBot.Configurations;

namespace UtilityBot.Services;

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
                if (Char.IsLetter(a))
                {
                    error = true;
                }
            }
        }
        return message.Text.Split(" ");
    }
    public int Process(string[] massive, string userFunction)
    {
        if (userFunction == "plus" || userFunction == "minus")
        {
            return TextActions.ActionsOnNumbers(massive, userFunction);
        }
        return TextActions.ActionsOnLetters(massive);
    }
}
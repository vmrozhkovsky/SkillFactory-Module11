using Telegram.Bot.Types;

namespace UtilityBot.Services;

public class TextActions
{
    public static int ActionsOnNumbers(string[] massive, string userFunction)
    {
        int IntResult = 0;
        switch (userFunction)
        {
            case "plus":
            {
                foreach (string num in massive)
                {
                    if (num != "")
                    {
                        IntResult = IntResult + Convert.ToInt32(num);
                    }
                }
                break;
            }
            case "minus":
            {
                foreach (string num in massive)
                {
                    if (num != "")
                    {
                        IntResult = IntResult - Convert.ToInt32(num);
                    }
                }
                break;
            }
        }
        return IntResult;
    }

    public static int ActionsOnLetters(string[] massive)
    {
        int IntResult = 0;
        foreach (var element in massive)
        {
            foreach (char a in element)
            {
                IntResult++;
            }
        }
        return IntResult;
    }

}
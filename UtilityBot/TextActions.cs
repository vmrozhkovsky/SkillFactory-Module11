namespace UtilityBot;

public class TextActions
{
    
    public static int ActionsOnNumbers(string[] massive, string userFunction)
    {
        int intResult = 0;
        switch (userFunction)
        {
            case "plus":
            {
                foreach (string num in massive)
                {
                    if (num != "")
                    {
                        intResult = intResult + Convert.ToInt32(num);
                    }
                }
                break;
            }
            case "minus":
            {
                intResult = Convert.ToInt32(massive[0]);
                for (int i = 1; i < massive.Length; i++)
                    if (massive[i] != "")
                    {
                        intResult = intResult - Convert.ToInt32(massive[i]);
                    }
                break;
            }
        }
        return intResult;
    }

    public static int ActionsOnLetters(string[] massive)
    {
        int intResult = 0;
        foreach (var element in massive)
        {
            intResult = intResult + element.Length;
        }
        return intResult;
    }

}
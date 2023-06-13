namespace UtilityBot
{
public class TextActions
{
    
    // Класс функционала для входящих текстовых сообщений
        public static decimal ActionsOnNumbers(string[] massive, string userFunction)
        {
            decimal intResult = 0;
            switch (userFunction)
            {
                case "plus":
                {
                    foreach (string num in massive)
                    {
                        if (num != "")
                        {
                            intResult = intResult + Convert.ToDecimal(num.Replace(".", ","));
                        }
                    }
                    break;
                }
                case "minus":
                {
                    intResult = Convert.ToDecimal(massive[0].Replace(".", ","));
                    for (int i = 1; i < massive.Length; i++)
                        if (massive[i] != "")
                        {
                            intResult = intResult - Convert.ToDecimal(massive[i].Replace(".", ","));
                        }
                    break;
                }
            }
            return intResult;
        }

        public static decimal ActionsOnLetters(string[] massive)
        {
            int intResult = 0;
            foreach (var element in massive)
            {
                // element.Replace("\n", "");
                intResult = intResult + element.Replace("\n", "").Length;
            }
            return intResult;
        }

    }
}
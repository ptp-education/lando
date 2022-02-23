using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArgumentHelper : MonoBehaviour
{
    public static bool ContainsCommand(string commandName, string entireArgument)
    {
        string[] commands = entireArgument.Split(' ');
        foreach(string c in commands)
        {
            if (string.Equals(c, commandName))
            {
                return true;
            }
        }
        return false;
    }

    public static List<string> ArgumentsFromCommand(string commandName, string entireArgument)
    {
        string[] commands = entireArgument.Split(' ');

        List<string> args = new List<string>();

        for (int i = 0; i < commands.Length; i++)
        {
            if (string.Equals(commands[i], commandName))
            {
                for (int ii = 0; ii < commands.Length - i; ii++)
                {
                    int indexToCheck = i + 1 + ii;

                    if (indexToCheck < commands.Length && !commands[indexToCheck].StartsWith("-"))
                    {
                        args.Add(commands[indexToCheck]);
                    }
                    else
                    {
                        return args;
                    }
                }
            }
        }

        return args;
    }

    public static List<string> ArgumentsInQuotationsFromCommand(string command, string entireArgument)
    {
        List<string> args = new List<string>();

        string[] split = entireArgument.Split(' ');

        int start = entireArgument.IndexOf(command);

        while (entireArgument.IndexOf('\"', start) > -1)
        {
            int firstQuote = entireArgument.IndexOf('\"', start);
            int secondQuote = entireArgument.IndexOf('\"', firstQuote + 1);

            if (firstQuote == -1 || secondQuote == -1)
            {
                Debug.LogWarning("Could not find matching quotes for argument : " + entireArgument);
                return args;
            }

            args.Add(entireArgument.Substring(firstQuote + 1, secondQuote - firstQuote - 1));

            if (secondQuote + 1 >= entireArgument.Length)
            {
                return args;
            }

            if (entireArgument[secondQuote + 1] != ' ')
            {
                Debug.LogWarning("No trailing space after quotation for argument : " + entireArgument);
                return args;
            }

            if (entireArgument[secondQuote + 2] == '-')
            {
                return args;
            }

            start = secondQuote + 1;
        }

        return args;
    }
}

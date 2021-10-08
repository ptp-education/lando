using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceCode
{
    public class Command
    {
        public string AnimationName;
        public int Quantifier = 1;

        public Command(string animation)
        {
            AnimationName = animation;
        }
    }

    public List<Command> Commands = new List<Command>();

    public void AddCommand(string animation)
    {
        Commands.Add(new Command(animation));
    }

    public void RemoveCommand()
    {
        if (Commands.Count > 0)
        {
            Commands.RemoveAt(Commands.Count - 1);
        }
    }

    public override string ToString()
    {
        string ret = "";
        foreach(Command c in Commands)
        {
            ret += c.AnimationName;
            ret += "\n\n";
        }
        return ret;
    }
}

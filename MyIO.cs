using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dtp15_todolist
{
    class MyIO
    {
        static public string[] ReadCommands(string commandPrompt)
        {
            Console.Write(commandPrompt);
            string[] commandLines = Console.ReadLine().Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return commandLines;
        }
        static public bool Contains(string[] actualCommand, string expectedCommand)
        {
            foreach (string command in actualCommand)
            {
                command.ToLower().Trim();
                if (command == expectedCommand) return true;
            }
            return false;
        }
        static public bool HasArgument(string rawCommand, string expected)
        {
            string command = rawCommand.Trim();
            if (command == "") return false;
            else
            {
                string[] cwords = command.Split(' ');
                if (cwords.Length < 2) return false;
                if (cwords[1] == expected) return true;
            }
            return false;
        }
    }
}

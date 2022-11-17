using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dtp15_todolist
{
    class MyIO
    {
        static public string[] ReadCommand(string commandPrompt)
        {
            Console.Write(commandPrompt);
            string[] commandLines = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return commandLines;
        }
        static public bool CheckFirstCommand(string[] actualCommand, string expectedCommand)
        {
            actualCommand[0] = actualCommand[0].ToLower();
            if (actualCommand[0] == expectedCommand) return true;
            else return false;
        }
        static public bool CheckAdditionalCommands(string[] actualCommand, string expectedCommand)
        {
            bool returnBool = false;
            foreach (string command in actualCommand)
            {
                if (command.ToLower() == expectedCommand)
                {
                    returnBool = true;
                    break;
                }
                else returnBool = false;
            }
            return returnBool;
        }
    }
}

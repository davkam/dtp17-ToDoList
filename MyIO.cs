using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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
            if (actualCommand[0].ToLower() == expectedCommand) return true;
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
        static public bool CheckCommandTaskName(string[] actualCommand, string[] expectedCommand)
        {
            bool returnBool = false;
            for (int x = 0; x < actualCommand.Length; x++)
            {
                for (int y = 0; y < expectedCommand.Length; y++)
                {
                    if (actualCommand[x].ToLower() == expectedCommand[y])
                    {
                        returnBool = true;
                        break;
                    }
                }
            }
            return returnBool;
        }
        static public int CheckCommandTaskIndex(string[] actualCommand, string[] expectedCommand)
        {
            int returnInt = 0;
            for (int x = 0; x < actualCommand.Length; x++)
            {
                for (int y = 0; y < expectedCommand.Length; y++)
                {
                    if (actualCommand[x].ToLower() == expectedCommand[y])
                    {
                        returnInt = y;
                        break;
                    }
                }
            }
            return returnInt;
        }
        static public void CheckIndexRange(int value, int min, int max)
        {
            if (value < min || value > max) throw new Exception("Index out of range!");
        }
    }
}

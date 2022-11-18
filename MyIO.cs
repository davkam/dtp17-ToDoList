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
    /// <summary>
    /// <b>MyIO</b> class.
    /// Contains public methods for use in other classes.
    /// </summary>
        // TBD: rework method algorithms for improved user input features.
    class MyIO
    {
        /// <summary>
        /// <b>ReadCommand</b> method.
        /// Takes a string to write to console, reads response from console and splits it into an array.
        /// </summary>
        /// <param name="commandPrompt">string.</param>
        /// <returns>string array (user commands).</returns>
            // TBD: rework string split so it does not split sentences inside citation marks.
        static public string[] ReadCommand(string commandPrompt)
        {
            Console.Write(commandPrompt);
            string[] commandLines = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return commandLines;
        }

        /// <summary>
        /// <b>CheckFirstCommand</b> method.
        /// Takes two arguments (string array, and string) and compares array to string.
        /// If array contains an equal string element it returns true, otherwise false.
        /// </summary>
        /// <param name="actualCommand">string[].</param>
        /// <param name="expectedCommand">string.</param>
        /// <returns>bool (true or false).</returns>
        static public bool CheckFirstCommand(string[] actualCommand, string expectedCommand)
        {
            if (actualCommand[0].ToLower() == expectedCommand) return true;
            else return false;
        }

        /// <summary>
        /// <b>CheckAdditionalCommands</b> method
        /// Takes two arguments (string array, and string) and compares array to string.
        /// If array contains an equal string element it returns true, otherwise false.
        /// </summary>
        /// <param name="actualCommand">string[].</param>
        /// <param name="expectedCommand">string.</param>
        /// <returns>bool (true or false).</returns>
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

        /// <summary>
        /// <b>CheckCommandTaskName</b> method.
        /// Takes two string array arguments, and compares array to array.
        /// If the arrays contain at least one equal element it returns true, otherwise false.
        /// </summary>
        /// <param name="actualCommand">string[].</param>
        /// <param name="expectedCommand">string[].</param>
        /// <returns>bool (true or false).</returns>
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

        /// <summary>
        /// <b>CheckCommandTaskIndex</b> method.
        /// Takes two string array arguments, and compares array to array.
        /// If the arrays contain at least one equal element it returns the index array, otherwise returns integer set to 0.
        /// </summary>
        /// <param name="actualCommand">string[].</param>
        /// <param name="expectedCommand">string[].</param>
        /// <returns>integer (default = 0).</returns>
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

        /// <summary>
        /// <b>CheckIndexRange</b> method.
        /// Takes three integer values, and compares if integer value is between min and max.
        /// If not, throws an exception!
        /// </summary>
        /// <param name="value">integer.</param>
        /// <param name="min">integer.</param>
        /// <param name="max">integer.</param>
        /// <exception cref="Exception">Index out of range!</exception>
        static public void CheckIndexRange(int value, int min, int max)
        {
            if (value < min || value > max) throw new Exception("Index out of range!");
        }
        public static int SetIndex(string consoleOutput, int min, int max)
        {
            int index = -1;
        startIndexSet:
            Console.Write(consoleOutput);
            try
            {
                index = Int32.Parse(Console.ReadLine());
                MyIO.CheckIndexRange(index, min, max);
            }
            catch (System.FormatException)
            {
                Console.WriteLine(". ERROR! Input string not in correct format!");
                goto startIndexSet;
            }
            catch (System.Exception)
            {
                Console.WriteLine(". ERROR! Index out of range!");
                goto startIndexSet;
            }
            return index;
        }
    }
}

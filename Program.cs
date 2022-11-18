using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace dtp15_todolist
{
    class Program
    {
        public string[] primaryCommands;
        public string[] secondaryCommands;
        public string[] tertiaryCommands;
        public static void PrintHelp()
        {
            Console.WriteLine(".................COMMANDS.................\r\n");
            Console.WriteLine($"- {"help", -30}List all commands.");
            Console.WriteLine($"- {"list...   (-d)", -30}List all active tasks in to-do list, with or without description.");
            Console.WriteLine($"  {"   ...all (-d)", -30}List all tasks in to-do list, with or without description.");
            Console.WriteLine($"  {"   ...help",-30}Show possible \"list\" commands.");
            Console.WriteLine($"- {"status...",-30}");
            Console.WriteLine($"  {"   ...set",-30}Show all tasks, and change status on chosen task.");
            Console.WriteLine($"  {"   ...\"status\"",-30}Change status of specific task, (active, waiting or ready).");
            Console.WriteLine($"  {"   ...\"status\" \"task name\"",-30}Change status of specific task, (active, waiting or ready).");
            Console.WriteLine($"- {"new...", -30}Add new task to to-do list.");
            Console.WriteLine($"  {"   ...\"task name\"",-30}Add new task to to-do list, and initialize with a task name");
            Console.WriteLine($"- {"load",-30}Load to-do list from \"/todo.lis\".");
            Console.WriteLine($"- {"save", -30}Save current to-do list to \"/todo.lis\".");
            Console.WriteLine($"- {"quit", -30}Quit and save to-do list.");
            Console.WriteLine();
        }
        public static void AppRestart()
        {
            Process.Start("dtp15_todolist.exe");
            Process.GetCurrentProcess().Kill();
        }
        public static void UnknownCommand(string[] commandInput)
        {
            string commandString = "";
            foreach (string command in commandInput) commandString += command + " ";
            Console.WriteLine($"Unknown command: {commandString}");
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("................TO-DO LIST................\r\n");
            Todo.ReadListFromFile();
            PrintHelp();
            string[] commandLines;
            do
            {
                commandLines = MyIO.ReadCommand("> ");
                if (MyIO.CheckFirstCommand(commandLines, "help"))
                {
                    PrintHelp();
                }
                else if (MyIO.CheckFirstCommand(commandLines, "list"))
                {
                    if (MyIO.CheckAdditionalCommands(commandLines, "all"))
                    {
                        if (MyIO.CheckAdditionalCommands(commandLines, "-d")) Todo.PrintTodoList(allTasks: true, verbose: true);
                        else Todo.PrintTodoList(allTasks: true, verbose: false);
                    }
                    else if (MyIO.CheckAdditionalCommands(commandLines, "help"))
                    {
                        Console.WriteLine(". Possible \"list\" commands...");
                        Console.WriteLine($"- {"list",-20}Shows all active tasks without description.");
                        Console.WriteLine($"- {"list +d",-20}Shows all active tasks with description.");
                        Console.WriteLine($"- {"list all",-20}Shows all tasks without description.");
                        Console.WriteLine($"- {"list all +d",-20}Shows all tasks with description.");
                    }
                    else
                    {
                        if (MyIO.CheckAdditionalCommands(commandLines, "-d")) Todo.PrintTodoList(allTasks: false, verbose: true);
                        else Todo.PrintTodoList(allTasks: false, verbose: false);
                    }
                }
                else if (MyIO.CheckFirstCommand(commandLines, "status"))
                {
                    if (MyIO.CheckAdditionalCommands(commandLines, "set")) Todo.ChangeTaskStatus();
                    else if (MyIO.CheckAdditionalCommands(commandLines, "active"))
                    {
                        if (commandLines.Length > 2) Todo.ChangeTaskStatus(Todo.Active, commandLines[2]);
                        else Todo.ChangeTaskStatus(Todo.Active);
                    }
                    else if (MyIO.CheckAdditionalCommands(commandLines, "waiting"))
                    {
                        if (commandLines.Length > 2) Todo.ChangeTaskStatus(Todo.Waiting, commandLines[2]);
                        else Todo.ChangeTaskStatus(Todo.Waiting);
                    }
                    else if (MyIO.CheckAdditionalCommands(commandLines, "ready"))
                    {
                        if (commandLines.Length > 2) Todo.ChangeTaskStatus(Todo.Ready, commandLines[2]);
                        else Todo.ChangeTaskStatus(Todo.Ready);
                    }
                    else UnknownCommand(commandLines);
                }
                else if (MyIO.CheckFirstCommand(commandLines, "new"))
                {
                    if (commandLines.Length > 1 && commandLines[1] != "")
                    {
                        Todo.AddNewTask(commandLines[1]);
                    }
                    else Todo.AddNewTask();
                }
                else if (MyIO.CheckFirstCommand(commandLines, "load"))
                {
                    Todo.ReadListFromFile();
                }
                else if (MyIO.CheckFirstCommand(commandLines, "save"))
                {
                    Todo.SaveListToFile();
                }
                else if (MyIO.CheckFirstCommand(commandLines, "quit"))
                {
                    Todo.SaveListToFile();
                    Console.WriteLine("\r\n. Thank you for using To-Do List!\r\n. Press any key to shutdown application.");
                    Console.ReadKey(true);
                    break;
                }
                else
                {
                    UnknownCommand(commandLines);
                }
            }
            while (true);
        }
    }
}

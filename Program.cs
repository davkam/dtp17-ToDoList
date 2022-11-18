using System.Diagnostics;

namespace dtp15_todolist
{
    class Program
    {
        public static string[] taskNameCommands;
        public static void PrintWelcome()
        {
            Console.WriteLine("..................ABOUT...................\r\n");
            Console.WriteLine(". Welcome to To-Do List!");
            Console.WriteLine(". This app was created for educational purposes.");
            Console.WriteLine(". To start off, enter the commands given below.\r\n");
        }
        public static void PrintHelp()
        {
            Console.WriteLine(".................COMMANDS.................\r\n");
            Console.WriteLine($"- {"help", -30}List all commands.");
            Console.WriteLine($"- {"list...    -d", -30}List all active tasks in to-do list, with or without description (-d).");
            Console.WriteLine($"  {"    ...all -d", -30}List all tasks in to-do list, with or without description (-d).");
            Console.WriteLine($"  {"    ...\"task name\"",-30}List specific task in to-do list with description.");
            Console.WriteLine($"  {"    ...help",-30}Show possible \"list\" commands.");
            Console.WriteLine($"- {"status...",-30}");
            Console.WriteLine($"  {"    ...change",-30}Show all tasks, and change status on chosen task.");
            Console.WriteLine($"  {"    ...\"task name\"",-30}Change status of specific task.");
            Console.WriteLine($"  {"    ...\"task name\" \"status\"",-30}Change status of specific task to active, waiting or ready.");
            Console.WriteLine($"- {"new...", -30}Add new task to to-do list.");
            Console.WriteLine($"  {"    ...\"task name\"",-30}Add new task to to-do list, and initialize with a task name"); // delete, clear, change 
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
            taskNameCommands = new string[Todo.todoList.Count];
            Todo.AddTaskNamesToCommand();
            PrintWelcome();
            PrintHelp();
            string[] commandLines;
            do
            {
                commandLines = MyIO.ReadCommand("> ");
                if (MyIO.CheckFirstCommand(commandLines, "help") && commandLines.Length <= 1)
                {
                    PrintHelp();
                }
                else if (MyIO.CheckFirstCommand(commandLines, "list"))
                {
                    if (MyIO.CheckAdditionalCommands(commandLines, "all") && commandLines.Length <= 3)
                    {
                        if (MyIO.CheckAdditionalCommands(commandLines, "-d")) Todo.PrintTodoList(allTasks: true, verbose: true);
                        else Todo.PrintTodoList(allTasks: true, verbose: false);
                    }
                    else if (MyIO.CheckCommandTaskName(commandLines, taskNameCommands) && commandLines.Length <= 2)
                    {
                        int taskIndex = MyIO.CheckCommandTaskIndex(commandLines, taskNameCommands);
                        Todo.PrintHead(true);
                        Todo.todoList[taskIndex].Print(true, taskIndex);
                        Todo.PrintFoot(true);
                    }
                    else if (MyIO.CheckAdditionalCommands(commandLines, "help") && commandLines.Length <= 2)
                    {
                        Console.WriteLine(". Possible \"list\" commands...");
                        Console.WriteLine($"- {"list",-20}Shows all active tasks without description.");
                        Console.WriteLine($"- {"list -d",-20}Shows all active tasks with description.");
                        Console.WriteLine($"- {"list all",-20}Shows all tasks without description.");
                        Console.WriteLine($"- {"list all -d",-20}Shows all tasks with description.");
                        Console.WriteLine($"- {"list \"task name\"",-20}Shows specific task with description.");
                    }
                    else if (MyIO.CheckAdditionalCommands(commandLines, "-d") || commandLines.Length <= 1)
                    {
                        if (MyIO.CheckAdditionalCommands(commandLines, "-d")) Todo.PrintTodoList(allTasks: false, verbose: true);
                        else Todo.PrintTodoList(allTasks: false, verbose: false);
                    }
                    else UnknownCommand(commandLines);
                }
                else if (MyIO.CheckFirstCommand(commandLines, "status"))
                {
                    if (MyIO.CheckAdditionalCommands(commandLines, "change") && commandLines.Length <= 2) Todo.ChangeTaskStatus();
                    else if (MyIO.CheckCommandTaskName(commandLines, taskNameCommands) && commandLines.Length <= 3 )
                    {
                        int taskIndex = MyIO.CheckCommandTaskIndex(commandLines, taskNameCommands);

                        if (MyIO.CheckAdditionalCommands(commandLines, "active")) Todo.ChangeTaskStatus(Todo.todoList[taskIndex].taskName, Todo.Active);
                        else if (MyIO.CheckAdditionalCommands(commandLines, "waiting")) Todo.ChangeTaskStatus(Todo.todoList[taskIndex].taskName, Todo.Waiting);
                        else if (MyIO.CheckAdditionalCommands(commandLines, "ready")) Todo.ChangeTaskStatus(Todo.todoList[taskIndex].taskName, Todo.Ready);
                        else if (commandLines.Length <= 2) Todo.ChangeTaskStatus(Todo.todoList[taskIndex].taskName, 0);
                        else UnknownCommand(commandLines);
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

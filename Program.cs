using System.Diagnostics;

namespace dtp15_todolist
{
    /// <summary>
    /// <b>Program</b> Class. 
    /// Contains public methods and variables, including 'Main' where application starts.
    /// </summary>
        // NYI: create a file of 'most-used' strings (eg. in PrintHelp and AppStartUp) in dir to read from.
    class Program
    {
        /// <summary>
        /// Public string array containing task names for command input. 
        /// </summary>
        public static string[]? taskNameCommands;

        /// <summary>
        /// <b>AppStartUp</b> method.
        /// A set of given commands that runs on app start-up.
        /// </summary>
        public static void AppStartUp()
        {
            Console.WriteLine("................TO-DO LIST................\r\n");

            Todo.ReadListFromFile();

            taskNameCommands = new string[Todo.todoList.Count];
            Todo.AddTaskNamesToCommand();

            Console.WriteLine("..................ABOUT...................\r\n");
            Console.WriteLine(". Welcome to To-Do List!");
            Console.WriteLine(". This app was created for educational purposes.");
            Console.WriteLine(". To start off, enter the commands given below.\r\n");
        }

        /// <summary>
        /// <b>AppRestart</b> method.
        /// Restarts application by starting new process and terminating current process.
        /// </summary>
        public static void AppRestart()
        {
            try { Process.Start("dtp15_todolist.exe"); }
            catch (System.ComponentModel.Win32Exception)
            {
                Console.WriteLine(". ERROR: Could not open file \"dtp15_todolist.exe\".");
            }
            Process.GetCurrentProcess().Kill();
        }

        /// <summary>
        /// <b>PrintHelp</b> method.
        /// Prints a set of strings (app 'help' commands) to console.
        /// </summary>
        public static void PrintHelp()
        {
            Console.WriteLine(".................COMMANDS.................\r\n");
            Console.WriteLine($"- {"help",-30}List all commands.");
            Console.WriteLine($"- {"list...     -d",-30}List all active tasks in to-do list, with or without description (-d).");
            Console.WriteLine($"  {"     ...all -d",-30}List all tasks in to-do list, with or without description (-d).");
            Console.WriteLine($"  {"     ...\"task name\"",-30}List specified task in to-do list with description.");
            Console.WriteLine($"  {"     ...help",-30}Show possible \"list\" commands.");
            Console.WriteLine($"- {"status...",-30}");
            Console.WriteLine($"  {"     ...change",-30}Show all tasks, and change status on chosen task.");
            Console.WriteLine($"  {"     ...\"task name\"",-30}Change status of specified task.");
            Console.WriteLine($"  {"     ...\"task name\" \"status\"",-30}Change status of specified task to active, waiting or ready.");
            Console.WriteLine($"- {"new...",-30}Add new task to to-do list.");
            Console.WriteLine($"  {"     ...\"task name\"",-30}Add new task to to-do list, and initialize with specified task name");
            Console.WriteLine($"- {"delete...",-30}Show all tasks, and delete chosen task.");
            Console.WriteLine($"  {"     ...all",-30}Delete all tasks, and clear to-do list.");
            Console.WriteLine($"  {"     ...\"task name\"",-30}Delete specified task in to-do list.");
            Console.WriteLine($"- {"load",-30}Load to-do list from \"/todo.lis\".");
            Console.WriteLine($"- {"save",-30}Save current to-do list to \"/todo.lis\".");
            Console.WriteLine($"- {"quit",-30}Quit and save to-do list.");
            Console.WriteLine();
        }

        /// <summary>
        /// <b>UnknownCommand</b> method.
        /// Takes argument string array, converts it into a string and prints it.
        /// Used with 'Main' method to print unknown user input commands.
        /// </summary>
        /// <param name="commandInput">string array</param>
        public static void UnknownCommand(string[] commandInput)
        {
            string commandString = "";

            foreach (string command in commandInput) commandString += command + " ";

            Console.WriteLine($"Unknown command: {commandString}");
        }

        /// <summary>
        /// <b>Main</b> method.
        /// Application starts here, runs initial tasks then enters loop with conditional statements.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
                // application starts here:
            AppStartUp();
            PrintHelp();
            string[] commandLines;
            do
            {
                    // do-while loop with conditional statements based on user input command:
                    // NYI: add option to show directory of all ".lis" files to load, save, or delete.
                    // TBD: add save, load to specific files (create static methods if needed).
                    // TBD: rework new "task name" command so it can register a task name with multiple words.
                    // TBD: rework do-while loop with switch cases and elaborate MyIO-methods to better suit that.
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
                    else if (MyIO.CheckCommandTaskName(commandLines, taskNameCommands) && commandLines.Length <= 3)
                    {
                        int taskIndex = MyIO.CheckCommandTaskIndex(commandLines, taskNameCommands);

                        if (MyIO.CheckAdditionalCommands(commandLines, "active"))
                        {
                            Todo.ChangeTaskStatus(Todo.todoList[taskIndex].taskName, Todo.Active);
                        }
                        else if (MyIO.CheckAdditionalCommands(commandLines, "waiting"))
                        {
                            Todo.ChangeTaskStatus(Todo.todoList[taskIndex].taskName, Todo.Waiting);
                        }
                        else if (MyIO.CheckAdditionalCommands(commandLines, "ready"))
                        {
                            Todo.ChangeTaskStatus(Todo.todoList[taskIndex].taskName, Todo.Ready);
                        }
                        else if (commandLines.Length <= 2)
                        {
                            Todo.ChangeTaskStatus(Todo.todoList[taskIndex].taskName, 0);
                        }
                        else UnknownCommand(commandLines);
                    }
                    else UnknownCommand(commandLines);
                }
                else if (MyIO.CheckFirstCommand(commandLines, "new") && commandLines.Length <= 2)
                {
                    if (commandLines.Length > 1 && commandLines[1] != "")
                    {
                        Todo.AddNewTask(commandLines[1]);
                    }
                    else Todo.AddNewTask();
                }
                else if (MyIO.CheckFirstCommand(commandLines, "delete"))
                {
                    if (commandLines.Length <= 1)
                    {
                        Todo.DeleteTask(false);
                    }
                    else if (MyIO.CheckAdditionalCommands(commandLines, "all") && commandLines.Length <= 2)
                    {
                        Todo.DeleteTask(true);
                    }
                    else if (MyIO.CheckCommandTaskName(commandLines, taskNameCommands) && commandLines.Length <= 2)
                    {
                        int taskIndex = MyIO.CheckCommandTaskIndex(commandLines, taskNameCommands);

                        Todo.DeleteTask(false, Todo.todoList[taskIndex].taskName); 
                    }
                    else UnknownCommand(commandLines);
                }
                else if (MyIO.CheckFirstCommand(commandLines, "load") && commandLines.Length <= 1)
                {
                    Todo.ReadListFromFile();
                }
                else if (MyIO.CheckFirstCommand(commandLines, "save") && commandLines.Length <= 1)
                {
                    Todo.SaveListToFile();
                }
                else if (MyIO.CheckFirstCommand(commandLines, "quit") && commandLines.Length <= 1)
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

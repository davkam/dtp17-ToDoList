using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace dtp15_todolist
{
    class MainClass
    {
        public static void AddNewTask(string taskName = "")
        {
            ConsoleKeyInfo keyPressed;
        setTask:
            int newTaskStatus = Todo.Waiting;
            string newTaskName;
            if (taskName == "")
            {
                Console.Write(". Set new task name: ");
                newTaskName = Console.ReadLine();
            }
            else
            {
                newTaskName = taskName;
                Console.WriteLine($". New task name set to \"{newTaskName}\".");
            }
            Console.Write(". Set task description: ");
            string? newTaskDescription = Console.ReadLine();

        setPrio:
            Console.Write(". Set task priority level (1-4): ");
            int newTaskPriority;
            try
            {
                newTaskPriority = Int32.Parse(Console.ReadLine());
                if (newTaskPriority < 1 || newTaskPriority > 4)
                {
                    Console.WriteLine(". INCORRECT! Input out of range!");
                    goto setPrio;
                }
            }
            catch (System.FormatException)
            {
                Console.WriteLine(". INCORRECT! Input not in correct format (1-4).");
                goto setPrio;
            }

            Console.WriteLine($". Add new task?\r\n\r\n{"TASK:",-20}{newTaskName}\r\n{"TASK DESCRIPTION:",-20}{newTaskDescription}\r\n{"TASK PRIORITY",-20}{newTaskPriority}\r\n{"TASK STATUS:",-20}{Todo.StatusToString(newTaskStatus)} (Default)\r\n");
        finalTask:
            Console.WriteLine(". Press Enter to add to list, Backspace to redo or Escape to quit new task!");
            {
                keyPressed = Console.ReadKey(true);
                if (keyPressed.Key == ConsoleKey.Enter)
                {
                    string taskLine = $"{newTaskStatus}|{newTaskPriority}|{newTaskName.Trim()}|{newTaskDescription.Trim()}";
                    Todo.TodoItem newTask = new Todo.TodoItem(taskLine);
                    Todo.todoList.Add(newTask);
                    Console.WriteLine($". New task \"{newTaskName}\" successfully added to list!\r\n. To change task status, use command \"status\".");
                }
                else if (keyPressed.Key == ConsoleKey.Backspace) goto setTask;
                else if (keyPressed.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine($". New task \"{newTaskName}\" NOT created!");
                    goto taskDone;
                }
                else goto finalTask;
            }
        taskDone:;
        }
        public static void PrintHelp()
        {
            Console.WriteLine(".................COMMANDS.................\r\n");
            Console.WriteLine($"- {"help", -30}List all commands.");
            Console.WriteLine($"- {"list...    (+/-)d", -30}List all active tasks in to-do list, with or without description.");
            Console.WriteLine($"  {"    ...all (+/-)d", -30}List all tasks in to-do list, with or without description.");
            Console.WriteLine($"  {"    ...help",-30}Shows possible \"list\" commands.");
            Console.WriteLine($"- {"new...", -30}Add new task to to-do list.");
            Console.WriteLine($"  {"   ...\"task name\"", -30}Add new task to to-do list, and initialize with a task name.");
            Console.WriteLine($"- {"save", -30}Saves current to-do list to \"/todo.lis\".");
            Console.WriteLine($"- {"quit", -30}Quit and save to-do list.");
            Console.WriteLine();
        }
        public static void AppRestart()
        {
            Process.Start("dtp15_todolist.exe");
            Process.GetCurrentProcess().Kill();
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
                        if (MyIO.CheckAdditionalCommands(commandLines, "+d")) Todo.PrintTodoList(allTasks: true, verbose: true);
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
                        if (MyIO.CheckAdditionalCommands(commandLines, "+d")) Todo.PrintTodoList(allTasks: false, verbose: true);
                        else Todo.PrintTodoList(allTasks: false, verbose: false);
                    }
                }
                else if (MyIO.CheckFirstCommand(commandLines, "new"))
                {
                    if (commandLines.Length > 1 && commandLines[1] != "")
                    {
                        AddNewTask(commandLines[1]);
                    }
                    else AddNewTask();
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
                    string commandString = "";
                    foreach (string command in commandLines) commandString += command + " ";
                    Console.WriteLine($"Unknown command: {commandString}");
                }
            }
            while (true);
        }
    }
}

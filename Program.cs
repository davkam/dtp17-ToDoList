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
                    Todo.list.Add(newTask);
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
            Console.WriteLine($"- {"list...    +/-describe", -30}List all active tasks in to-do list, with or without description.");
            Console.WriteLine($"  {"    ...all +/-describe", -30}List all tasks in to-do list, with or without description.");
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
            string command;
            do
            {
                command = MyIO.ReadCommand("> ");
                if (MyIO.Equals(command, "help"))
                {
                    PrintHelp();
                }
                else if (MyIO.Equals(command, "list"))
                {
                    if (MyIO.HasArgument(command, "all"))
                        Todo.PrintTodoList(verbose: true);
                    else
                        Todo.PrintTodoList(verbose: false);
                }
                else if (MyIO.Equals(command, "new"))
                {
                    AddNewTask();
                }
                else if (MyIO.Equals(command, "save"))
                {
                    Todo.SaveListToFile();
                }
                else if (MyIO.Equals(command, "quit"))
                {
                    Todo.SaveListToFile();
                    Console.WriteLine(". Thank you for using To-Do List!\r\n. Press any key to shutdown application.");
                    break;
                }
                else
                {
                    Console.WriteLine($"Unknown command: {command}");
                }
            }
            while (true);
        }
    }
}

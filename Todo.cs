using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace dtp15_todolist
{
    /// <summary>
    /// <b>Todo</b> class.
    /// Contains public methods and variables involved in 'to-do task' handling.
    /// </summary>
        // NYI: add new method 'ListSorter' to sort objects in class according to 'taskStatus' and then 'taskPriority'.
        // NYI: add new "FindTaskIndex" method for use in other methods.
        // TBD: make todoFileName a public attribute for use in other methods.
    public class Todo
    {
        /// <summary>
        /// public list of class objects 'todoList'.
        /// </summary>
        public static List<TodoItem> todoList = new List<TodoItem>();

        public const int Active = 1;
        public const int Waiting = 2;
        public const int Ready = 3;

        /// <summary>
        /// <b>TodoItem</b> subclass.
        /// Contains public methods, variables and a constructor for 'to-do task' handling.
        /// </summary>
        public class TodoItem
        {
            public int taskStatus;
            public int taskPriority;
            public string taskName;
            public string taskDescription;

            /// <summary>
            /// <b>TodoItem</b> constructor.
            /// Takes argument string, splits it into array, and sets variables accordingly.
            /// </summary>
            /// <param name="todoline">string.</param>
            public TodoItem(string todoline)
            {
                string[] field = todoline.Split('|');
                taskStatus = Int32.Parse(field[0]);
                taskPriority = Int32.Parse(field[1]);
                taskName = field[2];
                taskDescription = field[3];
            }

            /// <summary>
            /// <b>Print</b> method.
            /// Console print method for class object 'TodoItem'.
            /// Takes two argument, bool and integer, if bool is 'true' it also print 'taskDescription'.
            /// </summary>
            /// <param name="verbose">bool (default = false).</param>
            /// <param name="i">integer (default = 0).</param>
            public void Print(bool verbose = false, int i = 0)
            {
                string statusString = StatusToString(taskStatus);

                Console.Write($"|{"#" + (i + 1) + ". " + taskName,-20}|{statusString,-12}|{taskPriority,-6}|");
                if (verbose)
                    Console.WriteLine($"{taskDescription,-40}|");
                else
                    Console.WriteLine();
            }
        }

        /// <summary>
        /// <b>StatusToString</b> method.
        /// Takes argument integer, and converts it to a string representing 'taskStatus'.
        /// </summary>
        /// <param name="status">integer.</param>
        /// <returns>string (task status).</returns>
        public static string StatusToString(int status)
        {
            switch (status)
            {
                case Active: return "ACTIVE";
                case Waiting: return "WAITING";
                case Ready: return "READY";
                default: return "INCORRECT";
            }
        }

        /// <summary>
        /// <b>AddTaskNamestoCommand</b> method.
        /// Adds 'taskNames' to string array 'taskNameCommands' for use in 'Main' class.
        /// </summary>
        public static void AddTaskNamesToCommand()
        {
            for (int i = 0; i < todoList.Count; i++)
            {
                Program.taskNameCommands[i] = todoList[i].taskName.ToLower().Trim();
            }
        }

        /// <summary>
        /// <b>ReadListFromFile</b> method.
        /// Checks file existence, reads from file and adds new class object to list. 
        /// If no file exists, file creates and app restarts.
        /// </summary>
            // TBD: rework method so it can read list from different .lis files.
        public static void ReadListFromFile()
        {
            string todoFileName = "todo.lis";

            if (File.Exists(todoFileName))
            {
                int numRead = 0;
                string line;

                Console.WriteLine($". Loading file \"/{todoFileName}\".");
                StreamReader sr = new StreamReader(todoFileName);

                while ((line = sr.ReadLine()) != null)
                {
                    TodoItem item = new TodoItem(line);
                    todoList.Add(item);
                    numRead++;
                }

                sr.Close();
                Console.WriteLine($". \"{numRead}\" tasks successfully loaded.\r\n");
            }
            else
            {
                Console.WriteLine($". No file \"{todoFileName}\" found, press any key to create file and restart program.");
                Console.ReadKey(true);

                File.Create(todoFileName);
                Console.WriteLine($". File \"{todoFileName}\" successfully created!\r\n");

                Program.AppRestart();
            }
        }

        /// <summary>
        /// <b>SaveListToFile</b> method.
        /// Checks list count, and writes list to file in dir.
        /// </summary>
            // TBD: rework method so it can write list to different .lis files.
        public static void SaveListToFile()
        {
            string todoFileName = "todo.lis";

            if (todoList.Count > 0)
            {
                Console.WriteLine($". Writing list to file \"/{todoFileName}\".");
                using (StreamWriter sw = new StreamWriter(todoFileName))
                {
                    string listLine;
                    foreach (TodoItem item in todoList)
                    {
                        listLine = $"{item.taskStatus}|{item.taskPriority}|{item.taskName}|{item.taskDescription}";
                        sw.WriteLine(listLine);
                    }
                    Console.WriteLine($". List successfully saved to \"/{todoFileName}\".");
                    sw.Close();
                }
            }
            else
            {
                Console.WriteLine($". List could not be saved to \"/{todoFileName}\", list is empty!");
            }
        }

        /// <summary>
        /// <b>PrintHeadOrFoot</b> method.
        /// Takes two arguments (bools), checks which one is true and prints to console.
        /// If 'head' is true, prints a header to console.
        /// if 'verbose' is true, prints a line demarcator (for task description) to console.
        /// </summary>
        /// <param name="head">bool.</param>
        /// <param name="verbose">bool.</param>
            // TBD: change variable name 'verbose' for ease of understanding.
        private static void PrintHeadOrFoot(bool head, bool verbose)
        {
            if (head)
            {
                Console.Write($"|{"# NAME",-20}|{"STATUS",-12}|{"PRIO",-6}|");

                if (verbose) Console.WriteLine($"{"DESCRIPTION",-40}|");
                else Console.WriteLine();
            }
            Console.Write("|--------------------|------------|------|");

            if (verbose) Console.WriteLine("----------------------------------------|");
            else Console.WriteLine();
        }

        /// <summary>
        /// <b>PrintHead</b> method.
        /// Takes bool argument, calls 'PrintHeadOrFoot' with two bool arguments (head: true).
        /// </summary>
        /// <param name="verbose">bool.</param>
            // TBD: change variable name 'verbose' for ease of understanding.
        public static void PrintHead(bool verbose)
        {
            PrintHeadOrFoot(head: true, verbose);
        }

        /// <summary>
        /// <b>PrintFoot</b> method.
        /// Takes bool argument, calls 'PrintHeadOrFoot' with two bool arguments (head: false).
        /// </summary>
        /// <param name="verbose">bool.</param>
            // TBD: change variable name 'verbose' for ease of understanding.
        public static void PrintFoot(bool verbose)
        {
            PrintHeadOrFoot(head: false, verbose);
        }

        /// <summary>
        /// <b>PrintTodoList</b> method.
        /// Takes two bool agruments (false by default), and calls other print-methods in class.
        /// If 'alltasks' is true, prints all tasks in list. 
        /// If 'alltasks' is not true, only prints active tasks in list.
        /// If 'verbose' is true (false by default), prints a line demarcator for task description.
        /// </summary>
        /// <param name="alltasks">bool (default = false).</param>
        /// <param name="verbose">bool (default = false).</param>
            // TBD: change variable name 'verbose' for ease of understanding.
        public static void PrintTodoList(bool alltasks = false, bool verbose = false)
        {
            PrintHead(verbose);
            if (alltasks)
            {
                foreach (TodoItem item in todoList) item.Print(verbose, todoList.IndexOf(item));
            }
            else
            {
                foreach (TodoItem item in todoList)
                {
                    if (item.taskStatus == Active) item.Print(verbose, todoList.IndexOf(item));
                }
            }
            PrintFoot(verbose);
        }

        /// <summary>
        /// <b>AddNewTask</b> method.
        /// Takes a string argument (null by default), and adds a new class object to list.
        /// </summary>
        /// <param name="taskname">string (default = "").</param>
        public static void AddNewTask(string taskname = "")
        {
            ConsoleKeyInfo keyPressed;
            int newTaskStatus = Waiting;
            int newTaskPriority;
            string? newTaskName;
            string? newTaskDescription;

        setTask:
            if (taskname == "")
            {
                Console.Write(". Set new task name: ");
                newTaskName = Console.ReadLine();
            }
            else
            {
                newTaskName = taskname;
                Console.WriteLine($". New task name set to \"{newTaskName}\".");
            }

            Console.Write(". Set task description: ");
            newTaskDescription = Console.ReadLine();

            newTaskPriority = MyIO.SetIndex(". Set task priority level (1-4): ", 1, 4);

            Console.WriteLine($". Add new task?\r\n");
            Console.WriteLine($"- {"TASK:",-20}{newTaskName}\r\n- {"TASK DESCRIPTION:",-20}{newTaskDescription}\r\n- {"TASK PRIORITY:",-20}{newTaskPriority}\r\n- {"TASK STATUS:",-20}{StatusToString(newTaskStatus)} (Default)\r\n");

        finalTask:
            Console.WriteLine(". Press Enter to add to list, Backspace to redo or Escape to quit new task!");
            {
                keyPressed = Console.ReadKey(true);
                if (keyPressed.Key == ConsoleKey.Enter)
                {
                    string taskLine = $"{newTaskStatus}|{newTaskPriority}|{newTaskName.Trim()}|{newTaskDescription.Trim()}";
                    TodoItem newTask = new TodoItem(taskLine);
                    todoList.Add(newTask);

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

            Program.taskNameCommands = new string[todoList.Count];
            AddTaskNamesToCommand();

        taskDone:;
        }

        /// <summary>
        /// <b>DeleteTask</b> method.
        /// Takes two arguments (bool and string), and deletes a task depending on arguement value.
        /// if bool is true, it clears entire task list.
        /// if bool is false, it checks if string is empty or not and clears a specific task.
        /// </summary>
        /// <param name="alltasks">bool.</param>
        /// <param name="taskname">string (default = "").</param>
            // TBD: change string argument 'taskname' to an integer for 'taskindex' instead.
        public static void DeleteTask(bool alltasks, string taskname = "")
        {
            ConsoleKeyInfo keyPressed;
            int taskIndex;

            if (alltasks)
            {
            startClearAll:
                Console.WriteLine(". Clear entire list? Press Enter to accept, or Escape to quit!");
                keyPressed = Console.ReadKey(true);

                if (keyPressed.Key == ConsoleKey.Enter)
                {
                    File.Delete("todo.lis");

                    Console.WriteLine(". To-Do list successfully cleared! To continue press any key to restart application.");
                    Console.ReadKey(true);

                    File.Create("todo.lis");
                    Program.AppRestart();
                }
                else if (keyPressed.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine(". To-Do list was not cleared!");
                }
                else
                {
                    Console.WriteLine($". Unknown key: {keyPressed.Key}");
                    goto startClearAll;
                }
            }
            else if (alltasks == false && taskname == "")
            {
                PrintTodoList(alltasks: true, verbose: false);

                taskIndex = MyIO.SetIndex(". Which task would you like to delete?: #", 0, todoList.Count - 1);

                taskname = todoList[taskIndex].taskName;
                todoList.RemoveAt(taskIndex);
                
                Console.WriteLine($". Task \"{taskname}\" was successfully deleted! To save changes use \"save\" command.");

                Program.taskNameCommands = new string[todoList.Count];
                AddTaskNamesToCommand();
            }
            else if (alltasks == false && taskname != "")
            {
                try 
                { 
                    taskIndex = todoList.FindIndex(todoItem => todoItem.taskName.ToLower() == taskname.ToLower());
                    todoList.RemoveAt(taskIndex);
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    Console.WriteLine($". ERROR! Index was out of range! Could not find \"{taskname}\" in list.");
                }

                Console.WriteLine($". Task \"{taskname}\" was successfully deleted! To save changes use \"save\" command.");

                Program.taskNameCommands = new string[todoList.Count];
                AddTaskNamesToCommand();
            }
            else
            {
                Console.WriteLine(". No task was deleted.");
            }
        }

        /// <summary>
        /// <b>ChangeTaskStatus</b> method.
        /// Takes two arguments, string (empty by default) and an integer (0 by default).
        /// If no arguments are given it changes status on chosen task from list.
        /// If only string argument is given it changes status on specified task.
        /// if both arguments are given it changes status on specified task to specified status.
        /// </summary>
        /// <param name="taskname">string (default = "").</param>
        /// <param name="taskstatus">int (default = 0).</param>
            // TBD: change string argument 'taskname' to an integer for 'taskindex' instead.
        public static void ChangeTaskStatus(string taskname = "", int taskstatus = 0)
        {
            int taskIndex;
            int setStatus = taskstatus;
            ConsoleKeyInfo keyPressed;

            if (taskname == "")
            {
                PrintTodoList(alltasks: true, verbose: false);

            pickTask:
                taskIndex = MyIO.SetIndex(". Which task would you like to change status on?: #", 0, todoList.Count - 1);
                setStatus = AskTaskStatus(todoList[taskIndex].taskName);

            finalCheck:
                Console.WriteLine($". Finalize status change for \"{todoList[taskIndex].taskName}\" to \"{StatusToString(setStatus)}\"?");
                Console.WriteLine(". Press Enter to accept, Backspace to redo or Escape to quit changes.");

                keyPressed = Console.ReadKey(true);
                if (keyPressed.Key == ConsoleKey.Enter)
                {
                    todoList[taskIndex].taskStatus = setStatus;
                    Console.WriteLine($". Status of \"{todoList[taskIndex].taskName}\" was changed to \"{StatusToString(setStatus)}\"! To save changes use \"save\" command.");
                }
                else if (keyPressed.Key == ConsoleKey.Backspace)
                {
                    setStatus = 0;
                    goto pickTask;
                }
                else if (keyPressed.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine($". Status of \"{todoList[taskIndex].taskName}\" was NOT changed!");
                }
                else goto finalCheck;
            }

            else if (taskname != "")
            {
                try 
                { 
                    taskIndex = todoList.FindIndex(todoItem => todoItem.taskName.ToLower() == taskname.ToLower());

                    if (taskstatus == 0)
                    {
                        setStatus = AskTaskStatus(todoList[taskIndex].taskName);
                    }

                acceptChange:

                    Console.WriteLine($". Change status of \"{todoList[taskIndex].taskName}\" to \"{StatusToString(setStatus)}\"? Press Enter to accept, or Escape to quit!");
                    keyPressed = Console.ReadKey(true);

                    if (keyPressed.Key == ConsoleKey.Enter)
                    {
                        todoList[taskIndex].taskStatus = setStatus;
                        Console.WriteLine($". Status of \"{todoList[taskIndex].taskName}\" was changed to \"{StatusToString(setStatus)}\"! To save changes use \"save\" command.");
                    }
                    else if (keyPressed.Key == ConsoleKey.Escape)
                    {
                        Console.WriteLine($". Status of \"{todoList[taskIndex].taskName}\" was not changed.");
                    }
                    else goto acceptChange;
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    Console.WriteLine($". ERROR! Index was out of range! Could not find \"{taskname}\" in list.");
                }
            }
            else Console.WriteLine(". ERROR! Unable to change status of task!");
        }

        /// <summary>
        /// <b>SetTaskStatus</b> method.
        /// Takes a string argument for a task name, asks what status you want to change task to.
        /// Used in 'ChangeTaskStatus' method.
        /// </summary>
        /// <param name="taskname">string.</param>
        /// <returns>integer (task status).</returns>
        public static int AskTaskStatus(string taskname)
        {
            ConsoleKeyInfo keyPressed;
            int returnInt = 0;

            Console.WriteLine($". Change status of \"{taskname}\" to... (Pick an option, or press Escape to quit!)");
            Console.WriteLine("1. ACTIVE\r\n2. WAITING\r\n3. READY");
        startSetStatus:
            keyPressed = Console.ReadKey(true);
            if (keyPressed.Key == ConsoleKey.D1 || keyPressed.Key == ConsoleKey.NumPad1)
            {
                returnInt = Active;
            }
            else if (keyPressed.Key == ConsoleKey.D2 || keyPressed.Key == ConsoleKey.NumPad2)
            {
                returnInt = Waiting;
            }
            else if (keyPressed.Key == ConsoleKey.D3 || keyPressed.Key == ConsoleKey.NumPad3)
            {
                returnInt = Ready;
            }
            else if (keyPressed.Key == ConsoleKey.Escape)
            {
                Console.WriteLine($". No option chosen, default status \"WAITING\" set!");
                returnInt = Waiting;
            }
            else
            {
                Console.WriteLine($". ERROR! \"{keyPressed.Key}\" not a valid option.");
                goto startSetStatus;
            }
            return returnInt;
        }
    }
}

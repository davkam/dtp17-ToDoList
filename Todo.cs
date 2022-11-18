﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dtp15_todolist
{
    public class Todo
    {
        public static List<TodoItem> todoList = new List<TodoItem>();

        public const int Active = 1;
        public const int Waiting = 2;
        public const int Ready = 3;
        public class TodoItem
        {
            public int taskStatus;
            public int taskPriority;
            public string taskName;
            public string taskDescription;
            public TodoItem(int priority, string task)
            {
                this.taskStatus = Active;
                this.taskPriority = priority;
                this.taskName = task;
                this.taskDescription = "";
            }
            public TodoItem(string todoLine)
            {
                string[] field = todoLine.Split('|');
                taskStatus = Int32.Parse(field[0]);
                taskPriority = Int32.Parse(field[1]);
                taskName = field[2];
                taskDescription = field[3];
            }
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
        public static void AddTaskNamesToCommand()
        {
            for (int i = 0; i < todoList.Count; i++)
            {
                Program.taskNameCommands[i] = todoList[i].taskName.ToLower().Trim();
            }
        }
        public static void ReadListFromFile()
        {
            string todoFileName = "todo.lis";
            if (File.Exists(todoFileName))
            {
                Console.WriteLine($". Loading file \"/{todoFileName}\".");
                StreamReader sr = new StreamReader(todoFileName);
                int numRead = 0;
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    TodoItem item = new TodoItem(line);
                    todoList.Add(item);
                    numRead++;
                }
                sr.Close();
                Console.WriteLine($". \"{numRead}\" tasks successfully loaded.");
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
        public static void PrintHead(bool verbose)
        {
            PrintHeadOrFoot(head: true, verbose);
        }
        public static void PrintFoot(bool verbose)
        {
            PrintHeadOrFoot(head: false, verbose);
        }
        public static void PrintTodoList(bool allTasks = false, bool verbose = false)
        {
            PrintHead(verbose);
            if (allTasks)
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
        public static void AddNewTask(string taskName = "")
        {
            ConsoleKeyInfo keyPressed;
        setTask:
            int newTaskStatus = Waiting;
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
                MyIO.CheckIndexRange(newTaskPriority, 1, 4);
            }
            catch (System.FormatException)
            {
                Console.WriteLine(". ERROR! Input not in correct format!");
                goto setPrio;
            }
            catch (System.Exception)
            {
                Console.WriteLine(". ERROR! Index out of range!");
                goto setPrio;
            }

            Console.WriteLine($". Add new task?\r\n\r\n{"TASK:",-20}{newTaskName}\r\n{"TASK DESCRIPTION:",-20}{newTaskDescription}\r\n{"TASK PRIORITY",-20}{newTaskPriority}\r\n{"TASK STATUS:",-20}{StatusToString(newTaskStatus)} (Default)\r\n");
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
        public static void ChangeTaskStatus(string name = "", int status = 0)
        {
            int taskIndex;
            int setStatus = status;
            ConsoleKeyInfo keyPressed;

            if (name == "")
            {
                PrintTodoList(allTasks: true, verbose: false);

            pickTask:
                Console.Write(". Which task would you like to change status on?: #");
                try
                {
                    taskIndex = Int32.Parse(Console.ReadLine());
                    MyIO.CheckIndexRange(taskIndex, 1, todoList.Count);
                }
                catch (System.FormatException)
                {
                    Console.WriteLine(". ERROR! Input string not in correct format!");
                    goto pickTask;
                }
                catch (System.Exception)
                {
                    Console.WriteLine(". ERROR! Index out of range!");
                    goto pickTask;
                }

                setStatus = SetTaskStatus(todoList[taskIndex - 1].taskName);

            finalCheck:
                Console.WriteLine($". Finalize status change for \"{todoList[taskIndex - 1].taskName}\" to \"{StatusToString(setStatus)}\"?");
                Console.WriteLine(". Press Enter to accept, Backspace to redo or Escape to quit changes.");

                keyPressed = Console.ReadKey(true);
                if (keyPressed.Key == ConsoleKey.Enter)
                {
                    todoList[taskIndex - 1].taskStatus = setStatus;
                    Console.WriteLine($". Status of \"{todoList[taskIndex - 1].taskName}\" was changed to \"{StatusToString(setStatus)}\".");
                }
                else if (keyPressed.Key == ConsoleKey.Backspace)
                {
                    setStatus = 0;
                    goto pickTask;
                }
                else if (keyPressed.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine($". Status of \"{todoList[taskIndex - 1].taskName}\" was NOT changed!");
                }
                else goto finalCheck;
            }

            else if (name != "")
            {
                try 
                { 
                    taskIndex = todoList.FindIndex(todoItem => todoItem.taskName.ToLower() == name.ToLower());

                    if (status == 0)
                    {
                        setStatus = SetTaskStatus(todoList[taskIndex].taskName);
                    }

                acceptChange:

                    Console.WriteLine($". Change status of \"{todoList[taskIndex].taskName}\" to \"{StatusToString(setStatus)}\"? Press Enter to accept, or Escape to quit!");
                    keyPressed = Console.ReadKey(true);

                    if (keyPressed.Key == ConsoleKey.Enter)
                    {
                        todoList[taskIndex].taskStatus = setStatus;
                        Console.WriteLine($". Status of \"{todoList[taskIndex].taskName}\" was changed to \"{StatusToString(setStatus)}\".");
                    }
                    else if (keyPressed.Key == ConsoleKey.Escape)
                    {
                        Console.WriteLine($". Status of \"{todoList[taskIndex].taskName}\" was not changed.");
                    }
                    else goto acceptChange;
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    Console.WriteLine($". ERROR! Index was out of range! Could not find \"{name}\" in list.");
                }
            }
            else Console.WriteLine(". ERROR! Unable to change status of task!");
        }

        public static int SetTaskStatus(string taskName)
        {
            ConsoleKeyInfo keyPressed;
            int returnInt = 0;

            Console.WriteLine($". Change status of \"{taskName}\" to... (Pick an option, or press Escape to quit!)");
            Console.WriteLine("1. ACTIVE\r\n2. WAITING\r\n3. READY");

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
            }
            return returnInt;
        }
    }
}

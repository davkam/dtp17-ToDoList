using System;
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
                Console.Write($"|{(i+1) + ". " + taskName,-20}|{statusString,-12}|{taskPriority,-6}|");
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
                case Active: return "Active";
                case Waiting: return "Waiting";
                case Ready: return "Ready";
                default: return "INCORRECT";
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
        private static void PrintHead(bool verbose)
        {
            PrintHeadOrFoot(head: true, verbose);
        }
        private static void PrintFoot(bool verbose)
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
    }
}

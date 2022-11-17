using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dtp15_todolist
{
    public class Todo
    {
        public static List<TodoItem> list = new List<TodoItem>();

        public const int Active = 1;
        public const int Waiting = 2;
        public const int Ready = 3;
        public class TodoItem
        {
            public int status;
            public int priority;
            public string task;
            public string taskDescription;
            public TodoItem(int priority, string task)
            {
                this.status = Active;
                this.priority = priority;
                this.task = task;
                this.taskDescription = "";
            }
            public TodoItem(string todoLine)
            {
                string[] field = todoLine.Split('|');
                status = Int32.Parse(field[0]);
                priority = Int32.Parse(field[1]);
                task = field[2];
                taskDescription = field[3];
            }
            public void Print(bool verbose = false)
            {
                string statusString = StatusToString(status);
                Console.Write($"|{statusString,-12}|{priority,-6}|{task,-20}|");
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
                    list.Add(item);
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
                MainClass.AppRestart();
            }
        }
        public static void SaveListToFile()
        {
            string todoFileName = "todo.lis";
            if (list.Count > 0)
            {
                Console.WriteLine($". Writing list to file \"/{todoFileName}\".");
                using (StreamWriter sw = new StreamWriter(todoFileName))
                {
                    string listLine;
                    foreach (TodoItem item in list)
                    {
                        listLine = $"{item.status}|{item.priority}|{item.task}|{item.taskDescription}";
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
                Console.Write("|STATUS      |PRIO  |NAME                |");
                if (verbose) Console.WriteLine("DESCRIPTION                             |");
                else Console.WriteLine();
            }
            Console.Write("|------------|------|--------------------|");
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
        public static void PrintTodoList(bool verbose = false)
        {
            PrintHead(verbose);
            foreach (TodoItem item in list)
            {
                item.Print(verbose);
            }
            PrintFoot(verbose);
        }
    }
}

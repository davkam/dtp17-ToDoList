namespace dtp15_todolist
{
    class MainClass
    {
        public static void PrintHelp()
        {
            Console.WriteLine(".................COMMANDS.................\r\n");
            Console.WriteLine("- help       List all commands.");
            Console.WriteLine("- list       List all active tasks in to-do list.");
            Console.WriteLine("- quit       Quit and save to-do list");
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
                else if (MyIO.Equals(command, "quit"))
                {
                    Console.WriteLine(". Thank you for using To-Do List!");
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

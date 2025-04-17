using System;

namespace PA5
{
    class Program
    {
        static void Main(string[] args)
        {
            bool running = true;

            while (running)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Welcome to Papa's Pizzeria!");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Are you a manager or customer?");
                Console.WriteLine("Please enter \"1\" for Manager, \"2\" for Customer, or \"3\" to Exit:");

                Console.ForegroundColor = ConsoleColor.White;
                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Manager manager = new Manager();
                    manager.ManagerMenu();
                }
                else if (choice == "2")
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Customer customer = new Customer();
                    customer.CustomerMenu();
                }
                else if (choice == "3")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Thank you for visiting Papa's Pizzeria! Goodbye!");
                    running = false;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Invalid choice. Please try again.");
                }

                Console.ResetColor();
            }
        }
    }
}
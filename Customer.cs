using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PA5
{
    public class Customer
    {
        private string email;
        const int MAX_PIZZAS = 100;
        string[] pizzaNames = new string[MAX_PIZZAS];
        int[] pizzaToppings = new int[MAX_PIZZAS];
        string[] pizzaCrustSizes = new string[MAX_PIZZAS];
        double[] pizzaPrices = new double[MAX_PIZZAS];
        string[] isSoldOut = new string[MAX_PIZZAS];

        private string[] FindPizzaById(int id)
        {
            string[] menu = File.ReadAllLines("pizza-menu.txt");
            var sortedMenu = menu.Select(line => line.Split('#'))
                               .OrderBy(p => int.Parse(p[0]))
                               .ToArray();
            
            int left = 0, right = sortedMenu.Length - 1;
            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                int midId = int.Parse(sortedMenu[mid][0]);
                if (midId == id) return sortedMenu[mid];
                else if (midId < id) left = mid + 1;
                else right = mid - 1;
            }
            return null;
        }

        private void ViewLoyaltyStatus()
        {
            int orderCount = File.ReadAllLines("orders.txt")
                                .Count(line => line.Split('#')[1].Equals(email, StringComparison.OrdinalIgnoreCase));
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"📊 Loyalty Status: {orderCount} orders | {(orderCount >= 3 ? "🌟 Gold Tier" : "🔹 Regular Tier")}");
        }

        public void CustomerMenu()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("What is your email?");
            Console.ForegroundColor = ConsoleColor.White;
            email = Console.ReadLine();
            bool running = true;

            while (running)
            {
                Thread.Sleep(1500);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Welcome to the Customer Menu! Please select an option below.");
                Console.WriteLine("1. View the menu");
                Console.WriteLine("2. Place an order");
                Console.WriteLine("3. View past orders");
                Console.WriteLine("4. View loyalty status");
                Console.WriteLine("5. Exit");

                Console.ForegroundColor = ConsoleColor.White;
                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    ViewMenu();
                }
                else if (choice == "2")
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    OrderPizza();
                }
                else if (choice == "3")
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    PastOrders();
                }
                else if (choice == "4")
                {
                    ViewLoyaltyStatus();
                }
                else if (choice == "5")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    ExitCustomerMenu();
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

        private int ReadMenu(string[] pizzaNames, int[] pizzaToppings, string[] pizzaCrustSizes, double[] pizzaPrices, string[] pizzaTypes)
        {
            int count = 0;
            try
            {
                StreamReader inFile = new StreamReader("pizza-menu.txt");
                string line = inFile.ReadLine();
                while (line != null && count < MAX_PIZZAS)
                {
                    string[] temp = line.Split('#');
                    pizzaNames[count] = temp[1];
                    pizzaToppings[count] = int.Parse(temp[2]);
                    pizzaCrustSizes[count] = temp[3];
                    pizzaPrices[count] = double.Parse(temp[4]);
                    isSoldOut[count] = temp[5];

                    line = inFile.ReadLine();
                    count++;
                }
                inFile.Close();
            }
            catch (FileNotFoundException)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Error: The pizza menu file was not found.");
                Console.ResetColor();
            }
            catch (FormatException ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"Error: Invalid data format in the pizza menu file. {ex.Message}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.ResetColor();
            }
            return count;
        }

        public void ViewMenu()
        {
            int count = ReadMenu(pizzaNames, pizzaToppings, pizzaCrustSizes, pizzaPrices, isSoldOut);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("ID\t🍕 Name\t\tToppings\tCrust\t\tPrice\tSold Out");
            for (int i = 0; i < count; i++)
            {
                string pizzaEmoji = pizzaNames[i].Contains("Pepperoni") ? "🍖" : 
                                  pizzaNames[i].Contains("Veggie") ? "🥦" : "🍕";
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"{i}\t{pizzaEmoji} {pizzaNames[i]}\t{pizzaToppings[i]}\t{pizzaCrustSizes[i]}\t${pizzaPrices[i]:F2}\t{isSoldOut[i]}");
            }
            Console.ResetColor();
        }

        private void OrderPizza()
        {
            string[] menu = File.ReadAllLines("pizza-menu.txt");
            string[] orders = File.ReadAllLines("orders.txt");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("What is the ID of the pizza you want?");
            Console.ForegroundColor = ConsoleColor.White;
            int pizzaID = int.Parse(Console.ReadLine());
            
            string[] pizza = FindPizzaById(pizzaID);
            if (pizza == null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Invalid ID. Please try again.");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("What size pizza would you like? (Options: 8, 12, 16)");
            Console.ForegroundColor = ConsoleColor.White;
            int size = int.Parse(Console.ReadLine());
            if (size != 8 && size != 12 && size != 16)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Invalid size. Please try again.");
                Console.ResetColor();
                return;
            }

            string status = "Pending";
            int newOrderID = orders.Length;
            string orderDate = DateTime.Now.ToString("MM/dd/yyyy");
            string newOrder = $"{newOrderID}#{email}#{pizzaID}#{orderDate}#{size}#{status}";
            File.AppendAllText("orders.txt", newOrder + Environment.NewLine);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Order placed successfully!");
            Console.ResetColor();
        }

        private void PastOrders()
        {
            string[] orders = File.ReadAllLines("orders.txt");
            var matchingOrders = orders.Where(order => order.Split('#')[1].Equals(email, StringComparison.OrdinalIgnoreCase)).ToList();
            if (matchingOrders.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Here are your past orders:");
                foreach (string order in matchingOrders)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(order);
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("No past orders found for this email.");
            }
            Console.ResetColor();
        }

        private void ExitCustomerMenu()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Exiting customer menu");
            Console.ResetColor();
        }
    }
}
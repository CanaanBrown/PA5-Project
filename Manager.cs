using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace PA5
{
    public class Manager
    {
        public void ManagerMenu()
        {
            bool running = true;

            while (running)
            {
                try
                {
                    Thread.Sleep(1500);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Welcome to the Manager Menu! Please select an option below.");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("1. Add a pizza");
                    Console.WriteLine("2. Remove a pizza");
                    Console.WriteLine("3. Edit a pizza");
                    Console.WriteLine("4. Mark a pizza as complete");
                    Console.WriteLine("5. Access the report menu");
                    Console.WriteLine("6. Exit");

                    Console.ForegroundColor = ConsoleColor.White;
                    string choice = Console.ReadLine();

                    if (choice == "1")
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        AddPizza();
                    }
                    else if (choice == "2")
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        RemovePizza();
                    }
                    else if (choice == "3")
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        EditPizza();
                    }
                    else if (choice == "4")
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        CompletePizza();
                    }
                    else if (choice == "5")
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Reports();
                    }
                    else if (choice == "6")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        ExitManagerMenu();
                        running = false;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Invalid choice. Please try again.");
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
                finally
                {
                    Console.ResetColor();
                }
            }
        }

        private void AddPizza()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Please enter the name of the pizza you would like to add.");
                Console.ForegroundColor = ConsoleColor.White;
                string name = Console.ReadLine();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("How many toppings does the pizza have?");
                Console.ForegroundColor = ConsoleColor.White;
                int toppings = int.Parse(Console.ReadLine());

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Please enter the crust size of the pizza (Options: Thin, thick, stuffed, gluten-free).");
                Console.ForegroundColor = ConsoleColor.White;
                string crustSize = Console.ReadLine().ToLower();
                if (crustSize != "thin" && crustSize != "thick" && crustSize != "stuffed" && crustSize != "gluten-free")
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Invalid crust size. Defaulting to 'Thin'.");
                    Console.ResetColor();
                    crustSize = "Thin";
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Please enter the price of the pizza.");
                Console.ForegroundColor = ConsoleColor.White;
                double price = Convert.ToDouble(Console.ReadLine());

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Is this pizza sold out? (y/n)");
                Console.ForegroundColor = ConsoleColor.White;
                string soldOut;
                if (Console.ReadLine().ToLower() == "y")
                {
                    soldOut = "Yes";
                }
                else if (Console.ReadLine().ToLower() == "n")
                {
                    soldOut = "No";
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Invalid input. Defaulting to 'No'.");
                    Console.ResetColor();
                    soldOut = "No";
                }

                string[] menu = File.ReadAllLines("pizza-menu.txt");
                int newId = menu.Length;
                string newPizza = $"{newId}#{name}#{toppings}#{crustSize}#{price:F2}#{soldOut}";
                File.AppendAllText("pizza-menu.txt", newPizza + Environment.NewLine);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Pizza added successfully!");
            }
            catch (FormatException)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Invalid input. Please enter the correct format.");
            }
            catch (IOException ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"File error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            finally
            {
                Console.ResetColor();
            }
        }

        private void RemovePizza()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Enter the ID of the pizza you want to remove:");
                Console.ForegroundColor = ConsoleColor.White;
                int idToRemove = int.Parse(Console.ReadLine());

                string[] menu = File.ReadAllLines("pizza-menu.txt");
                if (idToRemove >= 0 && idToRemove < menu.Length)
                {
                    menu[idToRemove] = null;
                    File.WriteAllLines("pizza-menu.txt", menu.Where(line => line != null).ToArray());
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Pizza removed successfully!");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Invalid ID. No pizza removed.");
                }
            }
            catch (FormatException)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Invalid input. Please enter a valid pizza ID.");
            }
            catch (IOException ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"File error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            finally
            {
                Console.ResetColor();
            }
        }

        private void EditPizza()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Enter the ID of the pizza you want to edit:");
                Console.ForegroundColor = ConsoleColor.White;
                int idToEdit = int.Parse(Console.ReadLine());

                string[] menu = File.ReadAllLines("pizza-menu.txt");
                if (idToEdit >= 0 && idToEdit < menu.Length)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Enter the new name of the pizza.");
                    Console.ForegroundColor = ConsoleColor.White;
                    string name = Console.ReadLine();

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Enter the new number of toppings.");
                    Console.ForegroundColor = ConsoleColor.White;
                    int toppings = int.Parse(Console.ReadLine());

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Enter the new crust size (Options: Thin, thick, stuffed, gluten-free).");
                    Console.ForegroundColor = ConsoleColor.White;
                    string crustSize = Console.ReadLine();
                    if (crustSize != "thin" && crustSize != "thick" && crustSize != "stuffed" && crustSize != "gluten-free")
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Invalid crust size. Defaulting to 'Thin'.");
                        Console.ResetColor();
                        crustSize = "Thin";
                    }

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Enter the new price.");
                    Console.ForegroundColor = ConsoleColor.White;
                    double price = Convert.ToDouble(Console.ReadLine());

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Is this pizza sold out? (y/n)");
                    Console.ForegroundColor = ConsoleColor.White;
                    string soldOut;
                    if (Console.ReadLine().ToLower() == "y")
                    {
                        soldOut = "Yes";
                    }
                    else if (Console.ReadLine().ToLower() == "n")
                    {
                        soldOut = "No";
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Invalid input. Defaulting to 'No'.");
                        Console.ResetColor();
                        soldOut = "No";
                    }

                    string newPizza = $"{idToEdit}#{name}#{toppings}#{crustSize}#{price:F2}#{soldOut}";
                    menu[idToEdit] = newPizza;
                    File.WriteAllLines("pizza-menu.txt", menu);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Pizza edited successfully!");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Invalid ID. No pizza edited.");
                }
            }
            catch (FormatException)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Invalid input. Please enter the correct format.");
            }
            catch (IOException ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"File error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            finally
            {
                Console.ResetColor();
            }
        }

        private void CompletePizza()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("What is the order ID of the pizza you want to mark as complete?");
                Console.ForegroundColor = ConsoleColor.White;
                int orderID = int.Parse(Console.ReadLine());

                string[] orders = File.ReadAllLines("orders.txt");
                if (orderID >= 0 && orderID < orders.Length)
                {
                    string[] order = orders[orderID].Split('#');
                    order[5] = "Complete";
                    orders[orderID] = string.Join("#", order);
                    File.WriteAllLines("orders.txt", orders);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Pizza marked as complete successfully!");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Invalid order ID. No pizza marked as complete.");
                }
            }
            catch (FormatException)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Invalid input. Please enter a valid order ID.");
            }
            catch (IOException ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"File error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            finally
            {
                Console.ResetColor();
            }
        }

        private void ExitManagerMenu()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Exiting Manager Menu...");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"An unexpected error occurred while exiting: {ex.Message}");
            }
            finally
            {
                Console.ResetColor();
            }
        }
        private void Reports()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Welcome to the Reports Menu! Please select an option below.");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("1. View today's orders");
                Console.WriteLine("2. View orders currently in progress");
                Console.WriteLine("3. View average pizza size by crust type");
                Console.WriteLine("4. View top 5 most popular pizzas");
                Console.WriteLine("5. Exit");

                Console.ForegroundColor = ConsoleColor.White;
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewTodaysOrders();
                        break;
                    case "2":
                        ViewInProgressOrders();
                        break;
                    case "3":
                        ViewAveragePizzaSize();
                        break;
                    case "4":
                        ViewTop5Pizzas();
                        break;
                    case "5":
                        ExitManagerMenu();
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"An error occurred in the Reports menu: {ex.Message}");
            }
            finally
            {
                Console.ResetColor();
            }
        }

        private void ViewTodaysOrders()
        {
            try
            {
                string[] orders = File.ReadAllLines("orders.txt");
                bool foundOrders = false;

                foreach (string order in orders)
                {
                    string[] orderDetails = order.Split('#');
                    if (orderDetails[3] == DateTime.Today.ToString("MM/dd/yyyy"))
                    {
                        Console.WriteLine(order);
                        foundOrders = true;
                    }
                }

                if (!foundOrders)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("No orders found for today.");
                }
            }
            catch (IOException ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"File error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            finally
            {
                Console.ResetColor();
            }
        }

        private void ViewInProgressOrders()
        {
            try
            {
                if (!File.Exists("orders.txt"))
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("The orders file does not exist.");
                    return;
                }

                string[] orders = File.ReadAllLines("orders.txt");
                bool foundOrders = false;

                foreach (string order in orders)
                {
                    string[] orderDetails = order.Split('#');
                    if (orderDetails.Length > 5 && orderDetails[5].Trim().Equals("Pending", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine(order);
                        foundOrders = true;
                    }
                }

                if (!foundOrders)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("No orders currently in progress.");
                }
            }
            catch (IOException ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"File error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            finally
            {
                Console.ResetColor();
            }
        }

        private void ViewAveragePizzaSize()
        {
            try
            {
                if (!File.Exists("orders.txt"))
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("The orders file does not exist.");
                    return;
                }

                string[] orders = File.ReadAllLines("orders.txt");
                Dictionary<string, List<int>> crustSizes = new Dictionary<string, List<int>>();

                foreach (string order in orders)
                {
                    string[] orderDetails = order.Split('#');
                    if (orderDetails.Length > 4)
                    {
                        string crustSize = orderDetails[3];
                        int size = int.Parse(orderDetails[4]);

                        if (!crustSizes.ContainsKey(crustSize))
                        {
                            crustSizes.Add(crustSize, new List<int>());
                        }
                        crustSizes[crustSize].Add(size);
                    }
                }

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Average Pizza Size by Crust Type:");
                foreach (var crust in crustSizes)
                {
                    double averageSize = crust.Value.Average();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"{crust.Key}: {averageSize:F2} inches");
                }
            }
            catch (FormatException)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Invalid data format in the orders file.");
            }
            catch (IOException ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"File error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            finally
            {
                Console.ResetColor();
            }
        }

        private void ViewTop5Pizzas()
        {
            try
            {
                if (!File.Exists("orders.txt"))
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("The orders file does not exist.");
                    return;
                }

                if (!File.Exists("pizza-menu.txt"))
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("The pizza menu file does not exist.");
                    return;
                }

                string[] orders = File.ReadAllLines("orders.txt");
                Dictionary<int, int> pizzaCounts = new Dictionary<int, int>();

                foreach (string order in orders)
                {
                    string[] orderDetails = order.Split('#');
                    if (orderDetails.Length > 2)
                    {
                        int pizzaID = int.Parse(orderDetails[2]);

                        if (!pizzaCounts.ContainsKey(pizzaID))
                        {
                            pizzaCounts.Add(pizzaID, 0);
                        }
                        pizzaCounts[pizzaID]++;
                    }
                }

                var top5Pizzas = pizzaCounts.OrderByDescending(pair => pair.Value).Take(5);
                string[] menu = File.ReadAllLines("pizza-menu.txt");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Top 5 Most Popular Pizzas:");
                foreach (var pizza in top5Pizzas)
                {
                    if (pizza.Key < menu.Length)
                    {
                        string[] pizzaDetails = menu[pizza.Key].Split('#');
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($"{pizzaDetails[1]}: {pizza.Value} orders");
                    }
                }
            }
            catch (FormatException)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Invalid data format in the orders or menu file.");
            }
            catch (IOException ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"File error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            finally
            {
                Console.ResetColor();
            }
        }
    }
}
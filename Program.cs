using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;

namespace PersonalExpenseTracker
{
    public class Expense
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return $"ID: {Id} | Amount: ${Amount:F2} | Category: {Category} | Date: {Date:yyyy-MM-dd} | Description: {Description}";
        }

        public string ToFileString()
        {
            // Escape pipe characters in data to prevent parsing issues
            var escapedCategory = Category?.Replace("|", "&#124;") ?? "";
            var escapedDescription = Description?.Replace("|", "&#124;") ?? "";
            return $"{Id}|{Amount}|{escapedCategory}|{Date:yyyy-MM-dd}|{escapedDescription}";
        }

        public static Expense FromFileString(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return null;

            var parts = line.Split('|');
            if (parts.Length != 5) return null;

            try
            {
                // Unescape pipe characters
                var category = parts[2].Replace("&#124;", "|");
                var description = parts[4].Replace("&#124;", "|");

                return new Expense
                {
                    Id = int.Parse(parts[0]),
                    Amount = decimal.Parse(parts[1], CultureInfo.InvariantCulture),
                    Category = category,
                    Date = DateTime.ParseExact(parts[3], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Description = description
                };
            }
            catch
            {
                // Return null if parsing fails for any field
                return null;
            }
        }
    }

    public class ExpenseTracker
    {
        private List<Expense> expenses;
        private int nextId;
        private const string DATA_FILE = "expenses.txt";

        public ExpenseTracker()
        {
            expenses = new List<Expense>();
            LoadData();
        }

        public void AddExpense(decimal amount, string category, DateTime date, string description)
        {
            // Validate inputs
            if (amount <= 0)
            {
                Console.WriteLine("Amount must be greater than zero.");
                return;
            }

            if (string.IsNullOrWhiteSpace(category))
            {
                Console.WriteLine("Category cannot be empty.");
                return;
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                Console.WriteLine("Description cannot be empty.");
                return;
            }

            var expense = new Expense
            {
                Id = nextId++,
                Amount = amount,
                Category = category.Trim(),
                Date = date,
                Description = description.Trim()
            };

            expenses.Add(expense);
            SaveData();
            Console.WriteLine($"Expense added successfully! ID: {expense.Id}");
        }

        public void UpdateExpense(int id, decimal? amount = null, string category = null, DateTime? date = null, string description = null)
        {
            var expense = expenses.FirstOrDefault(e => e.Id == id);
            if (expense == null)
            {
                Console.WriteLine($"Expense with ID {id} not found.");
                return;
            }

            // Validate amount if provided
            if (amount.HasValue && amount.Value <= 0)
            {
                Console.WriteLine("Amount must be greater than zero.");
                return;
            }

            if (amount.HasValue) expense.Amount = amount.Value;
            if (!string.IsNullOrWhiteSpace(category)) expense.Category = category.Trim();
            if (date.HasValue) expense.Date = date.Value;
            if (!string.IsNullOrWhiteSpace(description)) expense.Description = description.Trim();

            SaveData();
            Console.WriteLine("Expense updated successfully!");
        }

        public void DeleteExpense(int id)
        {
            var expense = expenses.FirstOrDefault(e => e.Id == id);
            if (expense == null)
            {
                Console.WriteLine($"Expense with ID {id} not found.");
                return;
            }

            expenses.Remove(expense);
            SaveData();
            Console.WriteLine("Expense deleted successfully!");
        }

        public void ListAllExpenses()
        {
            if (!expenses.Any())
            {
                Console.WriteLine("No expenses found.");
                return;
            }

            Console.WriteLine("\n=== All Expenses ===");
            foreach (var expense in expenses.OrderBy(e => e.Date))
            {
                Console.WriteLine(expense);
            }

            var total = expenses.Sum(e => e.Amount);
            Console.WriteLine($"\nTotal Amount: ${total:F2}");
        }

        public void FilterByCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                Console.WriteLine("Category cannot be empty.");
                return;
            }

            var filtered = expenses.Where(e => e.Category.Equals(category.Trim(), StringComparison.OrdinalIgnoreCase)).ToList();

            if (!filtered.Any())
            {
                Console.WriteLine($"No expenses found in category '{category}'.");
                return;
            }

            Console.WriteLine($"\n=== Expenses in Category: {category} ===");
            foreach (var expense in filtered.OrderBy(e => e.Date))
            {
                Console.WriteLine(expense);
            }

            var total = filtered.Sum(e => e.Amount);
            Console.WriteLine($"\nCategory Total: ${total:F2}");
        }

        public void FilterByDateRange(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                Console.WriteLine("Start date cannot be later than end date.");
                return;
            }

            var filtered = expenses.Where(e => e.Date >= startDate && e.Date <= endDate).ToList();

            if (!filtered.Any())
            {
                Console.WriteLine($"No expenses found between {startDate:yyyy-MM-dd} and {endDate:yyyy-MM-dd}.");
                return;
            }

            Console.WriteLine($"\n=== Expenses from {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd} ===");
            foreach (var expense in filtered.OrderBy(e => e.Date))
            {
                Console.WriteLine(expense);
            }

            var total = filtered.Sum(e => e.Amount);
            Console.WriteLine($"\nDate Range Total: ${total:F2}");
        }

        public void ShowHighestAndLowestExpense()
        {
            if (!expenses.Any())
            {
                Console.WriteLine("No expenses found.");
                return;
            }

            var highest = expenses.OrderByDescending(e => e.Amount).First();
            var lowest = expenses.OrderBy(e => e.Amount).First();

            Console.WriteLine("\n=== Expense Statistics ===");
            Console.WriteLine($"Highest Expense: {highest}");
            Console.WriteLine($"Lowest Expense: {lowest}");
        }

        public List<string> GetCategories()
        {
            return expenses.Select(e => e.Category).Distinct().OrderBy(c => c).ToList();
        }

        private void SaveData()
        {
            try
            {
                var lines = expenses.Select(e => e.ToFileString());
                File.WriteAllLines(DATA_FILE, lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }

        private void LoadData()
        {
            try
            {
                if (File.Exists(DATA_FILE))
                {
                    var lines = File.ReadAllLines(DATA_FILE);
                    var loadedCount = 0;

                    foreach (var line in lines)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            var expense = Expense.FromFileString(line);
                            if (expense != null)
                            {
                                expenses.Add(expense);
                                loadedCount++;
                                // Ensure nextId is always higher than existing IDs
                                if (expense.Id >= nextId)
                                    nextId = expense.Id + 1;
                            }
                            else
                            {
                                Console.WriteLine($"Warning: Could not parse line: {line}");
                            }
                        }
                    }

                    if (loadedCount > 0)
                        Console.WriteLine($"Loaded {loadedCount} expenses from file.");
                }
                else
                {
                    nextId = 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
                nextId = 1;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var tracker = new ExpenseTracker();
            bool running = true;

            Console.WriteLine("=== Personal Expense Tracker ===");

            while (running)
            {
                ShowMenu();
                var choice = Console.ReadLine()?.Trim();

                // Clear screen after getting input but before processing
                Console.Clear();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("=== ADD EXPENSE ===");
                        AddExpenseMenu(tracker);
                        break;
                    case "2":
                        Console.WriteLine("=== UPDATE EXPENSE ===");
                        UpdateExpenseMenu(tracker);
                        break;
                    case "3":
                        Console.WriteLine("=== DELETE EXPENSE ===");
                        DeleteExpenseMenu(tracker);
                        break;
                    case "4":
                        tracker.ListAllExpenses();
                        break;
                    case "5":
                        Console.WriteLine("=== FILTER BY CATEGORY ===");
                        FilterByCategoryMenu(tracker);
                        break;
                    case "6":
                        Console.WriteLine("=== FILTER BY DATE RANGE ===");
                        FilterByDateRangeMenu(tracker);
                        break;
                    case "7":
                        tracker.ShowHighestAndLowestExpense();
                        break;
                    case "8":
                        running = false;
                        Console.WriteLine("Thank you for using Personal Expense Tracker!");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                if (running)
                {
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        static void ShowMenu()
        {
            Console.WriteLine("\n=== MENU ===");
            Console.WriteLine("1. Add Expense");
            Console.WriteLine("2. Update Expense");
            Console.WriteLine("3. Delete Expense");
            Console.WriteLine("4. List All Expenses");
            Console.WriteLine("5. Filter by Category");
            Console.WriteLine("6. Filter by Date Range");
            Console.WriteLine("7. Show Highest/Lowest Expense");
            Console.WriteLine("8. Exit");
            Console.Write("Choose an option (1-8): ");
        }

        static void AddExpenseMenu(ExpenseTracker tracker)
        {
            try
            {
                Console.Write("Enter amount: $");
                var amountInput = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(amountInput) || !decimal.TryParse(amountInput, out decimal amount) || amount <= 0)
                {
                    Console.WriteLine("Invalid amount. Please enter a positive number.");
                    return;
                }

                Console.Write("Enter category: ");
                string category = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(category))
                {
                    Console.WriteLine("Category cannot be empty.");
                    return;
                }

                Console.Write("Enter date (yyyy-mm-dd) or press Enter for today: ");
                string dateInput = Console.ReadLine()?.Trim();
                DateTime date;

                if (string.IsNullOrEmpty(dateInput))
                {
                    date = DateTime.Today;
                }
                else if (!DateTime.TryParseExact(dateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    Console.WriteLine("Invalid date format. Please use yyyy-mm-dd.");
                    return;
                }

                Console.Write("Enter description: ");
                string description = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(description))
                {
                    Console.WriteLine("Description cannot be empty.");
                    return;
                }

                tracker.AddExpense(amount, category, date, description);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding expense: {ex.Message}");
            }
        }

        static void UpdateExpenseMenu(ExpenseTracker tracker)
        {
            tracker.ListAllExpenses();

            Console.Write("\nEnter expense ID to update: ");
            var idInput = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(idInput) || !int.TryParse(idInput, out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            Console.WriteLine("Leave blank to keep current value:");

            Console.Write("Enter new amount: $");
            string amountInput = Console.ReadLine()?.Trim();
            decimal? amount = null;
            if (!string.IsNullOrEmpty(amountInput))
            {
                if (decimal.TryParse(amountInput, out decimal amt) && amt > 0)
                    amount = amt;
                else
                {
                    Console.WriteLine("Invalid amount. Update cancelled.");
                    return;
                }
            }

            Console.Write("Enter new category: ");
            string category = Console.ReadLine()?.Trim();

            Console.Write("Enter new date (yyyy-mm-dd): ");
            string dateInput = Console.ReadLine()?.Trim();
            DateTime? date = null;
            if (!string.IsNullOrEmpty(dateInput))
            {
                if (DateTime.TryParseExact(dateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
                    date = dt;
                else
                {
                    Console.WriteLine("Invalid date format. Update cancelled.");
                    return;
                }
            }

            Console.Write("Enter new description: ");
            string description = Console.ReadLine()?.Trim();

            tracker.UpdateExpense(id, amount, category, date, description);
        }

        static void DeleteExpenseMenu(ExpenseTracker tracker)
        {
            tracker.ListAllExpenses();

            Console.Write("\nEnter expense ID to delete: ");
            var idInput = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(idInput) || !int.TryParse(idInput, out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            Console.Write("Are you sure you want to delete this expense? (y/N): ");
            string confirm = Console.ReadLine()?.Trim().ToLower();

            if (confirm == "y" || confirm == "yes")
            {
                tracker.DeleteExpense(id);
            }
            else
            {
                Console.WriteLine("Delete cancelled.");
            }
        }

        static void FilterByCategoryMenu(ExpenseTracker tracker)
        {
            var categories = tracker.GetCategories();
            if (!categories.Any())
            {
                Console.WriteLine("No categories found.");
                return;
            }

            Console.WriteLine("Available categories:");
            for (int i = 0; i < categories.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {categories[i]}");
            }

            Console.Write("Enter category name or number: ");
            string input = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Invalid category selection.");
                return;
            }

            string category = null;
            if (int.TryParse(input, out int choice) && choice > 0 && choice <= categories.Count)
            {
                category = categories[choice - 1];
            }
            else
            {
                category = input;
            }

            tracker.FilterByCategory(category);
        }

        static void FilterByDateRangeMenu(ExpenseTracker tracker)
        {
            Console.Write("Enter start date (yyyy-mm-dd): ");
            var startInput = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(startInput) || !DateTime.TryParseExact(startInput, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate))
            {
                Console.WriteLine("Invalid start date format.");
                return;
            }

            Console.Write("Enter end date (yyyy-mm-dd): ");
            var endInput = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(endInput) || !DateTime.TryParseExact(endInput, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endDate))
            {
                Console.WriteLine("Invalid end date format.");
                return;
            }

            if (startDate > endDate)
            {
                Console.WriteLine("Start date cannot be later than end date.");
                return;
            }

            tracker.FilterByDateRange(startDate, endDate);
        }
    }
}
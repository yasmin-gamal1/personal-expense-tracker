# Personal Expense Tracker 💰

A comprehensive console-based personal expense tracking application written in C# that helps you manage and analyze your daily expenses with ease.

## 🌟 Features

- ✅ **Add Expenses**: Record expenses with amount, category, date, and description
- ✅ **Update Expenses**: Modify existing expense details
- ✅ **Delete Expenses**: Remove expenses with confirmation prompt
- ✅ **List All Expenses**: View all expenses sorted by date with total calculations
- ✅ **Filter by Category**: View expenses within specific categories
- ✅ **Filter by Date Range**: Analyze expenses between specific dates
- ✅ **Expense Statistics**: View highest and lowest expenses
- ✅ **Data Persistence**: Automatic saving to local file storage
- ✅ **Input Validation**: Robust error handling and data validation
- ✅ **User-Friendly Interface**: Clean, intuitive menu-driven interface

## 🛠️ Technologies Used

- **Language**: C#
- **Framework**: .NET Core/.NET Framework
- **Data Storage**: File-based (expenses.txt)
- **Architecture**: Object-Oriented Programming

## 📋 Prerequisites

- .NET Core 3.1 or later / .NET Framework 4.7.2 or later
- Any C# IDE (Visual Studio, VS Code, Rider) or command line compiler

## 🚀 Getting Started

### Installation

1. **Clone the repository**:
   ```bash
   git clone https://github.com/yasmin-gamal1/personal-expense-tracker.git
   cd personal-expense-tracker
   ```

2. **Using .NET CLI**:
   ```bash
   dotnet run
   ```

3. **Using Visual Studio**:
   - Open the solution file (.sln)
   - Press F5 or click "Start" button

4. **Using Command Line**:
   ```bash
   csc Program.cs
   Program.exe
   ```

## 📱 How to Use

### Main Menu Options:

1. **Add Expense** 
   - Enter amount (positive number)
   - Specify category (e.g., Food, Transportation, Entertainment)
   - Set date (YYYY-MM-DD format or press Enter for today)
   - Add description

2. **Update Expense**
   - View list of all expenses
   - Enter expense ID to modify
   - Update any field (leave blank to keep current value)

3. **Delete Expense**
   - View list of all expenses
   - Enter expense ID to delete
   - Confirm deletion

4. **List All Expenses**
   - Displays all expenses sorted by date
   - Shows total amount spent

5. **Filter by Category**
   - Choose from existing categories or enter custom category
   - View category-specific expenses and totals

6. **Filter by Date Range**
   - Enter start and end dates
   - View expenses within the specified period

7. **Show Statistics**
   - View highest and lowest expense records

8. **Exit**
   - Safely close the application

### Sample Usage:

```
=== MENU ===
1. Add Expense
2. Update Expense
3. Delete Expense
4. List All Expenses
5. Filter by Category
6. Filter by Date Range
7. Show Highest/Lowest Expense
8. Exit
Choose an option (1-8): 1

=== ADD EXPENSE ===
Enter amount: $25.50
Enter category: Food
Enter date (yyyy-mm-dd) or press Enter for today: 2024-03-15
Enter description: Lunch at restaurant
Expense added successfully! ID: 1
```

## 📂 Project Structure

```
PersonalExpenseTracker/
├── Program.cs              # Main application code
├── expenses.txt            # Data storage file (auto-generated)
├── PersonalExpenseTracker.sln  # Visual Studio solution
├── PersonalExpenseTracker.csproj  # Project file
├── .gitignore              # Git ignore rules
└── README.md               # This documentation
```

## 🔧 Core Classes

### `Expense` Class
- Properties: Id, Amount, Category, Date, Description
- Methods: ToString(), ToFileString(), FromFileString()

### `ExpenseTracker` Class
- Manages expense collection and operations
- Handles file I/O operations
- Provides filtering and analysis methods

### `Program` Class
- Contains main application loop
- Handles user interface and menu system
- Manages user input and validation

## 💾 Data Storage

- **Format**: Pipe-delimited text file (expenses.txt)
- **Location**: Application directory
- **Features**: 
  - Automatic escaping of special characters
  - Error recovery for corrupted data
  - Incremental ID management

**Example data format:**
```
1|25.50|Food|2024-03-15|Lunch at restaurant
2|15.00|Transportation|2024-03-15|Bus fare
```

## ⚠️ Error Handling

The application includes comprehensive error handling for:
- Invalid input formats
- File I/O operations
- Data parsing errors
- User input validation
- Date format validation
- Negative amounts

## 🎯 Future Enhancements

- [ ] Export to CSV/Excel functionality
- [ ] Monthly/yearly expense reports
- [ ] Budget tracking and alerts
- [ ] Expense categories management
- [ ] Data visualization charts
- [ ] Multi-currency support
- [ ] Database integration
- [ ] Web interface version

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch:
   ```bash
   git checkout -b feature/amazing-feature
   ```
3. Commit your changes:
   ```bash
   git commit -m 'Add some amazing feature'
   ```
4. Push to the branch:
   ```bash
   git push origin feature/amazing-feature
   ```
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👩‍💻 Author

**Yasmin Gamal**
- GitHub: [@yasmin-gamal1](https://github.com/yasmin-gamal1)
- Email: [yasmingamal.edu0@gmail.com]

## 🙏 Acknowledgments

- Thanks to the .NET community for excellent documentation
- Inspired by the need for simple personal finance management
- Built as part of .NET bootcamp learning journey

## 📞 Support

If you encounter any issues or have questions:

1. Check the [Issues](https://github.com/yasmin-gamal1/personal-expense-tracker/issues) page
2. Create a new issue if your problem isn't already reported
3. Provide detailed information about the error and steps to reproduce

---

⭐ **Star this repository if you find it helpful!** ⭐

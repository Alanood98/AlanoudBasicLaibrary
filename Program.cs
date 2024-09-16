using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static System.Reflection.Metadata.BlobBuilder;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace BasicLibrary


{
    internal class Program
    {
        static List<(int BID, string BName, string BAuthor, int BorrowedCopies, int copies, string catogery, int BperiodDayes, int price)> Books = new List<(int BID, string BName, string BAuthor, int quantity, int copies, string catogery, int BperiodDayes, int price)>();
        static List<(int AID, string admin_name, string AdEmail, string password)> Admin = new List<(int AID, string admin_name, string AdEmail, string password)>();
        static List<(int UrId, string user_name, string UrEmail, string password)> User = new List<(int UrId, string user_name, string UrEmail, string password)>();
        static List<(int catgId, string catograyName, int numOfBookInEachCatogary)> Catogary = new List<(int catgId, string catograyName, int numOfBookInEachCatogary)>();
        // A boolean flag indicating whether the borrowed book has been returned (true for returned, false for not returned).
        static List<(int UrId, int BID, DateTime dateOfBorrow, DateTime dateOfReturn, DateTime actualReturnDate, int rating, bool IsReturned)> Borrowing = new List<(int UrId, int BID, DateTime dateOfBorrow, DateTime dateOfReturn, DateTime actualReturnDate, int rating, bool IsReturned)>();
        static string BooksPath = "C:\\Users\\Codeline User\\Desktop\\books.txt";
        static string AdminPath = "C:\\Users\\Codeline User\\Desktop\\admin.txt";
        static string UserPath = "C:\\Users\\Codeline User\\Desktop\\user.txt";
        static string BorrowingPath = "C:\\Users\\Codeline User\\Desktop\\borroing.txt";
        static string CatogryPath = "C:\\Users\\Codeline User\\Desktop\\catogry.txt";
        static void Main(string[] args)
        {
            // Load existing data
            LoadData();
            while (true)
            {
                Console.WriteLine("Library System");
                Console.WriteLine("1. User Login");
                Console.WriteLine("2. Admin Login");
                Console.WriteLine("3. Exit");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        UserLogin();
                        break;
                    case "2":
                        AdminLogin();
                        break;
                    case "3":
                        SaveData();
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }

        }

        static void LoadData()
        {
            // Load users from file
            if (File.Exists(UserPath))
            {
                var userIds = new HashSet<int>();
                var userNames = new HashSet<string>();
                var userEmails = new HashSet<string>();

                foreach (var line in File.ReadLines(UserPath))
                {
                    var parts = line.Split("|");
                    if (parts.Length == 4)
                    {
                        try
                        {
                            int id = int.Parse(parts[0]);
                            string name = parts[1];
                            string email = parts[2];

                            // Check for duplicates before adding to the list
                            if (userIds.Contains(id) || userNames.Contains(name) || userEmails.Contains(email))
                            {
                                Console.WriteLine($"Duplicate entry found: {name} (ID: {id}, Email: {email})");
                            }
                            else
                            {
                                User.Add((int.Parse(parts[0]), parts[1], parts[2], parts[3]));
                                userIds.Add(id);
                                userNames.Add(name);
                                userEmails.Add(email);
                            }
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine($"Error parsing user data: {ex.Message}");
                        }
                    }
                }
            }

            // Load admins from file
            if (File.Exists(AdminPath))
            {
                foreach (var line in File.ReadLines(AdminPath))
                {
                    var parts = line.Split("|");
                    if (parts.Length == 4)
                    {
                        try
                        {
                            Admin.Add((int.Parse(parts[0]), parts[1], parts[2], parts[3]));
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine($"Error parsing admin data: {ex.Message}");
                        }
                    }
                }
            }

            // Load books
            if (File.Exists(BooksPath))
            {
                foreach (var line in File.ReadLines(BooksPath))
                {
                    var parts = line.Split("|");
                    if (parts.Length == 8)
                    {
                        try
                        {
                            Books.Add((int.Parse(parts[0]), parts[1], parts[2], int.Parse(parts[3]), int.Parse(parts[4]), parts[5], int.Parse(parts[6]), int.Parse(parts[7])));
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine($"Error parsing book data: {ex.Message}");
                        }
                    }
                }
            }

            // Load borrowing records
            if (File.Exists(BorrowingPath))
            {
                foreach (var line in File.ReadLines(BorrowingPath))
                {
                    var parts = line.Split("|");
                    if (parts.Length == 7)
                    {
                        try
                        {
                            Borrowing.Add((int.Parse(parts[0]), int.Parse(parts[1]), DateTime.Parse(parts[2]), DateTime.Parse(parts[3]), DateTime.Parse(parts[4]), int.Parse(parts[5]), bool.Parse(parts[6])));
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine($"Error parsing borrowing data: {ex.Message}");
                        }
                    }
                }
            }

            // Load category records
            if (File.Exists(CatogryPath))
            {
                foreach (var line in File.ReadLines(CatogryPath))
                {
                    var parts = line.Split("|");
                    if (parts.Length == 3)
                    {
                        try
                        {
                            Catogary.Add((int.Parse(parts[0]), parts[1], int.Parse(parts[2])));
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine($"Error parsing category data: {ex.Message}");
                        }
                    }
                }
            }
        }

        static void UserLogin()
        {
            Console.WriteLine("Enter username:");
            string username = Console.ReadLine();

            string email;
            do
            {
                Console.WriteLine("Enter email (format: char@gmail.com):");
                email = Console.ReadLine();
            } while (!IsValidEmail(email));

            string password;
            string confirmPassword;
            do
            {
                Console.WriteLine("Enter password (at least 8 characters, one letter, one digit, one uppercase, and one symbol):");
                password = Console.ReadLine();

                Console.WriteLine("Re-enter password:");
                confirmPassword = Console.ReadLine();

                if (password != confirmPassword)
                {
                    Console.WriteLine("Passwords do not match. Please try again.");
                }
                else if (!IsValidPassword(password))
                {
                    Console.WriteLine("Password does not meet the requirements. Please try again.");
                }
            } while (password != confirmPassword || !IsValidPassword(password));

            bool userFound = false;
            bool correctPassword = false;
            int userId = 0;

            // Check tuples first
            foreach (var user in User)
            {
                if (user.UrEmail == email)
                {
                    userFound = true;
                    if (user.password == password)
                    {
                        correctPassword = true;
                        userId = user.UrId;
                        Console.WriteLine("User logged in.");

                        // Check for overdue books before showing the menu
                        bool hasOverdueBooks = CheckOverdueBooks(userId);

                        if (!hasOverdueBooks)
                        {
                            UserMenu(userId);
                        }
                        else
                        {
                            Console.WriteLine("You have overdue books. Please return them before proceeding.");
                            return;
                        }

                        return;
                    }
                    else
                    {
                        Console.WriteLine("Password is incorrect.");
                        return;
                    }
                }
            }

            // If not found tuple, check the file
            if (File.Exists(UserPath))
            {
                foreach (var line in File.ReadLines(UserPath))
                {
                    var parts = line.Split("|");
                    if (parts.Length == 4)
                    {
                        string fileEmail = parts[2];
                        string filePassword = parts[3];

                        if (fileEmail == email)
                        {
                            userFound = true;
                            if (filePassword == password)
                            {
                                correctPassword = true;
                                userId = int.Parse(parts[0]);
                                Console.WriteLine("Welcome user");

                                // Check for overdue books before showing the menu
                                bool hasOverdueBooks = CheckOverdueBooks(userId);

                                if (!hasOverdueBooks)
                                {
                                    UserMenu(userId);
                                }
                                else
                                {
                                    Console.WriteLine("You have overdue books. Please return them before proceeding.");
                                    return;
                                }

                                // Add user to tuple if not already present
                                bool isDuplicate = false;
                                foreach (var user in User)
                                {
                                    if (user.UrId == userId)
                                    {
                                        isDuplicate = true;
                                        break;
                                    }
                                }

                                if (!isDuplicate)
                                {
                                    User.Add((userId, parts[1], fileEmail, filePassword));
                                }
                                return;
                            }
                            else
                            {
                                Console.WriteLine("Password is incorrect.");
                                return;
                            }
                        }
                    }
                }
            }

            if (!userFound)
            {
                Console.WriteLine("User is not registered.");
                Console.WriteLine("Do you want to register? (yes/no)");
                if (Console.ReadLine().ToLower() == "yes")
                {
                    RegisterUser(username, password, email);
                }
            }
        }





        static void RegisterUser(string username, string password, string email)
        {
            // Check for duplicate usernames and emails before registering
            bool isDuplicate = false;
            foreach (var user in User)
            {
                if (user.user_name == username || user.UrEmail == email)
                {
                    isDuplicate = true;
                    Console.WriteLine("Username or email is already taken. Please choose a different username/email.");
                    return;
                }
            }

            int newId = 1;

            // Find the next available ID by checking both file and tuple
            if (File.Exists(UserPath))
            {
                // Read the highest ID from the file
                foreach (var line in File.ReadLines(UserPath))
                {
                    var parts = line.Split("|");
                    if (parts.Length == 4)
                    {
                        int id = int.Parse(parts[0]);
                        if (id >= newId)
                        {
                            newId = id + 1;
                        }
                    }
                }
            }

            // Check tuple for the highest ID
            foreach (var user in User)
            {
                if (user.UrId >= newId)
                {
                    newId = user.UrId + 1;
                }
            }

            // Write the new user to the file
            try
            {
                using (var writer = new StreamWriter(UserPath, true))
                {
                    writer.WriteLine($"{newId}|{username}|{email}|{password}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error writing to user file: " + ex.Message);
                return;
            }

            // Update the tuple
            User.Add((newId, username, email, password));

            Console.WriteLine("User registered successfully.");
        }

        static void UserMenu(int userId)
        {
            while (true)
            {
                Console.WriteLine("\nUser Menu:");
                Console.WriteLine("1. Borrow Book");
                Console.WriteLine("2. Return Book");
                Console.WriteLine("3. Search Book");
                Console.WriteLine("4. Show Profile");
                Console.WriteLine("5. Check Overdue Books");
                Console.WriteLine("6. Exit");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        DisplayCategoryBooks(); // Show all books before borrowing
                        Console.WriteLine("Enter ID of the book to borrow:");

                        if (int.TryParse(Console.ReadLine(), out int bookIdToBorrow))
                        {
                            BorrowBook(userId, bookIdToBorrow);
                            GetRecommendation(bookIdToBorrow); // Display recommendations based on the borrowed book
                        }
                        else
                        {
                            Console.WriteLine("Invalid book ID. Please enter a valid number.");
                        }
                        break;

                    case "2":
                        Console.WriteLine("Enter name of the book to return:");
                        string bookNameToReturn = Console.ReadLine();

                        // Confirm the book name before attempting to return
                        if (!string.IsNullOrWhiteSpace(bookNameToReturn))
                        {
                            ReturnBook(userId, bookNameToReturn);
                        }
                        else
                        {
                            Console.WriteLine("Book name cannot be empty.");
                        }
                        break;

                    case "3":
                        Console.WriteLine("Enter title to search:");
                        string searchTitle = Console.ReadLine();

                        if (!string.IsNullOrWhiteSpace(searchTitle))
                        {
                            SearchBookForUser(userId, searchTitle);
                        }
                        else
                        {
                            Console.WriteLine("Search title cannot be empty.");
                        }
                        break;

                    case "4":
                        ShowProfile(userId);
                        break;

                    case "5":
                        CheckOverdueBooks(userId);
                        break;

                    case "6":
                        return; // Exit the menu and return to the previous menu
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
        static void SaveData()
        {


            using (var writer = new StreamWriter(BooksPath))
            {
                foreach (var book in Books)
                {
                    writer.WriteLine($"{book.BID}|  {book.BName}|  {book.BAuthor}|  {book.BorrowedCopies}|  {book.copies}|  {book.catogery}|  {book.BperiodDayes}|  {book.price}");
                }
            }

            using (var writer = new StreamWriter(AdminPath))
            {
                foreach (var admin in Admin)
                {
                    writer.WriteLine($"{admin.AID}|  {admin.admin_name}|  {admin.AdEmail}|  {admin.password}");
                }
            }
            using (var writer = new StreamWriter(UserPath))
            {
                foreach (var user in User)
                {
                    writer.WriteLine($"{user.UrId}|  {user.user_name}|  {user.UrEmail}|  {user.password}");
                }
            }

            using (var writer = new StreamWriter(BorrowingPath))
            {
                foreach (var borrowing in Borrowing)
                {
                    writer.WriteLine($"{borrowing.UrId}|  {borrowing.BID}|  {borrowing.dateOfBorrow}|  {borrowing.dateOfReturn}|  {borrowing.actualReturnDate}|  {borrowing.rating}|  {borrowing.IsReturned}");
                }
            }
            using (var writer = new StreamWriter(CatogryPath))
            {
                foreach (var category in Catogary)
                {
                    writer.WriteLine($"{category.catgId}|  {category.catograyName}|  {category.numOfBookInEachCatogary}");
                }
            }

        }
        static void BorrowBook(int userId, int bookId)
        {
            bool bookFound = false;
            int bookIndex = -1;
            int bookCobies = 0;
            string bookName = "";
            string bookAuthor = "";
            int bookPeriodDays = 0; // Variable to store the borrowing period

            // Find the book with the given ID
            for (int i = 0; i < Books.Count; i++)
            {
                if (Books[i].BID == bookId)
                {
                    if (bookFound) // Check for duplicate book IDs
                    {
                        Console.WriteLine("Duplicate book ID found in the system. Please resolve this issue.");
                        return;
                    }

                    bookFound = true;
                    bookIndex = i;
                    bookName = Books[i].BName;
                    bookAuthor = Books[i].BAuthor;
                    bookCobies = Books[i].copies;
                    bookPeriodDays = Books[i].BperiodDayes; // Get the borrowing period
                }
            }

            if (!bookFound)
            {
                Console.WriteLine("Book not found.");
                return;
            }

            // Check if the book is available
            if (bookCobies <= 0)
            {
                Console.WriteLine("Book is not available.");
                return;
            }

            // Calculate the return date
            DateTime borrowDate = DateTime.Now;
            DateTime returnDate = borrowDate.AddDays(bookPeriodDays);

            // Display book details and borrowing information
            Console.WriteLine($"You are about to borrow the book:");
            Console.WriteLine($"{bookName} by {bookAuthor}");
            Console.WriteLine($"Borrowing period: {bookPeriodDays} days");
            Console.WriteLine($"You should return the book by: {returnDate.ToShortDateString()}");
            Console.WriteLine("Do you want to  borrowing this book? (yes/no)");

            // Get user confirmation
            string confirmation = Console.ReadLine().Trim().ToLower();
            if (confirmation != "yes")
            {
                Console.WriteLine("Borrowing cancelled.");
                return;
            }

            // Check for duplicate borrow records for the same book and user
            bool alreadyBorrowed = false;
            for (int i = 0; i < Borrowing.Count; i++)
            {
                if (Borrowing[i].UrId == userId && Borrowing[i].BID == bookId && !Borrowing[i].IsReturned)
                {
                    alreadyBorrowed = true;
                    break;
                }
            }

            if (alreadyBorrowed)
            {
                Console.WriteLine("You have already borrowed this book and have not returned it yet.");
                return;
            }

            // Add borrowing record
            Borrowing.Add((userId, bookId, DateTime.Today, DateTime.Today.AddDays(bookPeriodDays), DateTime.Today, 0, false));

            // Update the book quantity
            Books[bookIndex] = (bookId, bookName, bookAuthor, Books[bookIndex].BorrowedCopies + 1, bookCobies - 1, Books[bookIndex].catogery, bookPeriodDays, Books[bookIndex].price);

            try
            {
                // Save updated data to file
                SaveData();
                Console.WriteLine("Book borrowed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving data: " + ex.Message);
            }
        }

        static void ReturnBook(int userId, string bookName)
        {
            // Check for duplicate book names in the system
            int bookCount = 0;
            int bookIndex = -1;
            int bookCopies = 0;
            string bookAuthor = "";
            int bookPeriodDays = 0; // Variable to store the borrowing period

            // Find the book with the given name and ensure no duplicates
            for (int i = 0; i < Books.Count; i++)
            {
                if (Books[i].BName.Equals(bookName, StringComparison.OrdinalIgnoreCase))
                {
                    bookCount++;
                    if (bookCount > 1) // If more than one book with the same name is found
                    {
                        Console.WriteLine("Duplicate book names found. Please resolve this issue.");
                        return;
                    }

                    bookIndex = i;
                    bookCopies = Books[i].copies;
                    bookAuthor = Books[i].BAuthor;
                    bookPeriodDays = Books[i].BperiodDayes; // Get the borrowing period
                }
            }

            if (bookIndex == -1)
            {
                Console.WriteLine("Book not found.");
                return;
            }

            // Find the borrowing record with the given userId and book name
            bool borrowingFound = false;
            int borrowingIndex = -1;
            DateTime borrowingDate = DateTime.Today.AddDays(-bookPeriodDays); // Set borrowingDate to a default value
            DateTime returnDate = DateTime.Today.AddDays(bookPeriodDays);
            DateTime actualReturnDate = DateTime.Today; // Current date

            for (int i = 0; i < Borrowing.Count; i++)
            {
                if (Borrowing[i].UrId == userId && Borrowing[i].BID == Books[bookIndex].BID && !Borrowing[i].IsReturned)
                {
                    borrowingFound = true;
                    borrowingIndex = i;
                    borrowingDate = Borrowing[i].dateOfBorrow;
                    returnDate = Borrowing[i].dateOfReturn;
                    break;
                }
            }

            if (!borrowingFound)
            {
                Console.WriteLine("No record of this book being borrowed by you.");
                return;
            }

            // Ask user to confirm return
            Console.WriteLine($"You are about to return '{bookName}' by {bookAuthor}.");
            Console.WriteLine($"This book should have been returned by {returnDate.ToShortDateString()}.");
            Console.WriteLine("Do you want to return it? (yes/no)");
            string confirmation = Console.ReadLine().Trim().ToLower();
            if (confirmation != "yes")
            {
                Console.WriteLine("Return canceled.");
                return;
            }

            // Ask user to rate the book
            int rating = 0;
            bool validRating = false;
            while (!validRating)
            {
                Console.WriteLine("Please rate the book out of 5 (1 to 5):");
                if (int.TryParse(Console.ReadLine(), out rating) && rating >= 1 && rating <= 5)
                {
                    validRating = true;
                }
                else
                {
                    Console.WriteLine("Invalid rating. Please enter a number between 1 and 5.");
                }
            }

            // Mark the book as returned in the borrowings list
            Borrowing[borrowingIndex] = (userId, Books[bookIndex].BID, borrowingDate, returnDate, actualReturnDate, rating, true);

            // Update the book information
            Books[bookIndex] = (Books[bookIndex].BID, bookName, bookAuthor, Books[bookIndex].BorrowedCopies - 1, bookCopies + 1, Books[bookIndex].catogery, bookPeriodDays, Books[bookIndex].price);

            try
            {
                // Save updated data to file
                SaveData();
                Console.WriteLine("Book returned successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving data: " + ex.Message);
            }

            if (actualReturnDate > returnDate)
            {
                TimeSpan daysLate = actualReturnDate - returnDate; // Calculate days late
                Console.WriteLine($"You returned the book {daysLate.Days} days late.");
            }
        }



        static void DisplayCategoryBooks()
        {
            // Step 1: Display all categories
            if (Catogary.Count == 0)
            {
                Console.WriteLine("No categories available.");
                return;
            }

            Console.WriteLine("Available Categories:");
            foreach (var category in Catogary)
            {
                Console.WriteLine($"{category.catgId} - {category.catograyName}, Number of Books: {category.numOfBookInEachCatogary}");
            }

            // Step 2: Prompt user to select a category by name
            Console.WriteLine("Enter the name of the category to view books:");
            string selectedCategoryName = Console.ReadLine().Trim();

            // Step 3: Display books in the selected category
            bool foundBooks = false;
            Console.WriteLine($"Books in Category '{selectedCategoryName}':");

            foreach (var book in Books)
            {
                if (book.catogery.Equals(selectedCategoryName, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"{book.BID} - {book.BName} by {book.BAuthor}, Quantity: {book.BorrowedCopies}, Copies: {book.copies}, Period Days: {book.BperiodDayes}, Price: {book.price:C}");
                    foundBooks = true;
                }
            }

            if (!foundBooks)
            {
                Console.WriteLine("No books available in the selected category.");
            }
        }

        static void SearchBookForUser(int userId, string searchTerm)
        {
            bool bookFound = false;

            Console.WriteLine("Search Results:");

            // Convert the search term to lower case
            string lowerSearchTerm = searchTerm.ToLower();

            for (int i = 0; i < Books.Count; i++)
            {
                // Case-insensitive search for book title
                if (Books[i].BName.ToLower().Contains(lowerSearchTerm))
                {
                    // Book found, display its information
                    bookFound = true;
                    Console.WriteLine($"Book ID: {Books[i].BID}");
                    Console.WriteLine($"Title: {Books[i].BName}");
                    Console.WriteLine($"Author: {Books[i].BAuthor}");
                    Console.WriteLine($"Borrowed Copies: {Books[i].BorrowedCopies}");
                    Console.WriteLine($"Total Copies: {Books[i].copies}");
                    Console.WriteLine($"Category: {Books[i].catogery}");
                    Console.WriteLine($"Borrowing Period (days): {Books[i].BperiodDayes}");
                    Console.WriteLine($"Price: {Books[i].price:C}");
                    Console.WriteLine();
                }
            }

            if (!bookFound)
            {
                Console.WriteLine("No books found with the given title.");
                return;
            }

            // Prompt user to borrow a book
            Console.WriteLine("Would you like to borrow a book? (yes/no)");
            string choice = Console.ReadLine().Trim().ToLower();

            if (choice == "yes")
            {
                Console.WriteLine("Enter the Book ID you want to borrow:");
                if (int.TryParse(Console.ReadLine(), out int bookId))
                {
                    // Call BorrowBook method with userId and the bookId
                    BorrowBook(userId, bookId);
                }
                else
                {
                    Console.WriteLine("Invalid Book ID.");
                }
            }
            else
            {
                Console.WriteLine("Returning to main menu.");
            }
        }
        static void SearchBookForAdmin(int adminId, string searchTerm)
        {
            bool bookFound = false;

            Console.WriteLine("Search Results:");

            // Convert the search term to lower case
            string lowerSearchTerm = searchTerm.ToLower();

            for (int i = 0; i < Books.Count; i++)
            {
                // Case-insensitive search for book title
                if (Books[i].BName.ToLower().Contains(lowerSearchTerm))
                {
                    // Book found, display its information
                    bookFound = true;
                    Console.WriteLine($"Book ID: {Books[i].BID}");
                    Console.WriteLine($"Title: {Books[i].BName}");
                    Console.WriteLine($"Author: {Books[i].BAuthor}");
                    Console.WriteLine($"Borrowed Copies: {Books[i].BorrowedCopies}");
                    Console.WriteLine($"Total Copies: {Books[i].copies}");
                    Console.WriteLine($"Category: {Books[i].catogery}");
                    Console.WriteLine($"Borrowing Period (days): {Books[i].BperiodDayes}");
                    Console.WriteLine($"Price: {Books[i].price:C}"); // Format price as currency
                    Console.WriteLine();
                }
            }

            if (!bookFound)
            {
                Console.WriteLine("No books found with the given title.");
            }
        }
        static void AdminLogin()
        {
            // Step 1: Get admin name
            Console.WriteLine("Enter admin name:");
            string name = Console.ReadLine().Trim();

            // Step 2: Get and validate admin email
            string email;
            do
            {
                Console.WriteLine("Enter admin email (format: example@gmail.com):");
                email = Console.ReadLine().Trim();
            } while (!IsValidEmail(email));

            // Step 3: Get and validate passwords
            string password;
            string confirmPassword;
            do
            {
                Console.WriteLine("Enter password (at least 8 characters, one letter, one digit, one uppercase, and one symbol):");
                password = Console.ReadLine();

                Console.WriteLine("Re-enter password:");
                confirmPassword = Console.ReadLine();

                if (password != confirmPassword)
                {
                    Console.WriteLine("Passwords do not match. Please try again.");
                }
                else if (!IsValidPassword(password))
                {
                    Console.WriteLine("Password does not meet the required criteria. Please try again.");
                }

            } while (password != confirmPassword || !IsValidPassword(password));

            bool adminFound = false;

            // Check tuple for existing admin by email and password
            foreach (var admin in Admin)
            {
                if (admin.AdEmail.Trim().ToLower() == email.ToLower() && admin.password == password)
                {
                    adminFound = true;
                    Console.WriteLine("Admin logged in.");
                    AdminMenu(admin.AID);
                    return;
                }
            }

            // If not found in tuple, check the file
            if (File.Exists(AdminPath))
            {
                foreach (var line in File.ReadLines(AdminPath))
                {
                    var parts = line.Split("|"); // Ensure delimiter is correct
                    if (parts.Length == 4)
                    {
                        string fileEmail = parts[2].Trim();
                        string filePassword = parts[3].Trim();
                        if (fileEmail.ToLower() == email.ToLower() && filePassword == password)
                        {
                            adminFound = true;
                            Console.WriteLine("Welcome Admin");
                            // Add admin to tuple
                            Admin.Add((int.Parse(parts[0]), parts[1], fileEmail, filePassword));
                            AdminMenu(int.Parse(parts[0]));
                            return;
                        }
                    }
                }
            }

            // If admin is not found
            if (!adminFound)
            {
                Console.WriteLine("Invalid admin email or password.");
                Console.WriteLine("Do you want to register as a new admin? (yes/no)");
                if (Console.ReadLine().Trim().ToLower() == "yes")
                {
                    RegisterAdmin(email, password, name);  // Use validated email and password
                }
            }
        }

        static void RegisterAdmin(string email, string password, string name)
        {
            // Step 1: Check for duplicate names and emails in the tuple
            foreach (var admin in Admin)
            {
                if (admin.AdEmail.Trim().ToLower() == email.Trim().ToLower())
                {
                    Console.WriteLine("Admin email already exists.");
                    return;
                }
                if (admin.admin_name.Trim().ToLower() == name.Trim().ToLower())
                {
                    Console.WriteLine("Admin name already exists.");
                    return;
                }
            }

            // Step 2: Check for duplicate names and emails in the file
            if (File.Exists(AdminPath))
            {
                foreach (var line in File.ReadLines(AdminPath))
                {
                    var parts = line.Split('|'); // Ensure delimiter is correct
                    if (parts.Length == 4)
                    {
                        string fileEmail = parts[2].Trim();
                        string fileName = parts[1].Trim();
                        if (fileEmail.ToLower() == email.Trim().ToLower())
                        {
                            Console.WriteLine("Admin email already exists.");
                            return;
                        }
                        if (fileName.ToLower() == name.Trim().ToLower())
                        {
                            Console.WriteLine("Admin name already exists.");
                            return;
                        }
                    }
                }
            }

            // Step 3: Find the next available ID
            int newId = 1;
            if (File.Exists(AdminPath))
            {
                foreach (var line in File.ReadLines(AdminPath))
                {
                    var parts = line.Split('|'); // Ensure delimiter is correct
                    if (parts.Length == 4)
                    {
                        int id = int.Parse(parts[0]);
                        if (id >= newId)
                        {
                            newId = id + 1;
                        }
                    }
                }
            }

            // Also update the tuple to find the maximum ID
            foreach (var admin in Admin)
            {
                if (admin.AID >= newId)
                {
                    newId = admin.AID + 1;
                }
            }

            // Step 4: Write new admin to the file
            using (var writer = new StreamWriter(AdminPath, true))
            {
                writer.WriteLine($"{newId}|{name}|{email}|{password}");
            }

            // Step 5: Update tuple
            Admin.Add((newId, name, email, password));

            Console.WriteLine("Admin registered successfully.");
        }

        // Validation functions (reuse these for both users and admins)
        static bool IsValidEmail(string email)
        {
            // General email validation pattern: allows any domain
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }


        static bool IsValidPassword(string password)
        {
            // Password must be at least 8 characters, contain at least one letter, one digit, one uppercase letter, and one symbol
            string pattern = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).{8,}$";
            return Regex.IsMatch(password, pattern);
        }

        static void AdminMenu(int admin_id)
        {
            while (true)
            {
                Console.WriteLine("\nAdmin Menu:");
                Console.WriteLine("1. Add Book");
                Console.WriteLine("2. Edit Book");
                Console.WriteLine("3. Remove Book");
                Console.WriteLine("4. Search Books");
                Console.WriteLine("5. Display All Books of Common Category");
                Console.WriteLine("6. Show Report");
                Console.WriteLine("7. Add New Category");
                Console.WriteLine("8. Exit");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddNewBook();
                        break;
                    case "2":
                        EditBook();
                        break;
                    case "3":
                        RemoveBook();
                        break;
                    case "4":
                        Console.WriteLine("Please enter the name of the book you want to search: ");
                        string bookName = Console.ReadLine();
                        SearchBookForAdmin(admin_id, bookName);
                        break;
                    case "5":
                        DisplayCategoryBooks();
                        break;
                    case "6":
                        ShowReport();
                        break;
                    case "7":
                        AddNewCategory();
                        break;
                    case "8":
                        return; // Exit the Admin Menu
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void AddNewBook()
        {
            // Initialize the bookIdCounter if it has not been initialized yet
            int bookIdCounter = 1; // Default initial value

            // Check if there are existing books to determine the next ID
            if (Books.Count > 0)
            {
                // Find the highest existing book ID
                int maxId = 0;
                for (int i = 0; i < Books.Count; i++)
                {
                    if (Books[i].BID > maxId)
                    {
                        maxId = Books[i].BID;
                    }
                }
                bookIdCounter = maxId + 1;
            }

            // Display all categories and prompt the admin to select one
            DisplayCategoryBooks();
            Console.WriteLine("Enter the category name to add the book to:");
            string categoryName = Console.ReadLine();

            // Check if the selected category exists
            bool categoryExists = false;
            for (int i = 0; i < Catogary.Count; i++)
            {
                if (Catogary[i].catograyName.ToLower() == categoryName.ToLower())
                {
                    categoryExists = true;
                    break;
                }
            }

            if (!categoryExists)
            {
                Console.WriteLine("Category does not exist. Please add the category first.");
                return;
            }

            // Get book details from the admin
            Console.WriteLine("Enter Book Name: ");
            string name = Console.ReadLine();

            // Check if the book name already exists (case-insensitive)
            bool bookExists = false;
            for (int i = 0; i < Books.Count; i++)
            {
                if (Books[i].BName.ToLower() == name.ToLower())
                {
                    bookExists = true;
                    break;
                }
            }

            if (bookExists)
            {
                Console.WriteLine("A book with this name already exists. Please enter a different book name.");
                return;
            }

            Console.WriteLine("Enter Book Author:");
            string author = Console.ReadLine();

            Console.WriteLine("Enter Book quantity:");
            int quantity = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter how many copies we have:");
            int copies = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter Book period days:");
            int periodDays = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter Book price:");
            int price = int.Parse(Console.ReadLine());

            // Add the new book with the automatically generated ID
            Books.Add((bookIdCounter, name, author, 0, copies, categoryName, periodDays, price));
            Console.WriteLine($"Book '{name}' added successfully with ID {bookIdCounter}.");
        }


        static void AddNewCategory()
        {
            // Initialize the categoryIdCounter to 1 by default
            int categoryIdCounter = 1;

            // Check if there are existing categories to determine the next ID
            if (Catogary.Count > 0)
            {
                // Find the highest existing category ID
                int maxId = 0;
                for (int i = 0; i < Catogary.Count; i++)
                {
                    if (Catogary[i].catgId > maxId)
                    {
                        maxId = Catogary[i].catgId;
                    }
                }
                categoryIdCounter = maxId + 1;
            }

            // Generate a new category ID
            int catgID = categoryIdCounter;

            // Get category details from the admin
            Console.WriteLine("Enter category name: ");
            string categoryName = Console.ReadLine();

            // Check if the category name already exists (case-insensitive comparison)
            bool categoryExists = false;
            for (int i = 0; i < Catogary.Count; i++)
            {
                if (string.Equals(Catogary[i].catograyName, categoryName, StringComparison.OrdinalIgnoreCase))
                {
                    categoryExists = true;
                    break;
                }
            }

            if (categoryExists)
            {
                Console.WriteLine("This category already exists. Please enter a different category name.");
            }
            else
            {
                Console.WriteLine("Enter number of books in this category: ");
                int numOfBooks = int.Parse(Console.ReadLine());

                // Add the new category to the list
                Catogary.Add((catgID, categoryName, numOfBooks));
                Console.WriteLine("Category added successfully.");
            }
        }


        static void EditBook()
        {
            Console.WriteLine("Enter the name of the book you want to edit: ");
            string name = Console.ReadLine(); // Book name to search

            bool bookFound = false;

            // Loop to find the book by name
            for (int i = 0; i < Books.Count; i++)
            {
                if (Books[i].BName.ToLower() == name.ToLower())
                {
                    Console.WriteLine("Book found!");
                    Console.WriteLine($"Author: {Books[i].BAuthor}");
                    Console.WriteLine($"Category: {Books[i].catogery}");
                    Console.WriteLine($"Period (days): {Books[i].BperiodDayes}");
                    Console.WriteLine($"Price: {Books[i].price}");
                    Console.WriteLine($"Copies available: {Books[i].copies}");

                    // Ask for new book details
                    Console.WriteLine("Edit the book name: ");
                    string newBookName = Console.ReadLine();

                    Console.WriteLine("Edit the book author name: ");
                    string newAuthor = Console.ReadLine();

                    Console.WriteLine("Edit the number of copies available:");
                    int newCopies = int.Parse(Console.ReadLine()); // Update the number of available copies

                    Console.WriteLine("Edit the borrowed copies:");
                    int newBorrowedCopies = int.Parse(Console.ReadLine()); // Update the number of borrowed copies

                    Console.WriteLine("Edit the book category: ");
                    string newCategory = Console.ReadLine();

                    Console.WriteLine("Edit the book period (days): ");
                    int newPeriodDays = int.Parse(Console.ReadLine());

                    Console.WriteLine("Edit the book price: ");
                    int newPrice = int.Parse(Console.ReadLine());

                    // Update the book details
                    Books[i] = (Books[i].BID, newBookName, newAuthor, newBorrowedCopies, newCopies, newCategory, newPeriodDays, newPrice);
                    Console.WriteLine($"You have successfully updated the book '{name}'.");

                    bookFound = true;
                    break;
                }
            }

            if (!bookFound)
            {
                Console.WriteLine("Book not found.");
            }
        }

        static void RemoveBook()
        {
            Console.WriteLine("Enter the name of the book to remove:");
            string bookName = Console.ReadLine();

            bool bookFound = false;
            bool bookBorrowed = false;
            int bookIndex = -1;

            // Find the book by name
            for (int i = 0; i < Books.Count; i++)
            {
                if (Books[i].BName.ToLower() == bookName.ToLower())
                {
                    bookFound = true;
                    bookIndex = i;

                    // Check if the book has been borrowed
                    for (int j = 0; j < Borrowing.Count; j++)
                    {
                        if (Borrowing[j].BID == Books[i].BID && !Borrowing[j].IsReturned)
                        {
                            bookBorrowed = true;
                            break;
                        }
                    }

                    break;
                }
            }

            if (bookFound)
            {
                if (bookBorrowed)
                {
                    Console.WriteLine("Cannot remove the book as it is currently borrowed.");
                }
                else
                {
                    // Remove the book
                    Books.RemoveAt(bookIndex);
                    Console.WriteLine("Book removed successfully.");

                    // Save updated data to file
                    SaveData();
                }
            }
            else
            {
                Console.WriteLine("Book not found.");
            }
        }

        static void ShowReport()
        {
            // 1. Total number of categories
            int totalCategories = Catogary.Count;
            Console.WriteLine($"Total Number of Categories: {totalCategories}");

            // 2. Total number of books
            int totalBooks = Books.Count;
            Console.WriteLine($"Total Number of Books: {totalBooks}");

            // 3. Books per category
            Console.WriteLine("\nBooks per Category:");
            foreach (var category in Catogary)
            {
                // Find the books that belong to this category
                int booksInCategory = 0;
                foreach (var book in Books)
                {
                    if (book.catogery.Equals(category.catograyName, StringComparison.OrdinalIgnoreCase))
                    {
                        booksInCategory++;
                    }
                }
                Console.WriteLine($"Category: {category.catograyName}, Number of Books: {booksInCategory}");
            }

            // 4. Total copies of all books
            int totalCopies = 0;
            foreach (var book in Books)
            {
                totalCopies += book.copies;
            }
            Console.WriteLine($"\nTotal Copies of All Books: {totalCopies}");

            // 5. Total borrowed books
            int totalBorrowedBooks = 0;
            foreach (var book in Books)
            {
                totalBorrowedBooks += book.BorrowedCopies;
            }
            Console.WriteLine($"Total Borrowed Books: {totalBorrowedBooks}");

            // 6. Total returned books
            int totalReturnedBooks = 0;
            foreach (var borrow in Borrowing)
            {
                if (borrow.IsReturned)
                {
                    totalReturnedBooks++;
                }
            }
            Console.WriteLine($"Total Returned Books: {totalReturnedBooks}");
        }


        static void ShowProfile(int userId)
        {
            // Find the user by ID
            int userIndex = -1;
            for (int i = 0; i < User.Count; i++)
            {
                if (User[i].UrId == userId)
                {
                    userIndex = i;
                    break;
                }
            }

            if (userIndex == -1)
            {
                Console.WriteLine("User not found.");
                return;
            }

            // Display user details
            Console.WriteLine($"\nUser Profile:");
            Console.WriteLine($"ID: {User[userIndex].UrId}");
            Console.WriteLine($"Name: {User[userIndex].user_name}");
            Console.WriteLine($"Email: {User[userIndex].UrEmail}");

            // Display currently borrowed (unreturned) books
            Console.WriteLine("\nCurrently borrowed (unreturned) books:");
            bool hasUnreturnedBooks = false;
            for (int i = 0; i < Borrowing.Count; i++)
            {
                if (Borrowing[i].UrId == User[userIndex].UrId && !Borrowing[i].IsReturned)
                {
                    hasUnreturnedBooks = true;
                    string bookName = "";
                    for (int j = 0; j < Books.Count; j++)
                    {
                        if (Books[j].BID == Borrowing[i].BID)
                        {
                            bookName = Books[j].BName;
                            break;
                        }
                    }

                    Console.WriteLine($"- {bookName}");
                    Console.WriteLine($"  Borrowed on: {Borrowing[i].dateOfBorrow.ToShortDateString()}");
                    Console.WriteLine($"  Due on: {Borrowing[i].dateOfReturn.ToShortDateString()}");
                }
            }

            if (!hasUnreturnedBooks)
            {
                Console.WriteLine("No unreturned books.");
            }

            // Display returned books and check if they were returned on time or overdue
            Console.WriteLine("\nReturned books:");
            bool hasReturnedBooks = false;
            for (int i = 0; i < Borrowing.Count; i++)
            {
                if (Borrowing[i].UrId == User[userIndex].UrId && Borrowing[i].IsReturned)
                {
                    hasReturnedBooks = true;
                    string bookName = "";
                    for (int j = 0; j < Books.Count; j++)
                    {
                        if (Books[j].BID == Borrowing[i].BID)
                        {
                            bookName = Books[j].BName;
                            break;
                        }
                    }

                    Console.WriteLine($"- {bookName}");
                    Console.WriteLine($"  Borrowed on: {Borrowing[i].dateOfBorrow.ToShortDateString()}");
                    Console.WriteLine($"  Returned on: {Borrowing[i].actualReturnDate.ToShortDateString()}");

                    // Check if the book was returned on time or overdue
                    if (Borrowing[i].actualReturnDate > Borrowing[i].dateOfReturn)
                    {
                        TimeSpan daysLate = Borrowing[i].actualReturnDate - Borrowing[i].dateOfReturn;
                        Console.WriteLine($"  Returned late by {daysLate.Days} days.");
                    }
                    else
                    {
                        Console.WriteLine("  Returned on time.");
                    }
                }
            }

            if (!hasReturnedBooks)
            {
                Console.WriteLine("No books returned.");
            }
        }

        static bool CheckOverdueBooks(int userId)
        {
            DateTime today = DateTime.Today; // Use DateTime.Today for current date

            foreach (var borrow in Borrowing)
            {
                // Check if the current borrowing record matches the userId and is not returned
                if (borrow.UrId == userId && !borrow.IsReturned)
                {
                    DateTime returnDate = borrow.dateOfReturn; // Assuming 'dateOfReturn' is already a DateTime object

                    // For debugging purposes (remove or comment out in production)
                    Console.WriteLine($"Checking overdue: UserId = {userId}, ReturnDate = {returnDate.ToShortDateString()}, Today = {today.ToShortDateString()}");

                    // Check if today is past the return date
                    if (today > returnDate)
                    {
                        return true; // User has overdue books
                    }
                }
            }

            return false; // No overdue books
        }




        static void GetRecommendation(int bookId)
        {
            bool bookFound = false;
            int bookRating = 0;
            string bookName = "";
            string bookAuthor = "";

            // Find the book with the given ID and its rating
            for (int i = 0; i < Books.Count; i++)
            {
                if (Books[i].BID == bookId)
                {
                    bookFound = true;
                    bookName = Books[i].BName;
                    bookAuthor = Books[i].BAuthor;
                    // Find the rating of the specified book
                    for (int j = 0; j < Borrowing.Count; j++)
                    {
                        if (Borrowing[j].BID == bookId && Borrowing[j].IsReturned)
                        {
                            bookRating = Borrowing[j].rating;
                            break;
                        }
                    }
                    break;
                }
            }

            if (!bookFound)
            {
                Console.WriteLine("Book not found.");
                return;
            }

            Console.WriteLine($"Recommendations based on the book '{bookName}' by {bookAuthor}:");

            bool foundRecommendation = false;

            // Find and display other books with a higher rating
            for (int i = 0; i < Books.Count; i++)
            {
                if (Books[i].BID != bookId)
                {
                    // Check for the rating of other books
                    int otherBookRating = 0;
                    for (int j = 0; j < Borrowing.Count; j++)
                    {
                        if (Borrowing[j].BID == Books[i].BID && Borrowing[j].IsReturned)
                        {
                            otherBookRating = Borrowing[j].rating;
                            break;
                        }
                    }

                    if (otherBookRating > bookRating)
                    {
                        Console.WriteLine($"ID: {Books[i].BID}, Name: {Books[i].BName}, Author: {Books[i].BAuthor}, Rating: {otherBookRating}");
                        foundRecommendation = true;
                    }
                }
            }

            if (!foundRecommendation)
            {
                Console.WriteLine("No other books with a higher rating found.");
            }
        }
    }
}
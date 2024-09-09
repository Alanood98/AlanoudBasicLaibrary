using System;
using System.Text;
using System.Xml.Linq;
using static System.Reflection.Metadata.BlobBuilder;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BasicLibrary
{
    internal class Program
    {
        static List<(string BName, string BAuthor, int BID, int quantity)> Books = new List<(string BName, string BAuthor, int BID, int quntity)>();
        static List<(string AdEmail, int password)> Admin = new List<(string AdEmail, int password)>();
        static List<(string UrEmail, int password, int UrId)> User = new List<(string UrEmail, int password, int UrId)>();
        static List<(int UrId, int BID, bool Returned)> Borrowing = new List<(int UrId, int BID, bool Returned)>();
        static string filePath = "C:\\Users\\Codeline User\\Desktop\\lib.txt";
        static string AdminPath = "C:\\Users\\Codeline User\\Desktop\\Admin.txt";
        static string UserPath = "C:\\Users\\Codeline User\\Desktop\\User.txt";
        static string BorrowingPath = "C:\\Users\\Codeline User\\Desktop\\Borrowing.txt";
        // testing chuckout
        static void Main(string[] args)
        {
            // downloaded form Alanoud device
            bool ExitFlag = false;

            LoadBooksFromLibFile();
            LoadBooksFromAdminFile();
            LoadBooksFromUserFile();
            LoadBooksFromBorrowingFile();


            do
            {
                Console.WriteLine("Choose an option: \n" +
                "1 - Login \n" +
                 "2 - Admin \n" +
                 "3 - User \n" +
                "4 - Log out \n" +
                "5 - Save to Admin file \n" +
                "6 - Save to User file \n" +
                "7 - Save to Borrow file \n");
                string choice = Console.ReadLine();
                switch (choice)
                {

                    case "1":
                        Login();
                        break;

                    case "2":
                        AdminMenu();
                        break;

                    case "3":
                        UserMenu();
                        break;

                    case "4":
                        SaveBooksToPathFile();
                        ExitFlag = true;
                        break;
                    case "5":
                        SaveToAdminFile();
                        ExitFlag = true;
                        break;
                    case "6":
                        SaveToUserFile();
                        ExitFlag = true;
                        break;
                    case "7":
                        SaveToBorrowingFile();
                        ExitFlag = true;
                        break;

                    default:
                        Console.WriteLine("enter correct choice");
                        break;
                }
            } while (ExitFlag != true);


        }


        static void Login()
        {
            Console.WriteLine("Are you Admin or User?");
            string loginPerson = Console.ReadLine().ToLower();

            Console.WriteLine("Enter your email:");
            string email = Console.ReadLine();
            Console.WriteLine("Enter your password:");
            string password = Console.ReadLine();

            if (loginPerson == "user")
            {
                if (CheckLoginUser(UserPath, email, password))
                {
                    Console.WriteLine("Login successful as User.");
                    UserMenu(); 
                }
                else
                {
                    Console.WriteLine("User not registered. Do you want to register? (yes/no)");
                    if (Console.ReadLine().ToLower() == "yes")
                    {
                        RegisterUser(email, password);
                        Console.WriteLine("User registered successfully.");
                        UserMenu(); 
                    }
                }
            }
            else if (loginPerson == "admin")
            {
                if (CheckLoginAdmin(AdminPath, email, password))
                {
                    Console.WriteLine("Login successful as Admin.");
                    AdminMenu();  
                }
                else
                {
                    Console.WriteLine("Admin not registered. Do you want to register? (yes/no)");
                    if (Console.ReadLine().ToLower() == "yes")
                    {
                        RegisterAdmin(email, password);
                        Console.WriteLine("Admin registered successfully.");
                        AdminMenu();  
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter 'admin' or 'user'.");
                Console.Clear();
            }
        }

        static bool CheckLoginUser(string UserPath, string email, string password)
        {
            if (File.Exists(UserPath))
            {
                using (StreamReader reader = new StreamReader(UserPath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var data = line.Split(',');
                        if (data[1] == email && data[2] == password)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        static bool CheckLoginAdmin(string AdminPath, string email, string password)
        {
            if (File.Exists(AdminPath))
            {
                using (StreamReader reader = new StreamReader(AdminPath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var data = line.Split(',');
                        if (data[1] == email && data[2] == password)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        static void AdminMenu()
        {
            bool ExitFlag = false;

            do
            {
                Console.WriteLine("Welcome Admin");
                Console.WriteLine("\n Enter the char of operation you need :");
                Console.WriteLine("\n A- Add New Book");
                Console.WriteLine("\n B- Display All Books");
                Console.WriteLine("\n C- Search for Book by Name");
                Console.WriteLine("\n D- Edit the books");
                Console.WriteLine("\n E- Remove the books");
                Console.WriteLine("\n F- Display static");
                Console.WriteLine("\n H- Save and Exit");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "A":
                        AddnNewBook();
                        break;

                    case "B":
                        ViewAllBooks();
                        break;

                    case "C":
                        SearchForBook();
                        break;

                    case "D":
                        EditBook();
                        break;

                    case "E":
                        RemoveBook();
                        break;

                    case "F":
                        DisplayStatic();
                        break;

                    //case "G":
                       //RegisterAdmin();
                      //break;
                    case "H":
                        ExitFlag = true;
                        break;

                    default:
                        Console.WriteLine("Sorry your choice was wrong");
                        break;



                }

                Console.WriteLine("press any key to continue");
                string cont = Console.ReadLine();

                Console.Clear();

            } while (ExitFlag != true);

        }


        static void UserMenu()
        {

            bool ExitFlag = false;

            do
            {
                Console.WriteLine("Welcome User");
                Console.WriteLine("\n Enter the char of operation you need :");
                Console.WriteLine("\n A- Search for Book by Name");
                Console.WriteLine("\n B- Borrow Book");
                Console.WriteLine("\n C- Return Book ");
                Console.WriteLine("\n D- View Recommendations ");

                Console.WriteLine("\n E- Save and Exit");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "A":
                        SearchForBook();
                        break;

                    case "B":
                        BorrowBook();
                        break;

                    case "C":
                        ReturnBooks();
                        break;

                    case "D":
                        ViewRecommendations();
                        break;
                    case "E":
                        ExitFlag = true;
                        break;
                    //case "E":
                    //    RegisterUser();
                    //    break;

                    default:
                        Console.WriteLine("Sorry your choice was wrong");
                        break;



                }

                Console.WriteLine("press any key to continue");
                string cont = Console.ReadLine();

                Console.Clear();

            } while (ExitFlag != true);


        }


        static void AddnNewBook()
        {
            Console.WriteLine("Enter Book Name");
            string name = Console.ReadLine();

            Console.WriteLine("Enter Book Author");
            string author = Console.ReadLine();

            Console.WriteLine("Enter Book ID");
            int ID = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter Book quantity");
            int qnt = int.Parse(Console.ReadLine());

            Books.Add((name, author, ID, qnt));
            Console.WriteLine("Book Added Succefully");

        }

        static void ViewAllBooks()
        {
            StringBuilder sb = new StringBuilder();

            int BookNumber = 0;

            for (int i = 0; i < Books.Count; i++)
            {
                BookNumber = i + 1;
                sb.Append("Book ").Append(BookNumber).Append(" name : ").Append(Books[i].BName);
                sb.AppendLine();
                sb.Append("Book ").Append(BookNumber).Append(" Author : ").Append(Books[i].BAuthor);
                sb.AppendLine();
                sb.Append("Book ").Append(BookNumber).Append(" ID : ").Append(Books[i].BID);
                sb.AppendLine().AppendLine();
                sb.Append("Book ").Append(BookNumber).Append(" Quantity : ").Append(Books[i].quantity);
                sb.AppendLine().AppendLine();
                Console.WriteLine(sb.ToString());
                sb.Clear();

            }
        }

        static void EditBook()
        {

            Console.WriteLine("Enter the name of the book you want to edit: ");
            string name = Console.ReadLine(); // Book name to search

            bool flag = false;

            // Loop to find the book by name
            for (int i = 0; i < Books.Count; i++)
            {
                if (Books[i].BName == name)
                {
                    Console.WriteLine("Book found!");
                    Console.WriteLine("Book Author is : " + Books[i].BAuthor);

                    // Ask for new book details
                    Console.WriteLine("Edit the book name: ");
                    string NewBook = Console.ReadLine();

                    Console.WriteLine("Edit the book Author name:  ");
                    string NewAuthor = Console.ReadLine();

                    Console.WriteLine("Enter how many books you want:");
                    int qu = int.Parse(Console.ReadLine()); // Quantity user wants to modify
                    int newQuantity = Books[i].quantity + qu; // Modify the current quantity

                    // Ensure that quantity doesn't go below zero
                    if (newQuantity < 0)
                    {
                        Console.WriteLine("Error: Not enough books in stock to remove that quantity.");
                        return;
                    }

                    // Update the book details
                    Books[i] = (NewBook, NewAuthor, Books[i].BID, newQuantity);
                    Console.WriteLine($"You have successfully updated the book '{name}'. New quantity: {newQuantity}");

                    flag = true;
                    break;
                }
            }

            if (!flag)
            {
                Console.WriteLine("Book not found.");
            }


        }

        static void RemoveBook()
        {
            Console.WriteLine("Enter the name of the book you want to remove: ");
            string name = Console.ReadLine(); // Book name to search

            bool flag = false;

            // Loop to find the book by name
            for (int i = 0; i < Books.Count; i++)
            {
                if (Books[i].BName == name)
                {
                    Console.WriteLine("Book found!");
                    Console.WriteLine("Book Author is : " + Books[i].BAuthor);
                    Console.WriteLine("Are you sure you want to remove this book? (Y/N)");
                    string confirm = Console.ReadLine().ToUpper();

                    if (confirm == "Y")
                    {
                        Books.RemoveAt(i); // Remove the book from the list
                        Console.WriteLine($"The book '{name}' has been removed successfully.");
                        flag = true;
                    }
                    else
                    {
                        Console.WriteLine("Book removal cancelled.");
                    }
                    break; // Exit the loop once the book is found and processed
                }
            }

            if (!flag)
            {
                Console.WriteLine("Book not found.");
            }
        }

        static void DisplayStatic()
        {
            // Calculate and display the total number of books in the library
            int totalBooks = 0;
            for (int i = 0; i < Books.Count; i++)
            {
                totalBooks += Books[i].quantity;
            }
            Console.WriteLine("Total books in the library: " + totalBooks);
            Console.WriteLine();

            // Variables to track total borrowed and returned books
            int totalBooksBorrowed = 0;
            int totalBooksReturned = 0;

            // Loop through Borrowing list to count borrowed and returned books
            for (int i = 0; i < Borrowing.Count; i++)
            {
                if (Borrowing[i].Returned == false)  // Check if the book is not returned
                {
                    totalBooksBorrowed++;
                }
                else if (Borrowing[i].Returned == true)  // Check if the book is returned
                {
                    totalBooksReturned++;
                }
            }

            Console.WriteLine("Total number of books borrowed (not returned): " + totalBooksBorrowed);
            Console.WriteLine("Total number of books returned: " + totalBooksReturned);
            Console.WriteLine();

            // Variables to track book with the max and min borrowers
            int maxBorrowers = -1;
            int minBorrowers = int.MaxValue;
            string maxBorrowedBook = "";
            string minBorrowedBook = "";

            // Display the number of users borrowing each book along with the author name
            for (int i = 0; i < Books.Count; i++)
            {
                int bookId = Books[i].BID;
                int usersBorrowing = 0;
                int booksReturned = 0;

                // Loop through the Borrowing list to count users borrowing and books returned for each book
                for (int j = 0; j < Borrowing.Count; j++)
                {
                    if (Borrowing[j].BID == bookId)
                    {
                        if (Borrowing[j].Returned)
                        {
                            booksReturned++;
                        }
                        else
                        {
                            usersBorrowing++;
                        }
                    }
                }

                Console.WriteLine("Book Name: " + Books[i].BName);
                Console.WriteLine("Author Name: " + Books[i].BAuthor);
                Console.WriteLine("Number of users borrowing this book: " + usersBorrowing);
                Console.WriteLine("Number of books returned: " + booksReturned);
                Console.WriteLine();

                // Update the max and min borrowers
                if (usersBorrowing > maxBorrowers)
                {
                    maxBorrowers = usersBorrowing;
                    maxBorrowedBook = Books[i].BName;
                }

                if (usersBorrowing < minBorrowers)
                {
                    minBorrowers = usersBorrowing;
                    minBorrowedBook = Books[i].BName;
                }
            }

            // Display the book with the maximum and minimum number of borrowers
            if (maxBorrowers != -1)
            {
                Console.WriteLine("Book with the most borrowers: " + maxBorrowedBook + " (Borrowed by " + maxBorrowers + " users)");
            }

            if (minBorrowers != int.MaxValue)
            {
                Console.WriteLine("Book with the fewest borrowers: " + minBorrowedBook + " (Borrowed by " + minBorrowers + " users)");
            }
        }




        static void SearchForBook()
        {
            Console.WriteLine("Enter the book name you want:");
            string name = Console.ReadLine();
            bool flag = false;

            for (int i = 0; i < Books.Count; i++)
            {
                if (Books[i].BName == name)
                {
                    Console.WriteLine("Book found!");
                    Console.WriteLine("Book Author: " + Books[i].BAuthor);
                    Console.WriteLine("Available Quantity: " + Books[i].quantity); // Show available quantity
                    flag = true;
                    break;
                }
            }

            if (!flag)
            {
                Console.WriteLine("Book not found.");
            }
        }


        static void BorrowBook()
        {
            int defaultUserId = 1;  // Assuming default user ID for now
            List<int> borrowedBookIds = new List<int>(); // To keep track of borrowed book IDs

            Console.WriteLine("User ID: " + defaultUserId);

            // Loop to get multiple books
            while (true)
            {
                Console.WriteLine("Enter the Book name you want to borrow (or type 'done' to finish):");
                string bookName = Console.ReadLine();

                if (bookName.Equals("done", StringComparison.OrdinalIgnoreCase))
                {
                    break;  // Exit the loop if the user is done entering books
                }

                Console.WriteLine("Enter the author name:");
                string authorName = Console.ReadLine();

                Console.WriteLine("Enter the quantity you want to borrow:");
                int quantityToBorrow = int.Parse(Console.ReadLine());

                // Find the book by book name and author name
                bool bookFound = false;

                for (int i = 0; i < Books.Count; i++)
                {
                    if (Books[i].BName.Equals(bookName, StringComparison.OrdinalIgnoreCase) &&
                        Books[i].BAuthor.Equals(authorName, StringComparison.OrdinalIgnoreCase))
                    {
                        if (Books[i].quantity >= quantityToBorrow)
                        {
                            // Decrease the quantity of the book in stock
                            Books[i] = (Books[i].BName, Books[i].BAuthor, Books[i].BID, Books[i].quantity - quantityToBorrow);

                            // Add to borrowing list (defaultUserId, bookId, false for not returned)
                            for (int j = 0; j < quantityToBorrow; j++)
                            {
                                Borrowing.Add((defaultUserId, Books[i].BID, false));  // 'false' means the book has not been returned
                                borrowedBookIds.Add(Books[i].BID); // Track borrowed book IDs
                            }

                            Console.WriteLine("Book(s) borrowed successfully!");
                            bookFound = true;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Sorry, insufficient quantity in stock.");
                            bookFound = true;
                            break;
                        }
                    }
                }

                if (!bookFound)
                {
                    Console.WriteLine("Book not found.");
                }
            }

            
            if (borrowedBookIds.Count > 0)
            {
                var recommendations = GetRecommendations(borrowedBookIds);

                Console.WriteLine("Recommended Books:");
                foreach (var book in recommendations)
                {
                    Console.WriteLine($"Title: {book.BName}, Author: {book.BAuthor}, ID: {book.BID}");
                }
            }
        }
        static void ViewRecommendations()
        {
            Console.WriteLine("Enter the IDs of books you want to get recommendations for (comma-separated):");
            var input = Console.ReadLine();

            // Parse the input into a list of book IDs
            var bookIdsToBorrow = input.Split(',')
                                       .Select(id => int.TryParse(id.Trim(), out int result) ? result : (int?)null)
                                       .Where(id => id.HasValue)
                                       .Select(id => id.Value)
                                       .ToList();

            if (bookIdsToBorrow.Count > 0)
            {
                // Get recommendations based on the provided book IDs
                var recommendations = GetRecommendations(bookIdsToBorrow);

                Console.WriteLine("Recommended Books:");
                foreach (var book in recommendations)
                {
                    Console.WriteLine($"Title: {book.BName}, Author: {book.BAuthor}, ID: {book.BID}");
                }
            }
            else
            {
                Console.WriteLine("No valid book IDs entered.");
            }
        }


        static void ReturnBooks()
        {
            // Set a default user ID
            int defaultUserId = 1;  // Change this value as needed

            Console.WriteLine("User ID set to default: " + defaultUserId);

            Console.WriteLine("Enter the Book Name you want to return:");
            string bookName = Console.ReadLine();

            // Find the book by name
            bool bookFound = false;
            for (int i = 0; i < Books.Count; i++)
            {
                if (Books[i].BName.Equals(bookName))
                {
                    // Find the borrowed entry and mark it as returned
                    bool borrowingFound = false;
                    for (int j = 0; j < Borrowing.Count; j++)
                    {
                        if (Borrowing[j].BID == Books[i].BID && Borrowing[j].UrId == defaultUserId && Borrowing[j].Returned == false)
                        {
                            Borrowing[j] = (Borrowing[j].UrId, Borrowing[j].BID, true);  // Mark as returned
                            Console.WriteLine("Book returned successfully!");
                            borrowingFound = true;
                            break;
                        }
                    }

                    if (!borrowingFound)
                    {
                        Console.WriteLine("No unreturned books found for the default user.");
                    }

                    bookFound = true;
                    break;
                }
            }

            if (!bookFound)
            {
                Console.WriteLine("Book not found.");
            }
        }

        static void LoadBooksFromLibFile()

        {
            try
            {
                if (File.Exists(filePath))
                {
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var parts = line.Split(',');
                            if (parts.Length == 4)
                            {
                                Books.Add((parts[0], parts[1], int.Parse(parts[2]), int.Parse(parts[3])));
                            }
                        }
                    }
                    Console.WriteLine("Books loaded from file successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading from file: {ex.Message}");
            }
        }

        static void LoadBooksFromAdminFile()
        {
            try
            {
                if (File.Exists(AdminPath))
                {
                    using (StreamReader reader = new StreamReader(AdminPath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var parts = line.Split(',');
                            if (parts.Length == 2)
                            {
                                Admin.Add((parts[0], int.Parse(parts[1])));
                            }
                        }
                    }
                    Console.WriteLine("Books loaded from file successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading from file: {ex.Message}");
            }

        }

        static void LoadBooksFromUserFile()
        {
            try
            {
                if (File.Exists(UserPath))
                {
                    using (StreamReader reader = new StreamReader(UserPath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var parts = line.Split(',');
                            if (parts.Length == 3)
                            {
                                User.Add((parts[0], int.Parse(parts[1]), int.Parse(parts[2])));
                            }
                        }
                    }
                    Console.WriteLine("user loaded from file successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading from file: {ex.Message}");
            }


        }

        static void LoadBooksFromBorrowingFile()
        {
            try
            {
                if (File.Exists(BorrowingPath))
                {
                    using (StreamReader reader = new StreamReader(BorrowingPath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var parts = line.Split(',');
                            if (parts.Length == 3)
                            {
                                Borrowing.Add((int.Parse(parts[0]), int.Parse(parts[1]), bool.Parse(parts[2])));
                            }
                        }
                    }
                    Console.WriteLine("Books loaded from file successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading from file: {ex.Message}");
            }


        }



        static void SaveBooksToPathFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (var book in Books)
                    {
                        writer.WriteLine($"{book.BName}|{book.BAuthor}|{book.BID}");
                    }
                }
                Console.WriteLine("Books saved to file successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to file: {ex.Message}");
            }
        }

        static void SaveToAdminFile()
        {
            try
            {
                // Validate the file path
                if (string.IsNullOrWhiteSpace(AdminPath))
                {
                    Console.WriteLine("Error: Invalid file path.");
                    return;
                }

                // Open the file for writing (overwrite mode)
                using (StreamWriter writer = new StreamWriter(AdminPath, false))
                {
                    // Check if the Admin list contains any valid entries
                    foreach (var admin in Admin)
                    {
                        if (!string.IsNullOrWhiteSpace(admin.AdEmail) && admin.password > 0)
                        {
                            writer.WriteLine($"{admin.AdEmail},{admin.password}");
                        }
                        else
                        {
                            Console.WriteLine($"Warning: Skipping invalid admin data: {admin.AdEmail}, {admin.password}");
                        }
                    }
                }
                Console.WriteLine("Admin data saved to file successfully.");
            }
            catch (Exception ex)
            {
                // Display detailed error message
                Console.WriteLine($"Error saving to file: {ex.Message}");
            }

        }

        static void SaveToUserFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(UserPath))
                {
                    foreach (var user in User)
                    {
                        writer.WriteLine($"{user.UrEmail},{user.password},{user.UrId}");
                    }
                }
                Console.WriteLine("user saved to file successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to file: {ex.Message}");
            }
        }

        static void SaveToBorrowingFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(BorrowingPath))
                {
                    foreach (var borrow in Borrowing)
                    {
                        writer.WriteLine($"{borrow.BID}|{borrow.UrId}");
                    }
                }
                Console.WriteLine("user saved to file successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to file: {ex.Message}");
            }
        }
        static void RegisterUser(string email, string password)
        {
            using (StreamWriter writer = new StreamWriter(UserPath, true)) // 'true' to append
            {
                writer.WriteLine($"{email},{password}");
            }
        }

        static void RegisterAdmin(string email, string password)
        {
            static void RegisterAdmin(string email, string password)
            {
                using (StreamWriter writer = new StreamWriter(AdminPath, true)) // 'true' to append
                {
                    writer.WriteLine($"{email},{password}");
                }
            }
        }

        static List<(string BName, string BAuthor, int BID, int Quantity)> GetRecommendations(List<int> bookIdsToBorrow)
        {
            var recommendations = new Dictionary<int, int>();

            // Iterate through the Borrowing list
            for (int i = 0; i < Borrowing.Count; i++)
            {
                bool allBooksPresent = true;

                // Check if the current borrowing record includes all the books the user wants to borrow
                foreach (var bookId in bookIdsToBorrow)
                {
                    bool found = Borrowing.Any(b => b.BID == bookId && b.UrId == Borrowing[i].UrId);
                    if (!found)
                    {
                        allBooksPresent = false;
                        break;
                    }
                }

                if (allBooksPresent)
                {
                    // Count other books borrowed by the same user
                    for (int m = 0; m < Borrowing.Count; m++)
                    {
                        if (Borrowing[m].UrId == Borrowing[i].UrId && !bookIdsToBorrow.Contains(Borrowing[m].BID))
                        {
                            if (recommendations.ContainsKey(Borrowing[m].BID))
                            {
                                recommendations[Borrowing[m].BID]++;
                            }
                            else
                            {
                                recommendations[Borrowing[m].BID] = 1;
                            }
                        }
                    }
                }
            }

            var recommendedBooks = new List<(string BName, string BAuthor, int BID, int Quantity)>();

            foreach (var recommendation in recommendations)
            {
                int bookId = recommendation.Key;
                int score = recommendation.Value;
                var book = Books.FirstOrDefault(b => b.BID == bookId);

                if (book.BID != 0)
                {
                    recommendedBooks.Add((book.BName, book.BAuthor, book.BID, score));
                }
            }

            // Sort by recommendation score in descending order
            recommendedBooks.Sort((a, b) => b.Quantity.CompareTo(a.Quantity));

            return recommendedBooks;
        }
    }
}


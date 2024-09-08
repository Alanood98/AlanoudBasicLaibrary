using System;
using System.Text;
using System.Xml.Linq;
using static System.Reflection.Metadata.BlobBuilder;

namespace BasicLibrary
{
    internal class Program
    {
        static List<(string BName, string BAuthor, int BID, int quantity)> Books = new List<(string BName, string BAuthor, int BID, int quntity)>();
        static List<(string AdEmail, int password)> admin = new List<(string AdEmail, int password)>();
        static List<(string UrEmail, int password, int UrId)> user = new List<(string UrEmail, int password, int UrId)>();
        static List<(int UrId, int BID)> borrowing = new List<(int Id, int BID)>();
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
                Console.WriteLine("Choose 1 for Login Or 2 for Admin Or 3 for  User  or 4 for log out:");
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
                        SaveBooksToAdminFile();
                        ExitFlag = true;
                        break;
                    case "6":
                        SaveBooksToUserFile();
                        ExitFlag = true;
                        break;
                    case "7":
                        SaveBooksToBorrowingFile();
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
            bool ExitFlag = false;
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
                    Console.WriteLine("User not registered or incorrect credentials.");
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
                    Console.WriteLine("Admin not registered or incorrect credentials.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter 'admin' or 'user'.");
                Console.Clear();
            }
        }

        // Function to check user login credentials from file
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
                        if (data[0] == email && data[1] == password)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        // Function to check admin login credentials from file
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
                        if (data[0] == email && data[1] == password)
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
                Console.WriteLine("\n G- Save and Exit");

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

                    case "G":
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
                Console.WriteLine("\n D- Save and Exit");

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
            Console.WriteLine("Enter the name of the book you want to borrow:");
            string name = Console.ReadLine();
            bool flag = false;

            for (int i = 0; i < Books.Count; i++)
            {
                if (Books[i].BName == name)
                {
                    Console.WriteLine("Book Author is : " + Books[i].BAuthor);
                    Console.WriteLine("how manay book you want:");
                    int qu = int.Parse(Console.ReadLine());
                    int x = Books[i].quantity - qu;
                    Books[i] = (Books[i].BName, Books[i].BAuthor, Books[i].BID, x);
                    Console.WriteLine($"You have successfully borrowed '{name}'.");
                    return;
                    flag = true;
                    break;
                }
            }
            //    for (int i = 0; i < Books.Count; i++)
            //    {
            //        if (Books[i].quantity!=0)
            //        {
            //            Console.WriteLine("how manay book you want:");
            //            int qu =int.Parse(Console.ReadLine());
            //            int x = Books[i].quantity - qu; // Increase the number of available books
            //            Books[i] = (Books[i].BName, Books[i].BAuthor, Books[i].ID, x);
            //            Console.WriteLine($"You have successfully returned '{bookName}'.");
            //            return;
            //        }
            //      //  .Equals(bookName, StringComparison.OrdinalIgnoreCase)
            //    }
            //    Console.WriteLine("Book not found.");
        }

        static void ReturnBooks()
        {
            Console.WriteLine("Enter the name of the book you want to return:");
            string name = Console.ReadLine();
            bool flag = false;

            for (int i = 0; i < Books.Count; i++)
            {
                if (Books[i].BName == name)
                {
                    int y = Books[i].quantity + 1; // Increase the number of available books
                    Books[i] = (Books[i].BName, Books[i].BAuthor, Books[i].BID, y);
                    Console.WriteLine($"You have successfully returned '{name}'.");
                    return;
                }
            }
            Console.WriteLine("Book not found.");
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

        static void SaveBooksToAdminFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(AdminPath))
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

        static void SaveBooksToUserFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(AdminPath))
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

        static void SaveBooksToBorrowingFile()
        {

        }
    }
}

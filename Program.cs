using System.Text;
using static System.Reflection.Metadata.BlobBuilder;

namespace BasicLibrary
{
    internal class Program
    {
        static List<(string BName, string BAuthor, int BID , int quantity)> Books = new List<(string BName, string BAuthor, int BID , int quntity)>();
        static List<(string AdEmail,  int password)> admin = new List<(string AdEmail, int password)>();
        static List<(string UrEmail, int password , int UrId)> user = new List<(string UrEmail, int password , int UrId)>();
        static List<(int UrId ,  int BID)> borrowing = new List<(int Id , int BID)>();
        static string filePath = "C:\\Users\\Codeline User\\Desktop\\lib.txt";
        static string UserPath = "C:\\Users\\Codeline User\\Desktop\\Admin.txt";
        static string AdminPath = "C:\\Users\\Codeline User\\Desktop\\User.txt";
        static string BorrowingPath = "C:\\Users\\Codeline User\\Desktop\\Borrowing.txt";
        // testing chuckout
        static void Main(string[] args)
        {
            // downloaded form Alanoud device 
                bool ExitFlag = false;

                LoadBooksFromFile();

                do
                {
                    Console.WriteLine("Choose 1 for admin Or 2 for user Or 3 for Save & Exit:");
                    string choice = Console.ReadLine();
                    switch (choice)
                    {

                        case "1":
                            AdminMenu();
                            break;

                        case "2":
                            UserMenu();
                            break;

                        case "3":
                            SaveBooksToFile();
                            ExitFlag = true;
                            break;

                        default:
                            Console.WriteLine("enter correct choice");
                            break;
                    }
                } while (ExitFlag != true);


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
                Console.WriteLine("\n D- Save and Exit");

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
                sb.Append("Book ").Append(BookNumber).Append(" ID : ").Append(Books[i].ID);
                sb.AppendLine().AppendLine();
                sb.Append("Book ").Append(BookNumber).Append(" Quantity : ").Append(Books[i].quantity);
                sb.AppendLine().AppendLine();
                Console.WriteLine(sb.ToString());
                sb.Clear();

            }
        }

        static void SearchForBook()
        {
            Console.WriteLine("Enter the book name you want");
            string name = Console.ReadLine();
            bool flag = false;

            for (int i = 0; i < Books.Count; i++)
            {
                if (Books[i].BName == name)
                {
                    Console.WriteLine("Book Author is : " + Books[i].BAuthor);
                    flag = true;
                    break;
                }
            }

            if (flag != true)
            { Console.WriteLine("book not found"); }
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
                    Books[i] = (Books[i].BName, Books[i].BAuthor, Books[i].ID, x);
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
                    Books[i] = (Books[i].BName, Books[i].BAuthor, Books[i].ID, y);
                    Console.WriteLine($"You have successfully returned '{name}'.");
                    return;
                }
            }
            Console.WriteLine("Book not found.");
        }

        static void LoadBooksFromFile()
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

        static void SaveBooksToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (var book in Books)
                    {
                        writer.WriteLine($"{book.BName}|{book.BAuthor}|{book.ID}");
                    }
                }
                Console.WriteLine("Books saved to file successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to file: {ex.Message}");
            }
        }

    }
}

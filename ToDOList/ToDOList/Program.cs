using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;

namespace ToDOList
{
    internal class Program
    {
        static UsersPage user = new UsersPage();

        static void Main(string[] args)
        {
            while (true)
            {

                Console.WriteLine("\t<<<<<<<<<<<<<<<<<<<<<<<<< WELCOME! >>>>>>>>>>>>>>>>>>>>>>>>>\t");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Exit");
                int userResponse;
                while (true)
                {
                    Console.Write("Enter your choice: ");
                    if(int.TryParse(Console.ReadLine(), out userResponse))
                    {
                        if(userResponse >= 1 && userResponse <= 3)
                        {
                            break;
                        }                        
                    }
                    Console.ForegroundColor= ConsoleColor.Red;
                    Console.WriteLine("Invalid response. Please enter number between 1 and 3");
                    Console.ResetColor();
                }
                Console.WriteLine();

                switch (userResponse)
                {
                    case 1:
                        user.Register();
                        
                        break;
                    case 2:
                        user.Login();
                        
                        break;
                    case 3:
                        Console.WriteLine("Exiting application...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
                Console.WriteLine();

            }
        }


    }
}

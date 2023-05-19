using System;
using System.Text.RegularExpressions;

namespace ToDOList
{
    internal class Validations
    {
        public static bool ValidateEntry(string input)
        {
            string pattern = @"^[a-zA-Z ]*$";

            if (Regex.IsMatch(input, pattern))
            {
                return true;

            }
            else
            {
                return false;
            }

        }

        public static bool ValidatePriorityLevel(string input)
        {
            if (input.ToLower() != "low" && input.ToLower() != "medium" && input.ToLower() != "high")
            {
                return true;
            }
            return false;
        }

        public static int ValidateEdit(string prompt)
        {
            int value;
            while (true)
            {
                Console.Write(prompt);
                string idInput = Console.ReadLine();

                if (string.IsNullOrEmpty(idInput))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Please enter a valid task ID to Complete.");
                    Console.ResetColor();
                    continue;
                }

                if (int.TryParse(idInput, out value))
                {
                    break;
                }
            }
            return value;
        }
    }
}

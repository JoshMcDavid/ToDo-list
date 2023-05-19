using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


namespace ToDOList
{
    internal class UsersPage
    {
        private readonly string userNameMesssage = "Please Insert your Name: ";
        private readonly string taskTitleMesssage = "Enter task title: ";
        private readonly string taskDescriptionMesssage = "Enter task description: ";
        private readonly string taskDueDateMesssage = "Enter due date (YYYY-MM-DD): ";
        private readonly string DueDateEditMesssage = "Enter current or future date (YYYY-MM-DD)";
        private readonly string taskpriorityMesssage = "Enter priority level ('Low', 'Medium', 'High'): ";
        static readonly int tableWidth = 90;
        private readonly DateTime currentDate = DateTime.Now.Date;
        public List<UserLogins> users = new List<UserLogins>();
        public UserLogins currentUser = new UserLogins();

        public void MainMenu()
        {
            while (true)
            {
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Exit");

                int choice;
                while (true)
                {
                    Console.Write("Enter your choice: ");
                    if (int.TryParse(Console.ReadLine(), out choice))
                    {
                        if (choice >= 1 && choice <= 3)
                        {
                            break;
                        }
                    }
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid response. Please enter a number between 1 and 3.");
                    Console.ResetColor();
                }
                Console.WriteLine();

                switch (choice)
                {
                    case 1:
                        Register();
                        break;
                    case 2:
                        Login();
                        break;
                    case 3:
                        Console.Clear();
                        Console.WriteLine("Exiting application...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
                Console.WriteLine();
            }
        }

        public bool UserExist(string email)
        {
            foreach (UserLogins user in users)
            {
                if (user.Email == email)
                {
                    return true;
                }
            }
            return false;
        }
        public void Register()
        {
            Console.Clear();
            string fName;
            Console.Write(userNameMesssage);
            while (true)
            {
                fName = Console.ReadLine().Trim();

                bool checkUserName = Validations.ValidateEntry(fName);
                if (string.IsNullOrEmpty(fName))
                {
                    DisplayErrorMessage("Name cannot be empty. try again", userNameMesssage);
                }
                else if (!checkUserName)
                {
                    DisplayErrorMessage("Name Shouldn't contain digits or symbols", userNameMesssage);
                }
                else
                {
                    break;
                }
            }

            string email;
            bool isCorrect = false;
            do
            {
                Console.Write("Enter an email address: ");
                email = Console.ReadLine();

                if (Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    isCorrect = true;
                    while (UserExist(email))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Username already exists. Try again.: ");
                        Console.ResetColor();
                        email = Console.ReadLine();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid email address. Please try again.");
                    Console.ResetColor();
                }
            } while (!isCorrect);

            string password;
            bool isRight = false;
            string password2;
            bool isSame = false;
            do
            {
                Console.Write("Create a password: ");
                password = Console.ReadLine();

                if (password.Length >= 8 &&
                    password.Any(char.IsUpper) &&
                    password.Any(char.IsLower) &&
                    password.Any(char.IsDigit))
                {
                    isRight = true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid password. Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, and one digit.");
                    Console.ResetColor();
                }
            } while (!isRight);


            do
            {
                Console.Write("Confirm Password: ");
                password2 = Console.ReadLine();
                if (password == password2)
                {
                    isSame = true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid password. Password does not match.");
                    Console.ResetColor();
                }
            } while (!isSame);
            UserLogins user = new UserLogins(fName, email, password);

            currentUser = user;
            users.Add(currentUser);
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Registration successful!");
            Console.ResetColor();

            if (user == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid email or password. Try again.");
                Console.ResetColor();

            }
            currentUser = user;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Welcome {0}!", email);
            Console.ResetColor();

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("1. Add task");
                Console.WriteLine("2. View all tasks");
                Console.WriteLine("3. Edit task");
                Console.WriteLine("4. Delete task");
                Console.WriteLine("5. Complete task");
                Console.WriteLine("6. Logout");

                int userResponse1;
                while (true)
                {
                    Console.Write("Enter your choice: ");
                    if (int.TryParse(Console.ReadLine(), out userResponse1))
                    {
                        if (userResponse1 >= 1 && userResponse1 <= 6)
                        {
                            break;
                        }
                    }
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid response. Please enter number between 1 and 6");
                    Console.ResetColor();
                }

                switch (userResponse1)
                {
                    case 1:
                        AddTask();
                        break;
                    case 2:
                        ViewAllTasks();
                        break;
                    case 3:
                        EditTask();
                        break;
                    case 4:
                        DeleteTask();
                        break;
                    case 5:
                        CompleteTask();
                        break;
                    case 6:
                        Console.Clear();
                        Console.WriteLine("Logging out... Thank You!");
                        currentUser = null;
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        public UserLogins GetUserLogin(string email, string password)
        {
            foreach (UserLogins user in users)
            {
                if (user.Email == email && user.Password == password)
                {
                    return user;
                }
            }
            return null;
        }

        public void Login()
        {
            Console.Clear();
            Console.Write("Enter email: ");
            string email = Console.ReadLine();

            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            UserLogins user = GetUserLogin(email, password);

            if (user == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid email or password. Try again.");
                Console.ResetColor();
                return;
            }
            currentUser = user;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Welcome {0}!", email);
            Console.ResetColor();

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("1. Add task");
                Console.WriteLine("2. View all tasks");
                Console.WriteLine("3. Edit task");
                Console.WriteLine("4. Delete task");
                Console.WriteLine("5. Complete task");
                Console.WriteLine("6. Logout");

                int userResponse1;
                while (true)
                {
                    Console.Write("Enter your choice: ");
                    if (int.TryParse(Console.ReadLine(), out userResponse1))
                    {
                        if (userResponse1 >= 1 && userResponse1 <= 6)
                        {
                            break;
                        }
                    }
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid response. Please enter number between 1 and 6");
                    Console.ResetColor();
                }

                switch (userResponse1)
                {
                    case 1:
                        AddTask();
                        break;
                    case 2:
                        ViewAllTasks();
                        break;
                    case 3:
                        EditTask();
                        break;
                    case 4:
                        DeleteTask();
                        break;
                    case 5:
                        CompleteTask();
                        break;
                    case 6:
                        Console.Clear();
                        Console.WriteLine("Logging out...");
                        currentUser = null;
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }

        }

        public void AddTask()
        {
            Console.Clear();
            string title;
            Console.Write(taskTitleMesssage);

            while (true)
            {
                title = Console.ReadLine().Trim();
                if (string.IsNullOrEmpty(title))
                {
                    DisplayErrorMessage("Title cannot be empty. try again", taskTitleMesssage);
                }
                else
                {
                    break;
                }

            }
            string description;
            Console.Write(taskDescriptionMesssage);

            while (true)
            {
                description = Console.ReadLine().Trim();
                if (string.IsNullOrEmpty(description))
                {
                    DisplayErrorMessage("Description cannot be empty. try again", taskDescriptionMesssage);
                }
                else
                {
                    break;
                }

            }
            DateTime dueDate;

            Console.Write(taskDueDateMesssage);

            while (!DateTime.TryParse(Console.ReadLine(), out dueDate) || dueDate <= currentDate)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                DisplayErrorMessage("Invalid date.", taskDueDateMesssage);
            }

            string priorityLevel;
            Console.Write(taskpriorityMesssage);

            do
            {
                priorityLevel = Console.ReadLine();
                bool checkPriority = Validations.ValidatePriorityLevel(priorityLevel);
                if (string.IsNullOrEmpty(priorityLevel))
                {
                    DisplayErrorMessage("Priority level cannot be empty.", taskpriorityMesssage);
                }
                else if (checkPriority)
                {
                    DisplayErrorMessage("Invalid priority level.", taskpriorityMesssage);
                }
                else
                {
                    break;
                }
            } while (priorityLevel.ToLower() != "low" && priorityLevel.ToLower() != "medium" && priorityLevel.ToLower() != "high");

            Task task = new Task(title, description, dueDate, priorityLevel);
            currentUser.tasks.Add(task);
            task.Id = currentUser.Id;

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Task added successfully!");
            Console.ResetColor();
        }

        static void PrintLine()
        {
            Console.WriteLine(new string('-', tableWidth));
        }

        static void PrintRow(params string[] columns)
        {
            int width = (tableWidth - columns.Length + 1) / columns.Length;
            string row = "|";
            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }
            Console.WriteLine(row);
        }

        static string AlignCentre(string text, int width)
        {
            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;
            return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
        }

        static string CentreText(string text, int width)
        {
            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            int totalSpaces = width - text.Length;
            int leftSpaces = totalSpaces / 2;
            return new string(' ', leftSpaces) + text + new string(' ', totalSpaces - leftSpaces);
        }

        public void ViewAllTasks()
        {
            Console.Clear();
            Console.WriteLine(CentreText("Todo App", tableWidth));
            Console.WriteLine();
            Console.WriteLine($"Name: {currentUser.Name}\nEmail: {currentUser.Email}\nUserID: {currentUser.Id}\nNumber of Tasks: {currentUser.tasks.Count}");
            PrintLine();
            PrintRow("ID", "TITLE", "DESCRIPTION", "DUE DATE", "PRIORITY", "STATUS");
            PrintLine();
            foreach (Task task in currentUser.tasks)
            {
                int index = currentUser.tasks.IndexOf(task) + 1;
                PrintRow(index.ToString(), task.title, task.description,
                    task.dueDate.ToShortDateString(), task.priorityLevel, task.status);
            }
            PrintLine();
        }

        public Task GetTaskById(int id)
        {
            foreach (Task task in currentUser.tasks)
            {
                if (task.id == id)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Task Title: {task.title}");
                    Console.WriteLine($"Task Description: {task.description}");
                    Console.WriteLine($"Task Due Date: {task.dueDate}");
                    Console.WriteLine($"Task Priority Level: {task.priorityLevel}");
                    Console.WriteLine($"Task Status: {task.status}");
                    Console.ResetColor();
                    return task;
                }
            }

            return null;
        }

        public void EditTask()
        {
            int id = Validations.ValidateEdit("Enter task ID to Edit: ");

            Console.Clear();
            Task taskToEdit = GetTaskById(id);

            if (taskToEdit == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Task not found. Try again.");
                Console.ResetColor();
                return;
            }

            Console.Write("Enter new title (or press enter to keep the current title): ");
            string newTitle = Console.ReadLine();
            if (!string.IsNullOrEmpty(newTitle))
            {
                taskToEdit.title = newTitle;
            }


            Console.Write("Enter new description (or press enter to keep the current description): ");
            string newDescription = Console.ReadLine();
            if (!string.IsNullOrEmpty(newDescription))
            {
                taskToEdit.description = newDescription;
            }

            while (true)
            {
                Console.Write("Enter new due date (YYYY-MM-DD) or press enter to keep the current due date: ");
                string newDueDate = Console.ReadLine();

                if (string.IsNullOrEmpty(newDueDate))
                {
                    break;
                }
                if (DateTime.TryParse(newDueDate, out DateTime userInput))
                {
                    if (userInput.Date >= DateAndTime.Now)
                    {
                        taskToEdit.dueDate = userInput;
                        break;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(DueDateEditMesssage);
                        Console.ResetColor();
                    }
                }
                break;
            }

            while (true)
            {
                Console.Write("Enter new priority level (or press enter to keep the current priority level): ");
                string newPriorityLevel = Console.ReadLine();

                if (string.IsNullOrEmpty(newPriorityLevel))
                {
                    break;
                }
                else if (!Validations.ValidatePriorityLevel(newPriorityLevel))
                {
                    taskToEdit.priorityLevel = newPriorityLevel;
                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(taskpriorityMesssage);
                    Console.ResetColor();
                }
            }

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Task edited successfully!");
            Console.ResetColor();
        }

        public Task GetTask(int id)
        {
            foreach (Task task in currentUser.tasks)
            {
                if (task.id == id)
                {
                    return task;
                }
            }
            return null;
        }
        public void DeleteTask()
        {
            int id = Validations.ValidateEdit("Enter task ID to Delete: ");
            Task task = GetTask(id);

            if (task == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Task not found. Try again.");
                Console.ResetColor();
                return;
            }

            currentUser.tasks.Remove(task);
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Task deleted successfully!");
            Console.ResetColor();
        }

        public void CompleteTask()
        {
            int id = Validations.ValidateEdit("Enter task ID to Complete: ");
            Task task = GetTask(id);

            if (task == null)
            {
                Console.ForegroundColor = (ConsoleColor.Red);
                Console.WriteLine("Task not found. Try again.");
                Console.ResetColor();
                return;
            }

            task.status = "Complete";

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Task completed successfully!");
            Console.ResetColor();
        }

        private static void DisplayErrorMessage(string message, string retryMessage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error: {message}");
            Console.ResetColor();
            Console.WriteLine(retryMessage);
        }

        public void Start()
        {
            Console.WriteLine("\t<<<<<<<<<<<<<<<<<<<<<<<<< WELCOME! >>>>>>>>>>>>>>>>>>>>>>>>>\t");

            MainMenu();
        }
    }
}

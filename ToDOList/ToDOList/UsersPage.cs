using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ToDOList
{
    public class UsersPage
    {
        public List<UserLogins> users = new List<UserLogins>();
        public UserLogins currentUser = null;


        public bool UserExist(string email)
        {
            foreach(UserLogins user in users)
            {
                if(user.email == email)
                {
                    return true;
                }
            }
            return false;
        }
        public void Register()
        {
            string fName;
            bool isValid = false;

            do
            {
                Console.Write("Enter your Name: ");
                fName = Console.ReadLine();

                if (!int.TryParse(fName, out _) &&
                    !double.TryParse(fName, out _) &&
                    !DateTime.TryParse(fName, out _))
                {
                    isValid = true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Please enter a string.");
                    Console.ResetColor();
                }
            } while (!isValid);


            string email;
            bool isCorrect = false;

            do
            {
                Console.Write("Enter an email address: ");
                email = Console.ReadLine();

                if (Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    isCorrect = true;
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
            

            UserLogins user = new UserLogins(fName, email, password);

            if(UserExist(email))
            {
                Console.ForegroundColor= ConsoleColor.Red;
                Console.WriteLine("Username already exists. Try again.");
                Console.ResetColor();
                return;
            }
            
            currentUser = user;
            users.Add(currentUser);
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Registration successful!");
            Console.ResetColor();
        }

        public UserLogins GetUserLogin(string email, string password)
        {
            foreach (UserLogins user in users)
            {
                if(user.email == email && user.password == password)
                {
                    return user;
                }
            }
            return null;
        }

        public void Login()
        {
            Console.Write("Enter email: ");
            string email = Console.ReadLine();

            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            UserLogins user = GetUserLogin(email, password);

            if(user == null)
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
            Console.Write("Enter task title: ");
            string title = Console.ReadLine();

            Console.Write("Enter task description: ");
            string description = Console.ReadLine();

            Console.Write("Enter due date (YYYY-MM-DD): ");
            DateTime dueDate = DateTime.Parse(Console.ReadLine());


            string priorityLevel;
            bool isTrue = false;

            do
            {
                Console.Write("Enter priority level ('Low', 'Medium', 'High')");
                priorityLevel = Console.ReadLine();

                if (!int.TryParse(priorityLevel, out _) &&
                    !double.TryParse(priorityLevel, out _) &&
                    !DateTime.TryParse(priorityLevel, out _))
                {
                    isTrue = true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Please enter valid priority level ('Low', 'Medium', 'High')");
                    Console.ResetColor();
                }
            } while (!isTrue);

            Task task = new Task(title, description, dueDate, priorityLevel);
            currentUser.tasks.Add(task);

            Console.Clear();
            Console.WriteLine("Task added successfully!"); 
        }

        public void ViewAllTasks()
        {
            Console.WriteLine("|------------------------------------------------------------------|");
            Console.WriteLine("| ID | Title              | Description          | Due Date        |");
            Console.WriteLine("|----|--------------------|----------------------|-----------------|");
            foreach(Task task in currentUser.tasks)
            {
                Console.WriteLine("|{0,-4}|{1,-20}|{2,-22}|{3,-17}|", task.id, task.title, task.description, task.dueDate.ToString("MM/dd/yyyy"));
            }
            Console.WriteLine("|------------------------------------------------------------------|");
        }

        public Task GetTaskById(int id)
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
        public void EditTask()
        {
            Console.Write("Enter task ID: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Task taskToEdit = GetTaskById(id);

            if (taskToEdit == null)
            {
                Console.WriteLine("Task not found. Try again.");
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

            Console.Write("Enter new due date -YYYY-MM-DD (or press enter to keep the current due date): ");
            string newDueDate = (Console.ReadLine());
            if (!string.IsNullOrEmpty(newDueDate))
            {
                taskToEdit.dueDate = DateTime.Parse(newDueDate);
            }

            Console.Write("Enter new priority level (or press enter to keep the current priority level): ");
            string newPriorityLevel = Console.ReadLine();
            if (!string.IsNullOrEmpty(newPriorityLevel))
            {
                taskToEdit.priorityLevel = newPriorityLevel;
            }
            
            Console.Clear();
            Console.WriteLine("Task edited successfully!");
        }

        public Task GetTask(int id)
        {
            foreach(Task task in currentUser.tasks)
            {
                if(task.id == id)
                {
                    return task;
                }
            }
            return null;
        }
        public void DeleteTask()
        {
            Console.Write("Enter task ID: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Task task = GetTask(id);

            if (task == null)
            {
                Console.ForegroundColor= ConsoleColor.Red;
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
            Console.Write("Enter task ID: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Task task = GetTask(id);

            if (task == null)
            {
                Console.ForegroundColor=( ConsoleColor.Red);
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
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ToDOList
{
    public class UserLogins
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public List<Task> tasks;

        public UserLogins(string name, string email, string password)
        {
            this.Name = name;
            this.Email = email;
            this.Password = password;
            tasks = new List<Task>();
        }

        public UserLogins()
        {

        }


    }
}

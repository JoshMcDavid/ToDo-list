using System;
using System.Collections.Generic;
using System.Text;

namespace ToDOList
{
    public class UserLogins
    {
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }

        public List<Task> tasks;

        public UserLogins(string name, string email, string password)
        {
            this.name = name;
            this.email = email;
            this.password = password;
            tasks = new List<Task>();
        }


    }
}

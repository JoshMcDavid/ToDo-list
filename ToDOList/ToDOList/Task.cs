using System;
using System.Collections.Generic;
using System.Text;

namespace ToDOList
{
    public class Task
    {
        public int nextId = 1;
        public int id;
        public string title;
        public string description;
        public DateTime dueDate;
        public string status = "Incomplete";
        public string priorityLevel;


        public List<Task> tasks;



        public Task(string title, string description, DateTime dueDate, string priorityLevel)
        {
            this.id = nextId;
            this.title = title;
            this.description = description;
            this.dueDate = dueDate;
            this.priorityLevel = priorityLevel;
            tasks = new List<Task>();
        }
    }
}

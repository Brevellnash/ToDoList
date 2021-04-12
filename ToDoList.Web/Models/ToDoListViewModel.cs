using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Web.Models
{
    public class ToDoListViewModel
    {
        public string Title { get; set; }

        public List<ToDoTask> Tasks { get; set; }
    }
}

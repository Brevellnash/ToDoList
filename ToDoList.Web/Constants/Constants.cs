using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Web.Constants
{
    public static class Constants
    {
        public static class Views
        {
            public const string Index = "Index";

            public const string Error = "Error";

            public const string Edit = "Edit";

            public const string Create = "Create";

            public const string Delete = "Delete";
        }


        public static class Errors
        {
            public const string TaskNotFound = "Task was not found";

            public const string IdCannotBeEmpty = "Id cannot be empty";

            public const string FailedLoadOpen = "Failed to load open tasks, please try again";

            public const string FailedLoadClosed = "Failed to load completed tasks, please try again";

            public const string FailedUpdate = "Failed to update task, please try again";

            public const string FailedDelete = "Failed to delete task, please try again";

            public const string FailedCreate = "Failed to create new task, please try again";
        }
    }
}

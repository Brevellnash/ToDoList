
using System.Collections.Generic;
using ToDoList.Models;

public interface IToDoService
{

    public List<ToDoTask> Get();

    public List<ToDoTask> GetOpenTasks();

}
}

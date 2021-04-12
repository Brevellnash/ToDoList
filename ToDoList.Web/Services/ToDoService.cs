using MongoDB.Driver;
using System.Collections.Generic;
using ToDoList.Models;

namespace ToDoList.Services
{
    public class ToDoService
    {
        private readonly IMongoCollection<ToDoTask> _toDoTasks;

        public ToDoService(IToDoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _toDoTasks = database.GetCollection<ToDoTask>(settings.CollectionName);
        }


        public List<ToDoTask> Get() =>
           _toDoTasks.Find(toDoTask => true).ToList();

        public List<ToDoTask> GetOpenTasks() =>
            _toDoTasks.Find(toDoTask => !toDoTask.Completed).ToList();
    }

}

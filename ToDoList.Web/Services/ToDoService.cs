using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoList.Web.Models;

namespace ToDoList.Web.Services
{
    public class ToDoService
    {
        private readonly IMongoCollection<ToDoTask> _toDoTasks;
        private readonly ILogger _logger;

        public ToDoService(IToDoDatabaseSettings settings, ILogger<ToDoService> logger)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _toDoTasks = database.GetCollection<ToDoTask>(settings.CollectionName);
            _logger = logger;
        }


        public async Task<List<ToDoTask>> GetAsyncClosedTasks()
        {
            List<ToDoTask> listOftasks = new List<ToDoTask>();
            try
            {
                var closedTasks = await _toDoTasks.FindAsync(toDoTask => toDoTask.Completed == true);
                listOftasks = closedTasks.ToList();
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Exception encountered when trying to retrieve the closed tasks");
            }
            return listOftasks;
        }
           

        public async Task<List<ToDoTask>> GetAsyncOpenTasks()
        {
            List<ToDoTask> listOftasks = new List<ToDoTask>();
            try
            {
                var openTasks = await _toDoTasks.FindAsync(toDoTask => toDoTask.Completed == false);
                listOftasks = openTasks.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception encountered when trying to retrieve the open tasks");
            }
            return listOftasks;
        }

        public async Task<ToDoTask> GetAsyncById(string id)
        {
            try
            {
                var item = await _toDoTasks.FindAsync(toDoTask => string.Equals(toDoTask.Id, id));
                return item.First();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception encountered when trying to retrieve task with id: {id}", id);
            }
            return null;

        }
    }

}

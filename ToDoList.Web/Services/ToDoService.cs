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

        public async Task<bool> UpdateAsync(ToDoTask todo)
        {
            try
            {
                var updateFilter = Builders<ToDoTask>.Filter.Eq(td => td.Id, todo.Id);
                var result =  await _toDoTasks.ReplaceOneAsync(updateFilter, todo);
                if(result.IsAcknowledged && result.ModifiedCount > 0)
                {
                    return true;
                }
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Exception encountered when trying to replace task with id {id}", todo.Id);
            }
            return false;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            try
            {
                var result = await _toDoTasks.DeleteOneAsync(td => td.Id.Equals(id));
                if(result.IsAcknowledged && result.DeletedCount > 0)
                {
                    return true;
                }
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Exception encountered when trying to delete task with id {id}", id);
            }
            return false;
        }

        public async Task CreateAsync(ToDoTask todo)
        {
            try
            {
                await _toDoTasks.InsertOneAsync(todo);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Exception encounted when inserting new task with Title {todo.Title}", todo.Title);
            }
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

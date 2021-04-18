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

        /// <summary>
        /// Gets all tasks from the database that have been completed
        /// </summary>
        /// <returns>List of ToDoTask object representing the tasks or null on error</returns>
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
                return null;
            }
            return listOftasks;
        }

        /// <summary>
        /// Gets all tasks from the database that have not yet been completed
        /// </summary>
        /// <returns>List of ToDoTask objects that represent the tasks</returns>
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
                return null;
            }
            return listOftasks;
        }

        /// <summary>
        /// Updates the task with id todo.Id in database to match the input variable todo
        /// </summary>
        /// <param name="todo">input task to be updated</param>
        /// <returns>boolean value representing success</returns>
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

        /// <summary>
        /// Deletes the task with Id = id in the database
        /// </summary>
        /// <param name="id">the database id of the item to delete</param>
        /// <returns>boolean value representing success</returns>
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

        /// <summary>
        /// Creates a new Task in the database with the values present in the input variable
        /// </summary>
        /// <param name="todo">The task to be added to the database</param>
        /// <returns>boolean value representing the successful execution of operation</returns>
        public async Task<bool> CreateAsync(ToDoTask todo)
        {
            try
            {
                await _toDoTasks.InsertOneAsync(todo);
                return true;
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Exception encounted when inserting new task with Title {todo.Title}", todo.Title);
                return false;
            }
        }

        /// <summary>
        /// Gets a specific task with Id = id from the database
        /// </summary>
        /// <param name="id">the database id of the item you want to retrieve</param>
        /// <returns>the ToDoTaskmodel feel f</returns>
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

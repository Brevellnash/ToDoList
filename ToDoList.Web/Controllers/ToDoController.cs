using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Web.Constants;
using ToDoList.Web.Models;
using ToDoList.Web.Services;

namespace ToDoList.Controllers
{
    public class ToDoController : Controller
    {
        private readonly ILogger<ToDoController> _logger;
        private readonly ToDoService _toDoService;

        public ToDoController(ILogger<ToDoController> logger, ToDoService toDoService)
        {
            _logger = logger;
            _toDoService = toDoService;
        }

        public async Task<IActionResult> Index()
        {
            var openTasks = await _toDoService.GetAsyncOpenTasks();
            if(openTasks == null)
            {
                return View(Constants.Views.Error, new ErrorViewModel() { Message = Constants.Errors.FailedLoadOpen });
            }
            return View(new ToDoListViewModel() { Title = "To Do List", Tasks = openTasks });
        }

        public async Task<IActionResult> CompletedTasks()
        {
            var closedTasks = await _toDoService.GetAsyncClosedTasks();
            if (closedTasks == null)
            {
                return View(Constants.Views.Error, new ErrorViewModel() { Message = Constants.Errors.FailedLoadClosed });
            }
            return View(Constants.Views.Index, new ToDoListViewModel() { Title = "Completed Tasks", Tasks = closedTasks });
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View(Constants.Views.Error, new ErrorViewModel(){Message = Constants.Errors.IdCannotBeEmpty});
            }
            var item = await _toDoService.GetAsyncById(id);
            if (item == null)
            {
                return View(Constants.Views.Error, new ErrorViewModel() { Message = Constants.Errors.TaskNotFound });
            }
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, ToDoTask todo)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View(Constants.Views.Error, new ErrorViewModel() { Message = Constants.Errors.TaskNotFound });
            }
            var success = await _toDoService.UpdateAsync(todo);
            if (success)
            {
                return RedirectToAction(Constants.Views.Index);
            }
            return View(Constants.Views.Error, new ErrorViewModel() { Message = Constants.Errors.FailedUpdate });
        }

        public async Task<IActionResult> Delete (string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View(Constants.Views.Error, new ErrorViewModel() { Message = Constants.Errors.IdCannotBeEmpty });
            }
            var item = await _toDoService.GetAsyncById(id);
            if (item == null)
            {
                return View(Constants.Views.Error, new ErrorViewModel() { Message = Constants.Errors.TaskNotFound });
            }
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ToDoTask todo)
        {
            if (string.IsNullOrEmpty(todo.Id))
            {
                return View(Constants.Views.Error, new ErrorViewModel() { Message = Constants.Errors.IdCannotBeEmpty });
            }
            var success = await _toDoService.DeleteAsync(todo.Id);
            if (success)
            {
                return RedirectToAction(Constants.Views.Index);
            }
            return View(Constants.Views.Error, new ErrorViewModel() { Message = Constants.Errors.FailedDelete });
        }

        public ViewResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(ToDoTask todo)
        {
            var ret = await _toDoService.CreateAsync(todo);
            if (ret) 
            {
                return RedirectToAction(Constants.Views.Index);
            }
            return View(Constants.Views.Error, new ErrorViewModel() { Message = Constants.Errors.FailedCreate });
        }
    }
}

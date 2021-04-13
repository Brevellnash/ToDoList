using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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

            return View(new ToDoListViewModel() { Title = "To Do List", Tasks = openTasks });
        }

        public async Task<IActionResult> CompletedTasks()
        {
            var closedTasks = await _toDoService.GetAsyncClosedTasks();

            return View("Index", new ToDoListViewModel() { Title = "Completed Tasks", Tasks = closedTasks });
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var item = await _toDoService.GetAsyncById(id);
            if (item == null){
                return NotFound();
            }
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, ToDoTask todo)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var res = await _toDoService.UpdateAsync(todo);
            if (res)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return BadRequest("Could not update task");
            }
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PocketLists.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using PocketLists.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace PocketLists.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly TasksContext _tasksContext;

        public HomeController(ILogger<HomeController> logger,
                              IConfiguration configuration,
                              TasksContext tasksContext)
        {
            _logger = logger;
            _configuration = configuration;
            _tasksContext = tasksContext;
        }
        [Authorize] 
        public async Task<IActionResult> Index()
        {
            Console.WriteLine(User.Identity.Name);

            var tasks = from t in _tasksContext.Tasks
                        where t.TaskOwner.Equals(int.Parse(User.Identity.Name))
                        select t;

            var homeVM = new HomeViewModel
            {
                Tasks = await tasks.ToListAsync(),
            };

            ViewData["username"] = User.Identity.Name;

            return View(homeVM);
        }

        // POST: Tasks/Delete/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _tasksContext.Tasks.FindAsync(id);
            _tasksContext.Tasks.Remove(task);
            await _tasksContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateTask(string taskTitle)
        {
            Models.Task task = new Models.Task { TaskTitle = taskTitle, TaskOwner = int.Parse(User.Identity.Name) };
            _tasksContext.Add(task);
            await _tasksContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> CheckTask(int id)
        {
            var task = await _tasksContext.Tasks.FindAsync(id);
            task.Done = task.Done == 0 ? 1 : 0;
            _tasksContext.Update(task);
            await _tasksContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Controllers
{
    public class TasksController : Controller
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var tasks = await _taskService.GetUserTasksAsync(userId);
            return View(tasks);
        }

        public async Task<IActionResult> Details(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var task = await _taskService.GetTaskByIdAsync(id, userId);

            if (task == null)
                return NotFound();

            return View(task);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateCategoriesDropdown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                await _taskService.CreateTaskAsync(model, userId);
                TempData["Success"] = "Task created successfully!";
                return RedirectToAction(nameof(Index));
            }

            await PopulateCategoriesDropdown();
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var task = await _taskService.GetTaskByIdAsync(id, userId);

            if (task == null)
                return NotFound();

            var model = new TaskViewModel
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Priority = task.Priority,
                CategoryId = task.CategoryId,
                IsCompleted = task.IsCompleted
            };

            await PopulateCategoriesDropdown();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                var success = await _taskService.UpdateTaskAsync(id, model, userId);

                if (success)
                {
                    TempData["Success"] = "Task updated successfully!";
                    return RedirectToAction(nameof(Index));
                }

                return NotFound();
            }

            await PopulateCategoriesDropdown();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _taskService.ToggleTaskStatusAsync(id, userId);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var success = await _taskService.DeleteTaskAsync(id, userId);

            if (success)
                TempData["Success"] = "Task deleted successfully!";

            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateCategoriesDropdown()
        {
            var categories = await _taskService.GetCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
        }
    }
}

using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;

namespace TaskManager.Services
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;

        public TaskService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TaskItem>> GetUserTasksAsync(string userId)
        {
            return await _context.Tasks
                .Include(t => t.Category)
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<TaskItem?> GetTaskByIdAsync(int id, string userId)
        {
            return await _context.Tasks
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        }

        public async Task<TaskItem> CreateTaskAsync(TaskViewModel model, string userId)
        {
            var task = new TaskItem
            {
                Title = model.Title,
                Description = model.Description,
                DueDate = model.DueDate,
                Priority = model.Priority,
                CategoryId = model.CategoryId,
                UserId = userId
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<bool> UpdateTaskAsync(int id, TaskViewModel model, string userId)
        {
            var task = await GetTaskByIdAsync(id, userId);
            if (task == null) return false;

            task.Title = model.Title;
            task.Description = model.Description;
            task.DueDate = model.DueDate;
            task.Priority = model.Priority;
            task.CategoryId = model.CategoryId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTaskAsync(int id, string userId)
        {
            var task = await GetTaskByIdAsync(id, userId);
            if (task == null) return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ToggleTaskStatusAsync(int id, string userId)
        {
            var task = await GetTaskByIdAsync(id, userId);
            if (task == null) return false;

            task.IsCompleted = !task.IsCompleted;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<DashboardViewModel> GetDashboardDataAsync(string userId)
        {
            var tasks = await GetUserTasksAsync(userId);
            var now = DateTime.UtcNow;

            return new DashboardViewModel
            {
                TotalTasks = tasks.Count,
                CompletedTasks = tasks.Count(t => t.IsCompleted),
                PendingTasks = tasks.Count(t => !t.IsCompleted),
                OverdueTasks = tasks.Count(t => !t.IsCompleted && t.DueDate < now),
                RecentTasks = tasks.Take(5).ToList(),
                UpcomingTasks = tasks
                    .Where(t => !t.IsCompleted && t.DueDate >= now)
                    .OrderBy(t => t.DueDate)
                    .Take(5)
                    .ToList()
            };
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }
    }
}

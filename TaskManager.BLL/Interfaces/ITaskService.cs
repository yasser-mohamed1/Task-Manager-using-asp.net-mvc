using TaskManager.Models;

namespace TaskManager.BLL.Interfaces
{
    public interface ITaskService
    {
        Task<List<TaskItem>> GetUserTasksAsync(string userId);
        Task<TaskItem?> GetTaskByIdAsync(int id, string userId);
        Task<TaskItem> CreateTaskAsync(TaskViewModel model, string userId);
        Task<bool> UpdateTaskAsync(int id, TaskViewModel model, string userId);
        Task<bool> DeleteTaskAsync(int id, string userId);
        Task<bool> ToggleTaskStatusAsync(int id, string userId);
        Task<DashboardViewModel> GetDashboardDataAsync(string userId);
        Task<List<Category>> GetCategoriesAsync();
    }
}

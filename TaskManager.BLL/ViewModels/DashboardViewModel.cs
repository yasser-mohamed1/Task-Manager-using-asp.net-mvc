namespace TaskManager.Models
{
    public class DashboardViewModel
    {
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int PendingTasks { get; set; }
        public int OverdueTasks { get; set; }
        public List<TaskItem> RecentTasks { get; set; } = new();
        public List<TaskItem> UpcomingTasks { get; set; } = new();
    }
}

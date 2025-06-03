using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class TaskViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [Display(Name = "Task Title")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Due Date")]
        [DataType(DataType.DateTime)]
        public DateTime? DueDate { get; set; }

        [Display(Name = "Priority")]
        public TaskPriority Priority { get; set; }

        [Display(Name = "Category")]
        public int? CategoryId { get; set; }

        public bool IsCompleted { get; set; }
    }
}

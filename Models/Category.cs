using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [StringLength(7)]
        public string Color { get; set; } = "#007bff";

        public virtual ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
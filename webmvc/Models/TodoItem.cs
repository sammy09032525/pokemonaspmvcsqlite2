using System.ComponentModel.DataAnnotations;

namespace webmvc.Models
{
    public class TodoItem
    {
        public int ID { get; set; }
        [Required]
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
    }
}
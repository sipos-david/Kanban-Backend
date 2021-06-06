using System.Collections.Generic;

namespace KanbanBoard.Models
{
    public class Task
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Number { get; set; }
        public int? ColumnId { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}

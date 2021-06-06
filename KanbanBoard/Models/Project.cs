using System.Collections.Generic;

namespace KanbanBoard.Models
{
    public class Project
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public ICollection<Table> Tables { get; set; } = new List<Table>();
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}

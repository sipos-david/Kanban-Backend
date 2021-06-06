using System.Collections.Generic;

namespace KanbanBoard.Models
{
    public class Column
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public int? TableId { get; set; }
        public int Number { get; set; }
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
    }
}

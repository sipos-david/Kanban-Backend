using System;

namespace KanbanBoard.Models
{
    public class Comment
    {
        public int? Id { get; set; }
        public string? Text { get; set; }
        public int? TaskId { get; set; }
        public DateTime Date { get; set; }
        public DateTime? EditedDate { get; set; }
        public int Number { get; set; }
        public User Author { get; set; } = default!;
    }
}

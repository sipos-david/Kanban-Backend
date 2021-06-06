using System;

namespace KanbanBoard.DAL.EfDbContext.DTO
{
    public class DbComment
    {
        public int? Id { get; set; }
        public string? Text { get; set; }
        public DbTask Task { get; set; } = default!;
        public DateTime Date { get; set; }
        public DateTime? EditedDate { get; set; }
        public DbUser Author { get; set; } = default!;
    }
}

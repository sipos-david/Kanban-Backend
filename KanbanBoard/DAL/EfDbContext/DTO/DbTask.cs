using System.Collections.Generic;

namespace KanbanBoard.DAL.EfDbContext.DTO
{
    public class DbTask
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Number { get; set; }
        public DbColumn Column { get; set; } = default!;
        public ICollection<DbComment> Comments { get; set; } = new List<DbComment>();
        public ICollection<DbUser> Users { get; set; } = new List<DbUser>();
    }
}

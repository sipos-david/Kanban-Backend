using System.Collections.Generic;

namespace KanbanBoard.DAL.EfDbContext.DTO
{
    public class DbColumn
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public int Number { get; set; }
        public DbTable Table { get; set; } = default!;
        public ICollection<DbTask> Tasks { get; set; } = new List<DbTask>();
    }
}

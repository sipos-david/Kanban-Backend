using System.Collections.Generic;

namespace KanbanBoard.DAL.EfDbContext.DTO
{
    public class DbTable
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public DbProject Project { get; set; } = default!;
        public ICollection<DbColumn> Columns { get; set; } = new List<DbColumn>();
    }
}

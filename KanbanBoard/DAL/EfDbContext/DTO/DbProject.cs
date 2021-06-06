using System.Collections.Generic;

namespace KanbanBoard.DAL.EfDbContext.DTO
{
    public class DbProject
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public ICollection<DbTable> Tables { get; set; } = new List<DbTable>();
        public ICollection<DbUser> Users { get; set; } = new List<DbUser>();
    }
}

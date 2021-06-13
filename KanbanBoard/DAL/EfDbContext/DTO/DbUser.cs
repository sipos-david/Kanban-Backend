using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace KanbanBoard.DAL.EfDbContext.DTO
{
    public class DbUser
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public ICollection<DbTask> Tasks { get; set; } = default!;
        public ICollection<DbComment> Comments { get; set; } = default!;
        public ICollection<DbProject> Projects { get; set; } = default!;
    }
}

using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace KanbanBoard.DAL.EfDbContext.DTO
{
    public class DbUser : IdentityUser
    {
        public ICollection<DbTask> Tasks { get; set; } = default!;
        public ICollection<DbComment> Comments { get; set; } = default!;
        public ICollection<DbProject> Projects { get; set; } = default!;
    }
}

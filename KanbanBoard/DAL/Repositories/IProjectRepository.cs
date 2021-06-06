using KanbanBoard.DAL.EfDbContext.DTO;
using KanbanBoard.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KanbanBoard.DAL.Repositories
{
    public interface IProjectRepository
    {
        Task<ICollection<Project>> GetAllProjectsAsync();
        Task<Project?> GetByIdAsync(int id);
        Task<DbProject?> GetDtoByIdAsync(int id);
        Task<Project> AddAsync(Project project);
        Task<Project?> RemoveAsync(int id);
        Task<Project> UpdateAsync(Project project);
    }
}

using KanbanBoard.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KanbanBoard.Services
{
    public interface IProjectService
    {
        Task<ICollection<Project>> GetAllAsync();
        Task<Project?> GetByIdAsync(int id);
        Task<Project> AddAsync(Project project);
        Task<Project?> RemoveAsync(int id);
        Task<Project> UpdateAsync(Project project);
        Task<Project?> PatchByIdAsync(int id, dynamic project);
        Task<Project?> AddTableAsync(int id, Table table);
        Task<Project?> RemoveTableAsync(int id, int table);
        Task<Project?> AddUsersByIdAsync(int id, List<string> users);
        Task<Project?> RemoveUserByIdAsync(int id, string user);
        Task<ICollection<Project>> GetAllByNameAsync(string name);
    }
}

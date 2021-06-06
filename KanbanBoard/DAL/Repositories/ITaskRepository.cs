using KanbanBoard.DAL.EfDbContext.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KanbanBoard.DAL.Repositories
{
    public interface ITaskRepository
    {
        Task<Models.Task?> GetByIdAsync(int id);
        Task<Models.Task> AddAsync(Models.Task task);
        Task<Models.Task?> RemoveAsync(int id);
        Task<ICollection<Models.Task>> RemoveListAsync(ICollection<Models.Task> list);
        Task<Models.Task> UpdateAsync(Models.Task task);
        Task<ICollection<Models.Task>> UpdateListAsync(ICollection<Models.Task> list);
        DbTask? GetTaskDtoById(int id);
    }
}

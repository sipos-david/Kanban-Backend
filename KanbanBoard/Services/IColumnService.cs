using KanbanBoard.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KanbanBoard.Services
{
    public interface IColumnService
    {
        Task<Column> AddAsync(Column column);
        Task<Column?> GetByIdAsync(int id);
        Task<Column?> RemoveAsync(int id);
        Task<ICollection<Column>> RemoveListAsync(ICollection<Column> list);
        Task<Column> UpdateAsync(Column column);
        Task<Column?> PatchByIdAsync(int id, dynamic column);
        Task<Models.Task?> AddTaskByIdAsync(int id, Models.Task task);
        Task<Models.Task?> RemoveTaskByIdAsync(int id, int task);
        Task<Models.Task?> MoveTaskByIdAsync(int id, Models.Task task);
    }
}

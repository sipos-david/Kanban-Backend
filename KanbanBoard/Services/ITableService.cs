using KanbanBoard.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KanbanBoard.Services
{
    public interface ITableService
    {
        Task<Table?> GetByIdAsync(int id);
        Task<Table> AddAsync(Table table);
        Task<Table?> RemoveAsync(int id);
        Task<ICollection<Table>> RemoveListAsync(ICollection<Table> list);
        Task<Table> UpdateAsync(Table table);
        Task<Table?> PatchByIdAsync(int id, dynamic table);
        Task<Table?> AddColumnAsync(int id, Column column);
        Task<Table?> MoveColumnByIdAsync(int id, Column column);
        Task<Table?> RemoveColumnAsync(int id, int column);
    }
}

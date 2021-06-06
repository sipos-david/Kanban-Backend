using KanbanBoard.DAL.EfDbContext.DTO;
using KanbanBoard.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KanbanBoard.DAL.Repositories
{
    public interface IColumnRepository
    {
        Task<Column?> GetByIdAsync(int id);
        Task<Column> AddAsync(Column column);
        Task<Column?> RemoveAsync(int id);
        Task<ICollection<Column>> RemoveListAsync(ICollection<Column> list);
        Task<Column> UpdateAsync(Column column);
        DbColumn? GetColumnDtoById(int id);
    }
}

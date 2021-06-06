using KanbanBoard.DAL.EfDbContext.DTO;
using KanbanBoard.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KanbanBoard.DAL.Repositories
{
    public interface ITableRepository
    {
        Task<Table?> GetByIdAsync(int id);
        Task<Table> AddAsync(Table table);
        Task<Table?> RemoveAsync(int id);
        Task<ICollection<Table>> RemoveListAsync(ICollection<Table> list);
        Task<Table> UpdateAsync(Table table);
        DbTable? GetDtoById(int id);
    }
}

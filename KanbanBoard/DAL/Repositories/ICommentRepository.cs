using KanbanBoard.DAL.EfDbContext.DTO;
using KanbanBoard.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KanbanBoard.DAL.Repositories
{
    public interface ICommentRepository
    {
        Task<Comment?> GetByIdAsync(int id);
        Task<Comment> AddAsync(Comment comment);
        Task<Comment?> RemoveAsync(int id);
        Task<ICollection<Comment>> RemoveListAsync(ICollection<Comment> list);
        Task<Comment> UpdateAsync(Comment comment);
        DbComment? GetCommentDtoById(int id);
    }
}

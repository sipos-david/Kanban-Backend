using KanbanBoard.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KanbanBoard.Services
{
    public interface ICommentService
    {
        Task<Comment?> GetByIdAsync(int id);
        Task<Comment?> RemoveAsync(int id);
        Task<ICollection<Comment>> RemoveListAsync(ICollection<Comment> list);
        Task<Comment> AddAsync(Comment comment);
        Task<Comment> UpdateAsync(Comment comment);
        Task<Comment?> PatchByIdAsync(int id, dynamic comment);
    }
}

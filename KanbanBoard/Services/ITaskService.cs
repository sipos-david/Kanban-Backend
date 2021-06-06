using KanbanBoard.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KanbanBoard.Services
{
    public interface ITaskService
    {
        Task<Models.Task> AddAsync(Models.Task task);
        Task<Models.Task?> GetByIdAsync(int id);
        Task<Models.Task?> RemoveAsync(int id);
        Task<ICollection<Models.Task>> RemoveListAsync(ICollection<Models.Task> list);
        Task<Models.Task> UpdateAsync(Models.Task task);
        Task<ICollection<Models.Task>> UpdateListAsync(ICollection<Models.Task> updatedTasks);
        Task<Models.Task?> PatchByIdAsync(int id, dynamic task);
        Task<Models.Task?> AddUsersByIdAsync(int id, List<string> users);
        Task<Models.Task?> RemoveUserByIdAsync(int id, string user);
        Task<Comment?> AddCommentAsync(int id, Comment comment);
        Task<Comment?> RemoveCommentByIdAsync(int id, int comment);
    }
}

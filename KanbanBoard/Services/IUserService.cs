using KanbanBoard.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KanbanBoard.Services
{
    public interface IUserService
    {
        Task<ICollection<User>> GetAllAsync();
        Task<User?> GetByIdAsync(string id);
        Task<bool> IsUserRegistered(string? id);
        Task<User?> RegisterUser(User user);
    }
}

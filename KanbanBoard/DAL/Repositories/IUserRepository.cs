using KanbanBoard.DAL.EfDbContext.DTO;
using KanbanBoard.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KanbanBoard.DAL.Repositories
{
    public interface IUserRepository
    {
        Task<ICollection<User>> GetAllUsersAsync();
        Task<User?> GetByIdAsync(string id);
        DbUser? GetUserDtoById(string id);
        Task<User> AddAsync(User user);
    }
}

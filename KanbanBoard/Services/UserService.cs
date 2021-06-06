using KanbanBoard.DAL.Repositories;
using KanbanBoard.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KanbanBoard.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository _userRepository)
        {
            userRepository = _userRepository;
        }

        public async Task<ICollection<User>> GetAllAsync()
        {
            return await userRepository.GetAllUsersAsync();
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            return await userRepository.GetByIdAsync(id);
        }
    }
}

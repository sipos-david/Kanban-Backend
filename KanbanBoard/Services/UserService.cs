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
        
        public async Task<bool> IsUserRegistered(string? id)
        {
            if (id == null)
            {
                return false;
            }
            var user = await userRepository.GetByIdAsync(id);
            return user != null ;
        }

        public async Task<User?> RegisterUser(User user)
        {
            return await userRepository.AddAsync(user);
        }
    }
}

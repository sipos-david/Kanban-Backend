using AutoMapper;
using KanbanBoard.DAL.EfDbContext;
using KanbanBoard.DAL.EfDbContext.DTO;
using KanbanBoard.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KanbanBoard.DAL.Repositories
{
    class UserRepository : IUserRepository
    {
        private readonly KanbanBoardDbContext context;
        private readonly IMapper mapper;

        public UserRepository(KanbanBoardDbContext _context, IMapper _mapper)
        {
            context = _context;
            mapper = _mapper;
        }

        public async Task<ICollection<User>> GetAllUsersAsync()
        {
            return mapper.Map<List<DbUser>, List<User>>(await context.Users.ToListAsync());
        }

        public DbUser? GetUserDtoById(string id)
        {
            return context.Users.Find(id);
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            return mapper.Map<User>(await context.Users.FindAsync(id));
        }
    }
}

using AutoMapper;
using KanbanBoard.DAL.EfDbContext.DTO;
using KanbanBoard.DAL.Repositories;
using KanbanBoard.Models;

namespace KanbanBoard.DAL.EfDbContext.Converters
{
    public class UserConverter : ITypeConverter<User, DbUser>
    {
        private readonly IUserRepository userRepository;

        public UserConverter(IUserRepository _userRepository)
        {
            userRepository = _userRepository;
        }

        public DbUser Convert(User source, DbUser destination, AutoMapper.ResolutionContext context)
        {
            if (source.Id != null)
            {
                var user = userRepository.GetUserDtoById(source.Id);
                if (user != null)
                {
                    return user;
                }
            }
            return new DbUser();
        }
    }
}

using AutoMapper;
using KanbanBoard.DAL.EfDbContext.DTO;
using KanbanBoard.DAL.Repositories;
using KanbanBoard.Models;
using System.Collections.Generic;

namespace KanbanBoard.DAL.EfDbContext.Converters
{
    public class ProjectConverter : ITypeConverter<Project, DbProject>
    {
        private readonly IProjectRepository projectRepository;
        private readonly IMapper mapper;

        public ProjectConverter(IProjectRepository _projectRepository, IMapper _mapper)
        {
            projectRepository = _projectRepository;
            mapper = _mapper;
        }

        public DbProject Convert(Project source, DbProject destination, ResolutionContext context)
        {
            DbProject project = new();
            if (source.Id != null)
            {
                var _project = projectRepository.GetDtoByIdAsync((int)source.Id).Result;
                if (_project != null)
                {
                    project = _project;
                }
            }
            project.Name = source.Name;
            project.Tables = mapper.Map<ICollection<DbTable>>(source.Tables);
            project.Users = mapper.Map<ICollection<DbUser>>(source.Users);
            return project;
        }
    }
}

using AutoMapper;
using KanbanBoard.DAL.EfDbContext.DTO;
using KanbanBoard.DAL.Repositories;
using KanbanBoard.Models;
using System.Collections.Generic;

namespace KanbanBoard.DAL.EfDbContext.Converters
{
    public class TableConverter : ITypeConverter<Table, DbTable>
    {
        private readonly ITableRepository tableRepository;
        private readonly IProjectRepository projectRepository;
        private readonly IMapper mapper;

        public TableConverter(ITableRepository _tableRepository, IProjectRepository _projectRepository, IMapper _mapper)
        {
            tableRepository = _tableRepository;
            projectRepository = _projectRepository;
            mapper = _mapper;
        }

        public DbTable Convert(Table source, DbTable destination, ResolutionContext context)
        {
            DbTable table = new();
            if (source.Id != null)
            {
                var _table = tableRepository.GetDtoById((int)source.Id);
                if (_table != null)
                {
                     table = _table;
                }
            }
            table.Name = source.Name;
            if (source.ProjectId != null)
            {
                var project = projectRepository.GetDtoByIdAsync((int)source.ProjectId).Result;
                if (project != null)
                {
                    table.Project = project;
                }
            }
            table.Columns = mapper.Map<ICollection<DbColumn>>(source.Columns);
            return table;
        }
    }
}

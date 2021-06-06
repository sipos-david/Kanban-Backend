using AutoMapper;
using KanbanBoard.DAL.EfDbContext.DTO;
using KanbanBoard.DAL.Repositories;
using KanbanBoard.Models;
using System.Collections.Generic;

namespace KanbanBoard.DAL.EfDbContext.Converters
{
    public class ColumnConverter : ITypeConverter<Column, DbColumn>
    {
        private readonly IColumnRepository columnRepository;
        private readonly ITableRepository tableRepository;
        private readonly IMapper mapper;

        public ColumnConverter(IColumnRepository _columnRepository, ITableRepository _tableRepository, IMapper _mapper)
        {
            columnRepository = _columnRepository;
            tableRepository = _tableRepository;
            mapper = _mapper;
        }

        public DbColumn Convert(Column source, DbColumn destination, ResolutionContext context)
        {
            DbColumn column = new();
            if (source.Id != null)
            {
                var _column = columnRepository.GetColumnDtoById((int)source.Id);
                if (_column != null)
                {
                    column = _column;
                }
            }
            column.Name = source.Name;
            if (source.TableId != null)
            {
                var table = tableRepository.GetDtoById((int)source.TableId);
                if (table != null)
                {
                    column.Table = table;
                }
            }
            column.Tasks = mapper.Map<ICollection<DbTask>>(source.Tasks);
            return column;
        }
    }
}

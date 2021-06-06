using AutoMapper;
using KanbanBoard.DAL.EfDbContext.DTO;
using KanbanBoard.DAL.Repositories;
using KanbanBoard.Models;
using System.Collections.Generic;

namespace KanbanBoard.DAL.EfDbContext.Converters
{
    public class TaskConverter : ITypeConverter<Task, DbTask>
    {
        private readonly ITaskRepository taskRepository;
        private readonly IColumnRepository columnRepository;
        private readonly IMapper mapper;

        public TaskConverter(ITaskRepository _taskRepository, IColumnRepository _columnRepository, IMapper _mapper)
        {
            taskRepository = _taskRepository;
            columnRepository = _columnRepository;
            mapper = _mapper;
        }

        public DbTask Convert(Task source, DbTask destination, ResolutionContext context)
        {
            DbTask task = new();
            if (source.Id != null)
            {
                var _task = taskRepository.GetTaskDtoById((int)source.Id);
                if (_task != null)
                {
                    task = _task;
                }
            }
            task.Name = source.Name;
            task.Description = source.Description;
            task.Number = source.Number;
            if (source.ColumnId != null) {
                var column = columnRepository.GetColumnDtoById((int)source.ColumnId);
                if (column != null)
                {
                    task.Column = column;
                }
            }
            task.Users = mapper.Map<ICollection<DbUser>>(source.Users);
            task.Comments = mapper.Map<ICollection<DbComment>>(source.Comments);
            return task;
        }
    }
}

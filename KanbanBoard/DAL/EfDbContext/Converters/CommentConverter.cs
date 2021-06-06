using AutoMapper;
using KanbanBoard.DAL.EfDbContext.DTO;
using KanbanBoard.DAL.Repositories;
using KanbanBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KanbanBoard.DAL.EfDbContext.Converters
{
    public class CommentConverter : ITypeConverter<Comment, DbComment>
    {
        private readonly ICommentRepository commentRepository;
        private readonly IMapper mapper;
        private readonly ITaskRepository taskRepository;

        public CommentConverter(ICommentRepository _commentRepository, ITaskRepository _taskRepository, IMapper _mapper)
        {
            taskRepository = _taskRepository;
            commentRepository = _commentRepository;
            mapper = _mapper;
        }

        public DbComment Convert(Comment source, DbComment destination, ResolutionContext context)
        {
            DbComment comment = new();
            if (source.Id != null)
            {
                var _comment = commentRepository.GetCommentDtoById((int)source.Id);
                if (_comment != null)
                {
                    comment = _comment;
                }
            }
            comment.Date = source.Date;
            comment.EditedDate = source.EditedDate;
            comment.Text = source.Text;
            comment.Author = mapper.Map<DbUser>(source.Author);
            if (source.TaskId != null)
            {
                var task = taskRepository.GetTaskDtoById((int)source.TaskId);
                if (task != null)
                {
                    comment.Task = task;
                }
            }
            return comment;
        }
    }
}

using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using KanbanBoard.DAL.EfDbContext;
using AutoMapper;
using KanbanBoard.DAL.EfDbContext.DTO;

namespace KanbanBoard.DAL.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly KanbanBoardDbContext context;
        private readonly IMapper mapper;

        public TaskRepository(KanbanBoardDbContext _context, IMapper _mapper)
        {
            context = _context;
            mapper = _mapper;
        }

        public async Task<Models.Task> AddAsync(Models.Task task)
        {
            var _task = await context.Tasks.AddAsync(mapper.Map<DbTask>(task));
            await context.SaveChangesAsync();
            return mapper.Map<Models.Task>(_task.Entity);
        }

        public async Task<Models.Task?> GetByIdAsync(int id)
        {
            return mapper.Map<Models.Task>(
                await context.Tasks
                    .Include(t => t.Users)
                    .Include(t => t.Comments)
                        .ThenInclude(c => c.Author)
                    .FirstAsync(task => task.Id == id));
        }

        public DbTask? GetTaskDtoById(int id)
        {
            return context.Tasks.Find(id);
        }

        public async Task<Models.Task?> RemoveAsync(int id)
        {
            var task = await GetByIdAsync(id);
            if (task != null)
            {
                context.Tasks.Remove(mapper.Map<DbTask>(task));
                await context.SaveChangesAsync();
            }
            return task;
        }

        public async Task<ICollection<Models.Task>> RemoveListAsync(ICollection<Models.Task> list)
        {
            ICollection<Models.Task> removedTasks = new List<Models.Task>();
            foreach (Models.Task t in list)
            {
                if (t.Id != null)
                {
                    var task = await GetByIdAsync((int)t.Id);
                    if (task != null)
                    {
                        var deletedTask = context.Tasks.Remove(mapper.Map<DbTask>(task));
                        if (deletedTask != null)
                        {
                            removedTasks.Add(mapper.Map<Models.Task>(deletedTask));
                        }
                    }
                }
            }
            await context.SaveChangesAsync();
            return removedTasks;
        }

        public async Task<Models.Task> UpdateAsync(Models.Task task)
        {
            var _task = context.Tasks.Update(mapper.Map<DbTask>(task));
            await context.SaveChangesAsync();
            return mapper.Map<Models.Task>(_task.Entity);
        }

        public async Task<ICollection<Models.Task>> UpdateListAsync(ICollection<Models.Task> list)
        {
            context.UpdateRange(mapper.Map<ICollection<DbTask>>(list));
            await context.SaveChangesAsync();
            return list;
        }
    }
}

using AutoMapper;
using KanbanBoard.DAL.EfDbContext;
using KanbanBoard.DAL.EfDbContext.DTO;
using KanbanBoard.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KanbanBoard.DAL.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly KanbanBoardDbContext context;
        private readonly IMapper mapper;

        public ProjectRepository(KanbanBoardDbContext _context, IMapper _mapper)
        {
            context = _context;
            mapper = _mapper;
        }

        public async Task<Project> AddAsync(Project project)
        {
            var _project = await context.Projects.AddAsync(mapper.Map<DbProject>(project));
            await context.SaveChangesAsync();
            return mapper.Map<Project>(_project.Entity);
        }

        public async Task<ICollection<Project>> GetAllProjectsAsync()
        {
            return mapper.Map<List<Project>>(await context.Projects.Include(p => p.Tables).Include(pr => pr.Users).ToListAsync());
        }

        public async Task<Project?> GetByIdAsync(int id)
        {
            var project = await context.Projects
                .Include(p => p.Tables)
                .Include(pr => pr.Users).FirstOrDefaultAsync(project => project.Id == id);
            if (project != null)
            {
                return mapper.Map<Project>(project);
            }
            else
            {
                return null;
            }
        }

        public async Task<DbProject?> GetDtoByIdAsync(int id)
        {
            return await context.Projects.Include(p => p.Tables).Include(pr => pr.Users).FirstAsync(project => project.Id == id);
        }

        public async Task<Project?> RemoveAsync(int id)
        {
            var project = await GetByIdAsync(id);
            if (project != null)
            {
                context.Projects.Remove(mapper.Map<DbProject>(project));
                await context.SaveChangesAsync();
            }
            return project;
        }

        public async Task<Project> UpdateAsync(Project project)
        {
            var _project = context.Projects.Update(mapper.Map<DbProject>(project));
            await context.SaveChangesAsync();
            return mapper.Map<Project>(_project.Entity);
        }
    }
}

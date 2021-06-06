using AutoMapper;
using KanbanBoard.DAL.EfDbContext;
using KanbanBoard.DAL.EfDbContext.DTO;
using KanbanBoard.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KanbanBoard.DAL.Repositories
{
    public class TableRepository : ITableRepository
    {
        private readonly KanbanBoardDbContext context;

        private readonly IMapper mapper;

        public TableRepository(KanbanBoardDbContext _context, IMapper _mapper)
        {
            context = _context;
            mapper = _mapper;
        }

        public async Task<Table> AddAsync(Table table)
        {
            var _table = await context.Tables.AddAsync(mapper.Map<DbTable>(table));
            await context.SaveChangesAsync();
            return mapper.Map<Table>(_table.Entity);
        }

        public async Task<Table?> GetByIdAsync(int id)
        {
            var table = await context.Tables
                .Include(t => t.Project)
                .Include(t => t.Columns)
                    .ThenInclude(c => c.Tasks)
                         .ThenInclude(t => t.Users)
                .Include(t => t.Columns)
                    .ThenInclude(c => c.Tasks)
                         .ThenInclude(t => t.Comments)
                            .ThenInclude(c => c.Author)
                .AsSplitQuery()
                .FirstOrDefaultAsync(table => table.Id == id);
            if (table != null)
            {
                return mapper.Map<Table>(table);
            }
            else
            {
                return null;
            }
        }

        public DbTable? GetDtoById(int id)
        {
            return context.Tables.Find(id);
        }

        public async Task<Table?> RemoveAsync(int id)
        {
            var table = await GetByIdAsync(id);
            if (table != null)
            {
                context.Tables.Remove(mapper.Map<DbTable>(table));
                await context.SaveChangesAsync();
            }
            return table;
        }

        public async Task<ICollection<Table>> RemoveListAsync(ICollection<Table> list)
        {
            foreach (Table t in list)
            {
                if (t.Id != null)
                {
                    var table = await GetByIdAsync((int)t.Id);
                    if (table != null)
                    {
                        context.Tables.Remove(mapper.Map<DbTable>(table));
                    }
                }
            }
            await context.SaveChangesAsync();
            return list;
        }

        public async Task<Table> UpdateAsync(Table table)
        {
            var _table = context.Tables.Update(mapper.Map<DbTable>(table));
            await context.SaveChangesAsync();
            return mapper.Map<Table>(_table.Entity);
        }
    }
}

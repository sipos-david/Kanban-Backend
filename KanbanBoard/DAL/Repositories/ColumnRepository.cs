using AutoMapper;
using KanbanBoard.DAL.EfDbContext;
using KanbanBoard.DAL.EfDbContext.DTO;
using KanbanBoard.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KanbanBoard.DAL.Repositories
{
    public class ColumnRepository : IColumnRepository
    {
        private readonly KanbanBoardDbContext context;

        private readonly IMapper mapper;

        public ColumnRepository(KanbanBoardDbContext _context, IMapper _mapper)
        {
            context = _context;
            mapper = _mapper;
        }

        public async Task<Column> AddAsync(Column column)
        {
            var _column = await context.Columns.AddAsync(mapper.Map<DbColumn>(column));
            await context.SaveChangesAsync();
            return mapper.Map<Column>(_column.Entity);
        }

        public async Task<Column?> GetByIdAsync(int id)
        {
            return mapper.Map<Column>(await context.Columns.Include(c => c.Tasks).FirstAsync(co => co.Id == id));
        }

        public DbColumn? GetColumnDtoById(int id)
        {
            return context.Columns.Find(id);
        }

        public async Task<Column?> RemoveAsync(int id)
        {
            var column = await GetByIdAsync(id);
            if (column != null)
            {
                context.Columns.Remove(mapper.Map<DbColumn>(column));
                await context.SaveChangesAsync();
            }
            return column;
        }

        public async Task<ICollection<Column>> RemoveListAsync(ICollection<Column> list)
        {
            foreach (Column c in list)
            {
                if (c.Id != null)
                {
                    var column = await GetByIdAsync((int)c.Id);
                    if (column != null)
                    {
                        context.Columns.Remove(mapper.Map<DbColumn>(column));
                    }
                }
            }
            await context.SaveChangesAsync();
            return list;
        }

        public async Task<Column> UpdateAsync(Column column)
        {
            var _column = context.Columns.Update(mapper.Map<DbColumn>(column));
            await context.SaveChangesAsync();
            return mapper.Map<Column>(_column.Entity);
        }
    }
}

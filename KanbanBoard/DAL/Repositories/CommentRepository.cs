using AutoMapper;
using KanbanBoard.DAL.EfDbContext;
using KanbanBoard.DAL.EfDbContext.DTO;
using KanbanBoard.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KanbanBoard.DAL.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly KanbanBoardDbContext context;
        private readonly IMapper mapper;

        public CommentRepository(KanbanBoardDbContext _context, IMapper _mapper)
        {
            context = _context;
            mapper = _mapper;
        }

        public async Task<Comment> AddAsync(Comment comment)
        {
            var _comment = await context.Comments.AddAsync(mapper.Map<DbComment>(comment));
            await context.SaveChangesAsync();
            return mapper.Map<Comment>(_comment.Entity);
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return mapper.Map<Comment>(await context.Comments.Include(c => c.Author).FirstAsync(co => co.Id == id));
        }

        public DbComment? GetCommentDtoById(int id)
        {
            return context.Comments.Find(id);
        }

        public async Task<Comment?> RemoveAsync(int id)
        {
            var comment = await GetByIdAsync(id);
            if (comment != null)
            {
                context.Comments.Remove(mapper.Map<DbComment>(comment));
                await context.SaveChangesAsync();
            }
            return comment;
        }

        public async Task<ICollection<Comment>> RemoveListAsync(ICollection<Comment> list)
        {
            foreach (Comment c in list)
            {
                if (c.Id != null)
                {
                    var comment = await GetByIdAsync((int)c.Id);
                    if (comment != null)
                    {
                        context.Comments.Remove(mapper.Map<DbComment>(comment));
                    }
                }
            }
            await context.SaveChangesAsync();
            return list;
        }

        public async Task<Comment> UpdateAsync(Comment comment)
        {
            var _comment = context.Comments.Update(mapper.Map<DbComment>(comment));
            await context.SaveChangesAsync();
            return mapper.Map<Comment>(_comment.Entity);
        }
    }
}

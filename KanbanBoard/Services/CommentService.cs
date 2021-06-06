using KanbanBoard.DAL.Repositories;
using KanbanBoard.Lib;
using KanbanBoard.Models;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KanbanBoard.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository commentRepository;

        public CommentService(ICommentRepository _commentRepository)
        {
            commentRepository = _commentRepository;
        }

        public async Task<Comment> AddAsync(Comment comment)
        {
            comment.Date = DateTime.Now;
            return await commentRepository.AddAsync(comment);
        }


        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await commentRepository.GetByIdAsync(id);
        }

        public async Task<Comment?> PatchByIdAsync(int id, dynamic comment)
        {
            var _comment = await commentRepository.GetByIdAsync(id);
            if (_comment != null)
            {
                bool updated = false;
                bool? isTextExists = DynamicLib.IsPropertyExists(comment, "text");
                if (isTextExists == null || isTextExists == true)
                {
                    _comment.Text = comment.text;
                    _comment.EditedDate = DateTime.Now;
                    updated = true;
                }
                if (updated)
                {
                    return await commentRepository.UpdateAsync(_comment);
                }
            }
            return null;
        }

        public async Task<Comment?> RemoveAsync(int id)
        {
            return await commentRepository.RemoveAsync(id);
        }

        public async Task<ICollection<Comment>> RemoveListAsync(ICollection<Comment> list)
        {
            return await commentRepository.RemoveListAsync(list);
        }

        public async Task<Comment> UpdateAsync(Comment comment)
        {
            comment.EditedDate = DateTime.Now;
            return await commentRepository.UpdateAsync(comment);
        }
    }
}

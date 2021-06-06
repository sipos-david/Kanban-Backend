using KanbanBoard.DAL.Repositories;
using KanbanBoard.Lib;
using KanbanBoard.Models;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KanbanBoard.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository taskRepository;
        private readonly IUserRepository userRepository;
        private readonly ICommentService commentService;

        public TaskService(ITaskRepository _taskRepository, IUserRepository _userRepository, ICommentService _commentService)
        {
            taskRepository = _taskRepository;
            userRepository = _userRepository;
            commentService = _commentService;
        }

        public async Task<Comment?> AddCommentAsync(int id, Comment comment)
        {
            var task = await taskRepository.GetByIdAsync(id);
            if (task != null)
            {
                if (comment.Id == null)
                {
                    comment.TaskId = id;
                    comment = await commentService.AddAsync(comment);
                }
                return comment;
            }
            return null;
        }

        public async Task<Models.Task> AddAsync(Models.Task task)
        {
            if (task.Id == null)
            {
                return await taskRepository.AddAsync(task);
            }
            else
            {
                var _task = await taskRepository.GetByIdAsync((int)task.Id);
                if (_task == null)
                {
                    task.Id = null;
                    _task = await taskRepository.AddAsync(task);
                }
                return _task;
            }
        }

        public async Task<Models.Task?> AddUsersByIdAsync(int id, List<string> users)
        {
            var task = await taskRepository.GetByIdAsync(id);
            if (task != null)
            {
                for (int i = 0; i < users.Count; i++)
                {
                    var user = await userRepository.GetByIdAsync(users[i]);
                    if (user != null)
                    {
                        Boolean found = false;
                        foreach (User u in task.Users)
                        {
                            if (u.Id == user.Id)
                            {
                                found = true;
                            }
                        }
                        if (!found) { task.Users.Add(user); }
                    }
                }
                return await taskRepository.UpdateAsync(task);
            }
            return null;
        }

        public async Task<Models.Task?> GetByIdAsync(int id)
        {
            return await taskRepository.GetByIdAsync(id);
        }

        public async Task<Models.Task?> PatchByIdAsync(int id, dynamic task)
        {
            var _task = await taskRepository.GetByIdAsync(id);
            if (_task != null)
            {
                bool updated = false;
                bool? isNameExists = DynamicLib.IsPropertyExists(task, "name");
                if (isNameExists == null || isNameExists == true)
                {
                    _task.Name = task.name;
                    updated = true;
                }
                bool? isDescription = DynamicLib.IsPropertyExists(task, "description");
                if (isDescription == null || isDescription == true)
                {
                    _task.Description = task.description;
                    updated = true;
                }
                if (updated)
                {
                    return await taskRepository.UpdateAsync(_task);
                }
            }
            return null;
        }



        public async Task<Comment?> RemoveCommentByIdAsync(int id, int comment)
        {
            var task = await taskRepository.GetByIdAsync(id);
            if (task != null)
            {
                var _comment = await commentService.GetByIdAsync(comment);
                if (_comment != null)
                {
                    foreach (Comment c in task.Comments)
                    {
                        if (c.Id == _comment.Id)
                        {
                            return await commentService.RemoveAsync(comment);
                        }
                    }
                }
            }
            return null;
        }

        public async Task<Models.Task?> RemoveAsync(int id)
        {
            return await taskRepository.RemoveAsync(id);
        }

        public async Task<Models.Task?> RemoveUserByIdAsync(int id, string user)
        {
            var task = await taskRepository.GetByIdAsync(id);
            if (task != null)
            {
                var _user = await userRepository.GetByIdAsync(user);
                if (_user != null)
                {
                    foreach (User u in task.Users)
                    {
                        if (u.Id == _user.Id) { _user = u; }
                    }
                    if (task.Users.Remove(_user))
                    {
                        return await taskRepository.UpdateAsync(task);
                    }
                }
            }
            return null;
        }

        public async Task<Models.Task> UpdateAsync(Models.Task task)
        {
            return await taskRepository.UpdateAsync(task);
        }

        public async Task<ICollection<Models.Task>> RemoveListAsync(ICollection<Models.Task> list)
        {
            ICollection<Models.Task> deletedTasks = new List<Models.Task>();
            foreach (Models.Task t in list)
            {
                if (t.Id != null)
                {
                    var task = await taskRepository.GetByIdAsync((int)t.Id);
                    if (task != null)
                    {
                        await commentService.RemoveListAsync(task.Comments);
                        deletedTasks.Add(task);
                    }
                }
            }
            return deletedTasks;
        }

        public async Task<ICollection<Models.Task>> UpdateListAsync(ICollection<Models.Task> list)
        {
            return await taskRepository.UpdateListAsync(list);
        }
    }
}

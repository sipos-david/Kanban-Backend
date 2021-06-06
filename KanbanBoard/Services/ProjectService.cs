using KanbanBoard.DAL.Repositories;
using KanbanBoard.Lib;
using KanbanBoard.Models;
using Microsoft.CSharp.RuntimeBinder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KanbanBoard.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository projectRepository;
        private readonly IUserRepository userRepository;
        private readonly ITableService tableService;

        public ProjectService(IProjectRepository _projectRepository, IUserRepository _userRepository, ITableService _tableService)
        {
            projectRepository = _projectRepository;
            userRepository = _userRepository;
            tableService = _tableService;
        }

        public async Task<Project> AddAsync(Project project)
        {
            return await projectRepository.AddAsync(project);
        }

        public async Task<Project?> AddTableAsync(int id, Table table)
        {
            var project = await projectRepository.GetByIdAsync(id);
            if (project != null && project.Id != null)
            {
                if (table.Id == null)
                {
                    table.ProjectId = project.Id;
                    table = await tableService.AddAsync(table);
                }
                project.Tables.Add(table);
                return await projectRepository.GetByIdAsync((int)project.Id);
            }
            return null;
        }

        public async Task<Project?> AddUsersByIdAsync(int id, List<string> users)
        {
            var project = await projectRepository.GetByIdAsync(id);
            if (project != null)
            {
                for (int i = 0; i < users.Count; i++)
                {
                    var user = await userRepository.GetByIdAsync(users[i]);
                    if (user != null && !project.Users.Contains(user))
                    {
                        project.Users.Add(user);
                    }
                }
                return await projectRepository.UpdateAsync(project);
            }
            return null;
        }

        public async Task<ICollection<Project>> GetAllAsync()
        {
            return await projectRepository.GetAllProjectsAsync();
        }

        public Task<Project?> GetByIdAsync(int id)
        {
            return projectRepository.GetByIdAsync(id);
        }

        public async Task<Project?> PatchByIdAsync(int id, dynamic project)
        {
            var _project = await projectRepository.GetByIdAsync(id);
            if (_project != null)
            {
                bool updated = false;
                bool? isNameExists = DynamicLib.IsPropertyExists(project, "name");
                if (isNameExists == null || isNameExists == true)
                {
                    _project.Name = project.name;
                    updated = true;
                }
                if (updated)
                {
                    return await projectRepository.UpdateAsync(_project);
                }
            }
            return null;
        }

        public async Task<Project?> RemoveAsync(int id)
        {
            var project = await projectRepository.GetByIdAsync(id);
            if (project != null && project.Id != null)
            {
                await tableService.RemoveListAsync(project.Tables);
                return await projectRepository.RemoveAsync(id);
            }
            return null;
        }

        public async Task<Project?> RemoveTableAsync(int id, int table)
        {
            var project = await projectRepository.GetByIdAsync(id);
            if (project != null)
            {
                var _table = await tableService.GetByIdAsync(table);
                if (_table != null)
                {
                    await tableService.RemoveAsync(table);
                    return await projectRepository.GetByIdAsync(id);
                }
            }
            return null;
        }

        public async Task<Project?> RemoveUserByIdAsync(int id, string user)
        {
            var project = await projectRepository.GetByIdAsync(id);
            if (project != null)
            {
                var _user = await userRepository.GetByIdAsync(user);
                if (_user != null)
                {
                    // Collection referenciát néz valószínűleg :,(
                    foreach (User u in project.Users)
                    {
                        if (u.Id == _user.Id)
                        {
                            _user = u;
                        }
                    }
                    project.Users.Remove(_user);
                    return await projectRepository.UpdateAsync(project);
                }
            }
            return null;
        }

        public async Task<Project> UpdateAsync(Project project)
        {
            return await projectRepository.UpdateAsync(project);
        }
    }
}

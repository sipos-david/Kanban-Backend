using KanbanBoard.Models;
using System.Threading.Tasks;
using KanbanBoard.DAL.Repositories;
using System.Collections.Generic;
using Microsoft.CSharp.RuntimeBinder;
using KanbanBoard.Lib;

namespace KanbanBoard.Services
{
    public class ColumnService : IColumnService
    {
        private readonly IColumnRepository columnRepository;
        private readonly ITaskService taskService;

        public ColumnService(IColumnRepository _columnRepository, ITaskService _taskService)
        {
            columnRepository = _columnRepository;
            taskService = _taskService;
        }

        public async Task<Column> AddAsync(Column column)
        {
            return await columnRepository.AddAsync(column);
        }

        public async Task<Models.Task?> AddTaskByIdAsync(int id, Models.Task task)
        {
            task.ColumnId = id;
            return await MoveTaskByIdAsync(id, task);
        }

        public async Task<Column?> GetByIdAsync(int id)
        {
            return await columnRepository.GetByIdAsync(id);
        }

        public async Task<Models.Task?> MoveTaskByIdAsync(int id, Models.Task task)
        {
            var column = await columnRepository.GetByIdAsync(id);
            if (column != null && column.Id != null)
            {
                if (task.Id != null && task.ColumnId != null)
                {
                    var _task = await taskService.GetByIdAsync((int)task.Id);
                    if (_task != null)
                    {
                        // ha helyben lett mozgatva
                        if (task.Number == _task.Number && _task.ColumnId == task.ColumnId)
                        {
                            return _task;
                        }
                        // ha csak fel kell cserelni egy columnban
                        if (_task.ColumnId == task.ColumnId)
                        {
                            return await MoveTaskInColumn(column, _task, task);
                        }
                        // ha mar volt masik columnban, akkor eltoljuk a number-eket a regi column-ban
                        else
                        {
                            var _column = await columnRepository.GetByIdAsync((int)task.ColumnId);
                            if (_column != null && _column.Tasks.Count > 0)
                            {
                                await TasksNumbersDecrement(_column, _task);
                            }
                        }
                    }
                }
                // eltoljuk a number-eket az uj column-ban
                if (column.Tasks.Count > 0)
                {
                    await TasksNumbersIncrement(column, task);
                }
                // beallitjuk a dolgokat es letrehozzuk / modositjuk
                task.ColumnId = column.Id;
                return await AddOrUpdateTask(task);
            }
            return null;
        }

        private async Task<Models.Task?> MoveTaskInColumn(Column column, Models.Task oldPos, Models.Task newPos)
        {
            ICollection<Models.Task> updatedTasks = new List<Models.Task>();
            if (newPos.Number < oldPos.Number)
            {
                foreach (Models.Task t in column.Tasks)
                {
                    if (newPos.Number <= t.Number && t.Number < oldPos.Number)
                    {
                        t.Number++;
                        updatedTasks.Add(t);
                    }
                }
            }
            if (newPos.Number > oldPos.Number)
            {
                foreach (Models.Task t in column.Tasks)
                {
                    if (oldPos.Number < t.Number && t.Number <= newPos.Number)
                    {
                        t.Number--;
                        updatedTasks.Add(t);
                    }
                }
            }
            await taskService.UpdateListAsync(updatedTasks);
            return await taskService.UpdateAsync(newPos);
        }

        private async Task<Models.Task?> AddOrUpdateTask(Models.Task task)
        {
            if (task.Id == null)
            {
                return await taskService.AddAsync(task);
            }
            else
            {
                return await taskService.UpdateAsync(task);
            }
        }

        private async Task<ICollection<Models.Task>> TasksNumbersDecrement(Column column, Models.Task task)
        {
            ICollection<Models.Task> updatedTasks = new List<Models.Task>();
            foreach (Models.Task t in column.Tasks)
            {
                if (task.Id != t.Id && task.Number < t.Number)
                {
                    t.Number--;
                    updatedTasks.Add(t);
                }
            }
            return await taskService.UpdateListAsync(updatedTasks);
        }

        private async Task<ICollection<Models.Task>> TasksNumbersIncrement(Column column, Models.Task task)
        {
            ICollection<Models.Task> updatedTasks = new List<Models.Task>();
            foreach (Models.Task t in column.Tasks)
            {
                if (task.Id != t.Id && task.Number <= t.Number)
                {
                    t.Number++;
                    updatedTasks.Add(t);
                }
            }
            return await taskService.UpdateListAsync(updatedTasks);
        }

        public async Task<Column?> PatchByIdAsync(int id, dynamic column)
        {
            var _column = await columnRepository.GetByIdAsync(id);
            if (_column != null)
            {
                bool updated = false;
                bool? isNameExists = DynamicLib.IsPropertyExists(column, "name");
                if (isNameExists == null || isNameExists == true)
                {
                    _column.Name = column.name;
                    updated = true;
                }
                if (updated)
                {
                    return await columnRepository.UpdateAsync(_column);
                }
            }
            return null;
        }

        public async Task<Column?> RemoveAsync(int id)
        {
            return await columnRepository.RemoveAsync(id);
        }

        public async Task<Models.Task?> RemoveTaskByIdAsync(int id, int task)
        {
            var column = await columnRepository.GetByIdAsync(id);
            if (column != null)
            {
                var _task = await taskService.GetByIdAsync(task);
                if (_task != null && _task.Id != null)
                {
                    bool found = false;
                    foreach (Models.Task t in column.Tasks)
                    {
                        if (t.Id == _task.Id) { found = true; }
                    }
                    if (found) { return await taskService.RemoveAsync((int)_task.Id); }
                }
            }
            return null;
        }

        public async Task<Column> UpdateAsync(Column column)
        {
            return await columnRepository.UpdateAsync(column);
        }

        public async Task<ICollection<Column>> RemoveListAsync(ICollection<Column> list)
        {
            ICollection<Column> deletedColumns = new List<Column>();
            foreach (Column c in list)
            {
                if (c.Id != null)
                {
                    var column = await columnRepository.GetByIdAsync((int)c.Id);
                    if (column != null)
                    {
                        await taskService.RemoveListAsync(column.Tasks);
                        deletedColumns.Add(column);
                    }
                }
            }
            return deletedColumns;
        }
    }
}

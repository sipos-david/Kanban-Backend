using KanbanBoard.DAL.Repositories;
using KanbanBoard.Lib;
using KanbanBoard.Models;
using Microsoft.CSharp.RuntimeBinder;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KanbanBoard.Services
{
    public class TableService : ITableService
    {
        private readonly ITableRepository tableRepository;
        private readonly IColumnService columnService;

        public TableService(ITableRepository _tableRepository, IColumnService _columnService)
        {
            tableRepository = _tableRepository;
            columnService = _columnService;
        }

        public async Task<Table?> AddColumnAsync(int id, Column column)
        {
            return await MoveColumnByIdAsync(id, column);
        }

        private void MoveColumns(Table table, Column column)
        {
            System.Threading.Tasks.Task[]? tasks = null;
            var shouldMoveColumns = false;
            int moveStartIndex = 0;
            var lastColumn = table.Columns.ElementAt(0);
            for (int i = 1; i < table.Columns.Count; i++)
            {
                var nextColumn = table.Columns.ElementAt(i);
                if (lastColumn.Number <= column.Number && column.Number <= nextColumn.Number)
                {
                    if (lastColumn.Number == column.Number)
                    {
                        column.Number += 1;
                    }
                    if (column.Number == nextColumn.Number)
                    {
                        nextColumn.Number += 1;
                        columnService.UpdateAsync(nextColumn).Wait();
                        if (nextColumn.Number == table.Columns.ElementAt(i + 1).Number)
                        {
                            shouldMoveColumns = true;
                            moveStartIndex = i + 1;
                            tasks = new System.Threading.Tasks.Task[table.Columns.Count - moveStartIndex];
                        }
                    }
                    i = table.Columns.Count;
                }
            }
            if (shouldMoveColumns && tasks != null)
            {
                for (int i = moveStartIndex; i < table.Columns.Count; i++)
                {
                    var movedColumn = table.Columns.ElementAt(i);
                    movedColumn.Number += 1;
                    tasks[i - moveStartIndex] = columnService.UpdateAsync(movedColumn);
                }
                System.Threading.Tasks.Task.WhenAll(tasks);
            }
        }

        public async Task<Table> AddAsync(Table table)
        {
            return await tableRepository.AddAsync(table);
        }

        public async Task<Table?> GetByIdAsync(int id)
        {
            return await tableRepository.GetByIdAsync(id);
        }

        public async Task<Table?> MoveColumnByIdAsync(int id, Column column)
        {
            var table = await tableRepository.GetByIdAsync(id);
            if (table != null && table.Id != null)
            {
                if (table.Columns.Count > 0 && table.Columns.Last().Number > column.Number)
                {
                    MoveColumns(table, column);
                }
                table.Columns.Add(column);
                column.TableId = table.Id;
                if (column.Id == null)
                {
                    await columnService.AddAsync(column);
                }
                else
                {
                    await columnService.UpdateAsync(column);
                }
                return await tableRepository.GetByIdAsync((int)table.Id);
            }
            return null;
        }

        public async Task<Table?> PatchByIdAsync(int id, dynamic table)
        {
            var _table = await tableRepository.GetByIdAsync(id);
            if (_table != null)
            {
                bool updated = false;
                bool? isNameExists = DynamicLib.IsPropertyExists(table, "name");
                if (isNameExists == null || isNameExists == true)
                {
                    _table.Name = table.name;
                    updated = true;
                }
                if (updated)
                {
                    return await tableRepository.UpdateAsync(_table);
                }
            }
            return null;
        }

        public async Task<Table?> RemoveColumnAsync(int id, int column)
        {
            var table = await tableRepository.GetByIdAsync(id);
            if (table != null)
            {
                var _column = await columnService.GetByIdAsync(column);
                if (_column != null && _column.Id != null)
                {
                    bool found = false;
                    foreach (Column c in table.Columns)
                    {
                        if (c.Id == _column.Id) { found = true;  }
                    }
                    if (found)
                    {
                        await columnService.RemoveAsync((int)_column.Id);
                        return await tableRepository.GetByIdAsync(id);
                    }
                }
            }
            return null;
        }

        public async Task<Table?> RemoveAsync(int id)
        {
            return await tableRepository.RemoveAsync(id);
        }

        public async Task<Table> UpdateAsync(Table table)
        {
            return await tableRepository.UpdateAsync(table);
        }

        public async Task<ICollection<Table>> RemoveListAsync(ICollection<Table> list)
        {
            ICollection<Table> deletedTables = new List<Table>();
            foreach (Table t in list)
            {
                if (t.Id != null)
                {
                    var table = await tableRepository.GetByIdAsync((int)t.Id);
                    if (table != null)
                    {
                        await columnService.RemoveListAsync(table.Columns);
                        deletedTables.Add(table);
                    }
                }
            }
            return deletedTables;
        }
    }
}

using KanbanBoard.Lib;
using KanbanBoard.Models;
using KanbanBoard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KanbanBoard.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class ColumnsController : ControllerBase
    {
        private readonly IColumnService columnService;

        public ColumnsController(IColumnService _columnService)
        {
            columnService = _columnService;
        }

        // GET api/<ColumnsController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Column))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Column>> GetAsync(int id)
        {
            var column = await columnService.GetByIdAsync(id);
            if (column != null)
            {
                return Ok(column);
            }
            return NotFound("Column not found");
        }

        // POST api/<ColumnsController>
        [HttpPost]
        public async Task<Column> PostAsync([FromBody] Column column)
        {
            return await columnService.AddAsync(column);
        }

        // PUT api/<ColumnsController>
        [HttpPut]
        public async Task<Column> PutAsync([FromBody] Column column)
        {
            return await columnService.UpdateAsync(column);
        }

        // DELETE api/<ColumnsController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Project))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Project?>> DeleteAsync(int id)
        {
            var column = await columnService.RemoveAsync(id);
            if (column != null)
            {
                return Ok(column);
            }
            return NotFound("Column not found");
        }

        // PATCH api/<ColumnsController>/5
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Project))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Project>> Patch(int id, [FromBody] object body)
        {
            dynamic? column = DynamicLib.Convert(body);
            if (column != null)
            {
                var _column = await columnService.PatchByIdAsync(id, column);
                if (_column != null)
                {
                    return Ok(_column);
                }
            }
            return BadRequest("Bad properties");
        }

        // POST api/<ColumnsController>/5/tasks
        [HttpPost("{id}/tasks")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Column))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Models.Task>> AddTaskAsync(int id, [FromBody] Models.Task task)
        {
            var newTask = await columnService.AddTaskByIdAsync(id, task);
            if (newTask != null)
            {
                return Ok(newTask);
            }
            return NotFound("Column not found");
        }

        // PUT api/<ColumnsController>/5/tasks
        [HttpPut("{id}/tasks")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Column))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Models.Task>> PutTaskByIdAsync(int id, [FromBody] Models.Task task)
        {
            var movedTask = await columnService.MoveTaskByIdAsync(id, task);
            if (movedTask != null)
            {
                return Ok(movedTask);
            }
            return NotFound("Column not found");
        }

        // DELETE api/<ColumnsController>/5/tasks
        [HttpDelete("{id}/tasks")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Column))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Models.Task>> DeleteTaskByIdAsync(int id, [FromQuery] int task)
        {
            var deletedTask = await columnService.RemoveTaskByIdAsync(id, task);
            if (deletedTask != null)
            {
                return Ok(deletedTask);
            }
            return NotFound("Column not found");
        }
    }
}

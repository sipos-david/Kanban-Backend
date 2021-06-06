using KanbanBoard.Lib;
using KanbanBoard.Models;
using KanbanBoard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KanbanBoard.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService taskService;

        public TasksController(ITaskService _taskService)
        {
            taskService = _taskService;
        }

        // GET api/<TasksController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Models.Task))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Models.Task>> GetAsync(int id)
        {
            var task = await taskService.GetByIdAsync(id);
            if (task != null)
            {
                return Ok(task);
            }
            return NotFound("Task not found");
        }

        // POST api/<TasksController>
        [HttpPost]
        public async Task<Models.Task> PostAsync([FromBody] Models.Task task)
        {
            return await taskService.AddAsync(task);
        }

        // PUT api/<TasksController>/5
        [HttpPut]
        public async Task<Models.Task> PutAsync([FromBody] Models.Task task)
        {
            return await taskService.UpdateAsync(task);
        }

        // DELETE api/<TasksController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Table))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Models.Task?>> DeleteAsync(int id)
        {
            var task = await taskService.RemoveAsync(id);
            if (task != null)
            {
                return Ok(task);
            }
            return NotFound("Task not found");
        }

        // PUT api/<TasksController>/5
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Table))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Models.Task>> Patch(int id, [FromBody] object body)
        {
            dynamic? task = DynamicLib.Convert(body);
            if (task != null)
            {
                var _task = await taskService.PatchByIdAsync(id, task);
                if (_task != null)
                {
                    return Ok(_task);
                }
            }
            return BadRequest("Bad properties");
        }

        // POST api/<TasksController>/5/users
        [HttpPost("{id}/users")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Models.Task))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Models.Task>> AddUserAsync(int id, [FromQuery] List<string> users)
        {
            var task = await taskService.AddUsersByIdAsync(id, users);
            if (task != null)
            {
                return Ok(task);
            }
            return NotFound("Task not found");
        }

        // DELETE api/<TasksController>/5/users
        [HttpDelete("{id}/users")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Models.Task))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Models.Task>> DeleteUserByIdAsync(int id, [FromQuery] string user)
        {
            var task = await taskService.RemoveUserByIdAsync(id, user);
            if (task != null)
            {
                return Ok(task);
            }
            return NotFound("Task not found");
        }

        // POST api/<TasksController>/5/users
        [HttpPost("{id}/comments")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Models.Task))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Models.Task>> AddCommentAsync(int id, [FromBody] Comment comment)
        {
            var task = await taskService.AddCommentAsync(id, comment);
            if (task != null)
            {
                return Ok(task);
            }
            return NotFound("Task not found");
        }

        // DELETE api/<TasksController>/5/users
        [HttpDelete("{id}/comments")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Models.Task))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Comment>> DeleteCommentByIdAsync(int id, [FromQuery] int comment)
        {
            var task = await taskService.RemoveCommentByIdAsync(id, comment);
            if (task != null)
            {
                return Ok(task);
            }
            return NotFound("Task not found");
        }
    }
}

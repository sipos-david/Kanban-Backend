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
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService projectService;

        public ProjectsController(IProjectService _projectService)
        {
            projectService = _projectService;
        }

        // GET: api/<ProjectsController>
        [HttpGet]
        public async Task<IEnumerable<Project>> Get()
        {
            return await projectService.GetAllAsync();
        }

        // GET api/<ProjectsController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Project))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Project>> GetAsync(int id)
        {
            var project = await projectService.GetByIdAsync(id);
            if (project != null)
            {
                return Ok(project);
            }
            return NotFound("Project not found");
        }

        //POST api/<ProjectsController>
        [HttpPost]
        public async Task<Project> PostAsync([FromBody] Project project)
        {
            return await projectService.AddAsync(project);
        }

        // PUT api/<ProjectsController>/5
        [HttpPut]
        public async Task<Project> PutAsync([FromBody] Project project)
        {
            return await projectService.UpdateAsync(project);
        }

        // DELETE api/<ProjectsController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Project))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Project?>> DeleteAsync(int id)
        {
            var project = await projectService.RemoveAsync(id);
            if (project != null)
            {
                return Ok(project);
            }
            return NotFound("Project not found");
        }

        // PATCH api/<ProjectsController>/5
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Project))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Project>> Patch(int id, [FromBody] object body)
        {
            dynamic? project = DynamicLib.Convert(body);
            if (project != null)
            {
                var _project = await projectService.PatchByIdAsync(id, project);
                if (_project != null)
                {
                    return Ok(_project);
                }  
            }
            return BadRequest("Bad properties");
        }

        //POST api/<ProjectsController>
        [HttpPost("{id}/tables")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Project))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Project>> AddTableByIdAsync(int id, [FromBody] Table table)
        {
            var project = await projectService.AddTableAsync(id, table);
            if (project != null)
            {
                return Ok(project);
            }
            return BadRequest("Model error");
        }

        // DELETE api/<ProjectsController>/5
        [HttpDelete("{id}/tables")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Project))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Project>> RemoveTableByIdAsync(int id, [FromQuery] int table)
        {
            var project = await projectService.RemoveTableAsync(id, table);
            if (project != null)
            {
                return Ok(project);
            }
            return NotFound("Project or table not found");
        }

        // POST api/<ProjectsController>/5/users
        [HttpPost("{id}/users")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Project))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Project>> AddUserAsync(int id, [FromQuery] List<string> users)
        {
            var project = await projectService.AddUsersByIdAsync(id, users);
            if (project != null)
            {
                return Ok(project);
            }
            return NotFound("Project or user not found");
        }

        // DELETE api/<ProjectsController>/5/users
        [HttpDelete("{id}/users")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Project))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Project>> DeleteUserByIdAsync(int id, [FromQuery] string user)
        {
            var project = await projectService.RemoveUserByIdAsync(id, user);
            if (project != null)
            {
                return Ok(project);
            }
            return NotFound("Project or user not found");
        }
    }
}

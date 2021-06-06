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
    public class TablesController : ControllerBase
    {
        private readonly ITableService tableService;

        public TablesController(ITableService _tableService)
        {
            tableService = _tableService;
        }

        // GET api/<TablesController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Table))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Table>> GetAsync(int id)
        {
            var table = await tableService.GetByIdAsync(id);
            if (table != null)
            {
                return Ok(table);
            }
            return NotFound("Table not found");
        }

        // POST api/<TablesController>
        [HttpPost]
        public async Task<Table> PostAsync([FromBody] Table table)
        {
            return await tableService.AddAsync(table);
        }

        // PUT api/<TablesController>/5
        [HttpPut]
        public async Task<Table> PutAsync([FromBody] Table table)
        {
            return await tableService.UpdateAsync(table);
        }

        // DELETE api/<TablesController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Table>> DeleteAsync(int id)
        {
            var table = await tableService.RemoveAsync(id);
            if (table != null)
            {
                return Ok(table);
            }
            return NotFound("Table not found");
        }

        // PUT api/<TablesController>/5
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Table))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Table>> Patch(int id, [FromBody] object body)
        {
            dynamic? table = DynamicLib.Convert(body);
            if (table != null)
            {
                var _table = await tableService.PatchByIdAsync(id, table);
                if (_table != null)
                {
                    return Ok(_table);
                }
            }
            return BadRequest("Bad properties");
        }

        // POST api/<TablesController>/5/columns
        [HttpPost("{id}/columns")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Column))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Table>> AddColumnAsync(int id, [FromBody] Column column)
        {
            var table = await tableService.AddColumnAsync(id, column);
            if (table != null)
            {
                return Ok(table);
            }
            return NotFound("Table not found");
        }

        // PUT api/<TablesController>/5/columns
        [HttpPut("{id}/columns")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Column))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Table>> MoveColumnByIdAsync(int id, [FromBody] Column column)
        {
            var table = await tableService.MoveColumnByIdAsync(id, column);
            if (table != null)
            {
                return Ok(table);
            }
            return NotFound("Table not found");
        }

        // DELETE api/<TablesController>/5/columns
        [HttpDelete("{id}/columns")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Column))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Table>> RemoveColumnAsync(int id, [FromQuery] int column)
        {
            var table = await tableService.RemoveColumnAsync(id, column);
            if (table != null)
            {
                return Ok(table);
            }
            return NotFound("Table not found");
        }
    }
}

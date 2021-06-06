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
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService commentService;

        public CommentsController(ICommentService _commentService)
        {
            commentService = _commentService;
        }

        // GET api/<CommentsController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Comment))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Comment>> GetAsync(int id)
        {
            var comment = await commentService.GetByIdAsync(id);
            if (comment != null)
            {
                return Ok(comment);
            }
            return NotFound("Comment not found");
        }

        // POST api/<CommentController>
        [HttpPost]
        public async Task<Comment> PostAsync([FromBody] Comment comment)
        {
            return await commentService.AddAsync(comment);
        }

        // PUT api/<CommentsController>/5
        [HttpPut]
        public async Task<Comment> PutAsync([FromBody] Comment comment)
        {
            return await commentService.UpdateAsync(comment);
        }

        // DELETE api/<CommentsController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Project))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Comment?>> DeleteAsync(int id)
        {
            var comment = await commentService.RemoveAsync(id);
            if (comment != null)
            {
                return Ok(comment);
            }
            return NotFound("Comment not found");
        }

        // PUT api/<CommentsController>/5
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Comment))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Comment>> Patch(int id, [FromBody] object body)
        {
            dynamic? comment = DynamicLib.Convert(body);
            if (comment != null)
            {
                var _comment = await commentService.PatchByIdAsync(id, comment);
                if (_comment != null)
                {
                    return Ok(_comment);
                }
            }
            return BadRequest("Bad properties");
        }
    }
}

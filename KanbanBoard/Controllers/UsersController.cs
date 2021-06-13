using KanbanBoard.Models;
using KanbanBoard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KanbanBoard.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;

        public UsersController(IUserService _userService)
        {
            userService = _userService;
        }

        // GET: api/<UsersController>
        [HttpGet]
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await userService.GetAllAsync();
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> GetByIdAsync(string id)
        {
            var user = await userService.GetByIdAsync(id);
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound("User not found");
        }

        // GET: api/<UsersController>/registration
        [HttpGet("registration")]
        public async Task<bool> GetUserRegistration()
        {
            return await userService.IsUserRegistered(User.Claims?.FirstOrDefault(c => c.Type == "is_profile_id")?.Value);
        }

        [HttpPost("registration")]
        public async Task<User?> RegisterUser()
        {
            return await userService.RegisterUser(
                new User
                {
                    Name = User.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value,
                    Id = User.Claims?.FirstOrDefault(c => c.Type == "is_profile_id")?.Value
                });
        }
    }
}

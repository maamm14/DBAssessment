using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersTasks.Models;
using UsersTasks.Models.DTO;
using UsersTasks.Services;

namespace UsersTasks.Controllers
{
    [Authorize]
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<List<User>> GetAllUsers()
        {
            return await _userService.GetAllUsersAsync();
        }

        [HttpGet("{id}")]
        public async Task<User> GetUserById(int id)
        {
            return await _userService.GetUserByIdAsync(id);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task Post([FromBody] CreateUser createUser)
        {
            await _userService.CreateUserAsync(createUser);
        }

        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] UpdateUser updateUser)
        {
            await _userService.UpdateUserAsync(updateUser);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _userService.DeleteUserAsync(id);
        }
    }

}

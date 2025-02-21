using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersTasks.Models;
using UsersTasks.Services;
using UsersTasks.Models.DTO;

namespace UsersTasks.Controllers
{
    [Authorize]
    [Route("api/task")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<List<TaskItem>> Get()
        {
            return await _taskService.GetAllTasksAsync();
        }

        [HttpGet("{id}")]
        public async Task<TaskItem> Get(int id)
        {
            return await _taskService.GetTaskByIdAsync(id);
        }

        [HttpPost]
        public async Task Post([FromBody] CreateTask createTask)
        {
            await _taskService.CreateTaskAsync(createTask);
        }

        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] UpdateTask updateTask)
        {
            await _taskService.UpdateTaskAsync(updateTask);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _taskService.DeleteTaskAsync(id);
        }

        [HttpGet("expired")]
        public async Task<List<TaskItem>> GetExpiredTasks()
        {
            return await _taskService.GetExpiredTasksAsync();
        }

        [HttpGet("active")]
        public async Task<List<TaskItem>> GetActiveTasks()
        {
            return await _taskService.GetActiveTasksAsync();
        }

        [HttpGet("date/{date}")]
        public async Task<List<TaskItem>> GetTasksByDate(DateTime date)
        {
            return await _taskService.GetTasksByDateAsync(date);
        }
    }

}

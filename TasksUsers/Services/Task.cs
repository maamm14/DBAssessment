using UsersTasks.Models;
using UsersTasks.Repositories;
using UsersTasks.Models.DTO;

namespace UsersTasks.Services
{
    public interface ITaskService
    {
        Task<List<TaskItem>> GetAllTasksAsync();
        Task<TaskItem> GetTaskByIdAsync(int id);
        Task CreateTaskAsync(CreateTask createTask);
        Task UpdateTaskAsync(UpdateTask updateTask);
        Task DeleteTaskAsync(int id);
        Task<List<TaskItem>> GetExpiredTasksAsync();
        Task<List<TaskItem>> GetActiveTasksAsync();
        Task<List<TaskItem>> GetTasksByDateAsync(DateTime date);
    }

    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;

        public TaskService(ITaskRepository taskRepository, IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
        }

        public async Task<List<TaskItem>> GetAllTasksAsync()
        {
            return await _taskRepository.GetAllAsync();
        }

        public async Task<TaskItem> GetTaskByIdAsync(int id)
        {
            return await _taskRepository.GetByIdAsync(id);
        }

        public async Task CreateTaskAsync(CreateTask createTask)
        {
            var userExists = await _userRepository.GetByIdAsync(createTask.AssigneeId) ?? throw new KeyNotFoundException($"User with id {createTask.AssigneeId} not found.");
            var taskItem = new TaskItem
            {
                Title = createTask.Title,
                Description = createTask.Description,
                DueDate = DateTime.UtcNow,
            };
            await _taskRepository.AddAsync(taskItem);
        }

        public async Task UpdateTaskAsync(UpdateTask updateTask)
        {
            var taskItem = new TaskItem
            {
                Id = updateTask.Id,
                Title = updateTask.Title,
                Description = updateTask.Description,
                DueDate = DateTime.Now,
            };
            await _taskRepository.UpdateAsync(taskItem);
        }

        public async Task DeleteTaskAsync(int id)
        {
            await _taskRepository.DeleteAsync(id);
        }

        public async Task<List<TaskItem>> GetExpiredTasksAsync()
        {
            return await _taskRepository.GetExpiredTasksAsync();
        }

        public async Task<List<TaskItem>> GetActiveTasksAsync()
        {
            return await _taskRepository.GetActiveTasksAsync();
        }

        public async Task<List<TaskItem>> GetTasksByDateAsync(DateTime date)
        {
            return await _taskRepository.GetTasksByDateAsync(date);
        }
    }

}

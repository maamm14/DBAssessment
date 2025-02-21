using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using UsersTasks.Data;
using UsersTasks.Models;

namespace UsersTasks.Repositories
{
     public interface ITaskRepository
    {
        Task<List<TaskItem>> GetAllAsync();
        Task<TaskItem> GetByIdAsync(int id);
        Task AddAsync(TaskItem task);
        Task UpdateAsync(TaskItem task);
        Task DeleteAsync(int id);
        Task<List<TaskItem>> GetExpiredTasksAsync();
        Task<List<TaskItem>> GetActiveTasksAsync();
        Task<List<TaskItem>> GetTasksByDateAsync(DateTime date);
    }
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TaskItem>> GetAllAsync()
        {
            return await _context.TaskItems.ToListAsync();
        }

        public async Task<TaskItem> GetByIdAsync(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null)
            {
                throw new KeyNotFoundException($"Task with id {id} not found.");
            }
            return task;
        }

        public async Task AddAsync(TaskItem task)
        {
            await _context.TaskItems.AddAsync(task);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TaskItem task)
        {
            _context.TaskItems.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task != null)
            {
                _context.TaskItems.Remove(task);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<TaskItem>> GetExpiredTasksAsync()
        {
            return await _context.TaskItems
                .Where(task => task.DueDate < DateTime.Today)
                .ToListAsync();
        }

        public async Task<List<TaskItem>> GetActiveTasksAsync()
        {
            return await _context.TaskItems
                .Where(task => task.DueDate >= DateTime.Today)
                .ToListAsync();
        }

        public async Task<List<TaskItem>> GetTasksByDateAsync(DateTime date)
        {
            return await _context.TaskItems
                .Where(task => task.DueDate.Date == date.Date)
                .ToListAsync();
        }
    }
}

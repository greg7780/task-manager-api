using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Data;
using TaskManagerApi.DTOs;
using TaskManagerApi.Models;

namespace TaskManagerApi.Services
{
    public interface ITaskService
    {
        Task<List<TaskItem>> GetTasks(int page, int pageSize);
        Task<TaskItem?> GetTaskById(long id);
        Task<long> CreateTask(TaskItemDTO dto);
        Task<bool> UpdateTask(long id, TaskItemDTO dto);
        Task<bool> DeleteTask(long id);
    }

    public class TaskService : ITaskService
    {
        private readonly TaskManagerContext _taskManagerContext;

        public TaskService(TaskManagerContext taskManagerContext)
        {
            this._taskManagerContext = taskManagerContext;
        }

        public async Task<List<TaskItem>> GetTasks(int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;

            var tasks = await _taskManagerContext.TaskItems
                .Where(i => i.Deleted == false)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return tasks;
        }

        public async Task<TaskItem?> GetTaskById(long id)
        {
            return await _taskManagerContext.TaskItems
                .FirstOrDefaultAsync(i => i.Id == id && i.Deleted == false);
        }

        public async Task<long> CreateTask(TaskItemDTO dto)
        {
            var newTask = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                Priority = dto.Priority,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "-",
                Deleted = false
            };

            await _taskManagerContext.TaskItems.AddAsync(newTask);
            await _taskManagerContext.SaveChangesAsync();

            return newTask.Id;
        }

        public async Task<bool> UpdateTask(long id, TaskItemDTO dto)
        {
            var existingTask = await _taskManagerContext.TaskItems
                .FirstOrDefaultAsync(i => i.Id == id && i.Deleted == false);

            if (existingTask == null)
                return false;

            existingTask.Title = dto.Title;
            existingTask.Description = dto.Description;
            existingTask.Priority = dto.Priority;
            existingTask.IsCompleted = dto.IsCompleted;
            existingTask.ModifiedAt = DateTime.UtcNow;
            existingTask.ModifiedBy = "-";

            await _taskManagerContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTask(long id)
        {
            var existingTask = await _taskManagerContext.TaskItems
                .FirstOrDefaultAsync(i => i.Id == id && i.Deleted == false);

            if (existingTask == null)
                return false;

            existingTask.Deleted = true;
            existingTask.ModifiedAt = DateTime.UtcNow;
            existingTask.ModifiedBy = "-";

            await _taskManagerContext.SaveChangesAsync();
            return true;
        }
    }
}

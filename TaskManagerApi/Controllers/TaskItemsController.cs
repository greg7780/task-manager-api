using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Data;
using TaskManagerApi.DTOs;
using TaskManagerApi.Models;

namespace TaskManagerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskItemsController : ControllerBase
    {
        private readonly TaskManagerContext taskManagerContext;

        public TaskItemsController(TaskManagerContext taskManagerContext)
        {
            this.taskManagerContext = taskManagerContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks(int page = 1, int pageSize = 10)
        {
            var skip = (page - 1) * pageSize;

            var tasks = await taskManagerContext.TaskItems
                .Where(i => i.Deleted == false)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(long id)
        {
            var task = await taskManagerContext.TaskItems.FirstOrDefaultAsync(i => i.Id == id && i.Deleted == false);
            if (task == null)
                return NotFound(new ErrorResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Task with id {id} not found.",
                    TraceId = HttpContext.TraceIdentifier
                });

            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskItemDTO task)
        {
            var addTask = new TaskItem() 
            { 
                Title = task.Title,
                Description = task.Description,
                Priority = task.Priority,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "-",
                Deleted = false
            };

            await taskManagerContext.TaskItems.AddAsync(addTask);
            await taskManagerContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { id = addTask.Id }, new { addTask.Id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(long id, TaskItemDTO task)
        {
            var updateTask = await taskManagerContext.TaskItems.Where(i => i.Id == id).FirstOrDefaultAsync();

            if (updateTask == null)
                return NotFound(new ErrorResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Task with id {id} not found.",
                    TraceId = HttpContext.TraceIdentifier
                });

            updateTask.Title = task.Title;
            updateTask.Description = task.Description;
            updateTask.Priority = task.Priority;
            updateTask.IsCompleted = task.IsCompleted;
            updateTask.ModifiedAt = DateTime.UtcNow;
            updateTask.ModifiedBy = "-";

            await taskManagerContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(long id)
        {
            var deleteTask = await taskManagerContext.TaskItems.FirstOrDefaultAsync(i => i.Id == id && i.Deleted == false);
            if (deleteTask == null)
                return NotFound(new ErrorResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Task with id {id} not found.",
                    TraceId = HttpContext.TraceIdentifier
                });

            deleteTask.Deleted = true;
            deleteTask.ModifiedAt = DateTime.UtcNow;
            deleteTask.ModifiedBy = "-";

            await taskManagerContext.SaveChangesAsync();
            return NoContent();
        }
    }
}

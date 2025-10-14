using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerApi.DTOs;
using TaskManagerApi.Services;

namespace TaskManagerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TaskItemsController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskItemsController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks(int page = 1, int pageSize = 10)
        {
            return Ok(await _taskService.GetTasks(page, pageSize));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(long id)
        {
            var task = await _taskService.GetTaskById(id);
            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskItemDTO task)
        {
            var id = await _taskService.CreateTask(task);
            return CreatedAtAction(nameof(GetTask), new { id }, new { id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(long id, TaskItemDTO task)
        {
            await _taskService.UpdateTask(id, task);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(long id)
        {
            await _taskService.DeleteTask(id);
            return NoContent();
        }
    }
}

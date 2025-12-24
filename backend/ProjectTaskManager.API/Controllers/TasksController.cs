namespace ProjectTaskManager.API.Controllers;

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectTaskManager.Application.DTOs.Tasks;
using ProjectTaskManager.Application.Interfaces;

[Authorize]
[ApiController]
[Route("api/projects/{projectId}/tasks")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim!);
    }

    [HttpGet]
    public async Task<IActionResult> GetTasks(
        Guid projectId,
        [FromQuery] string? searchTerm,
        [FromQuery] bool? isCompleted,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var userId = GetUserId();
        var filter = new TaskFilterRequest
        {
            SearchTerm = searchTerm,
            IsCompleted = isCompleted,
            PageNumber = pageNumber,
            PageSize = Math.Min(pageSize, 50) // Max 50 items per page
        };

        var result = await _taskService.GetTasksAsync(projectId, userId, filter);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTask(Guid projectId, Guid id)
    {
        var userId = GetUserId();
        var task = await _taskService.GetTaskByIdAsync(id, userId);

        if (task == null)
            return NotFound(new { message = "Task not found" });

        return Ok(task);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask(Guid projectId, [FromBody] CreateTaskRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var userId = GetUserId();
            var task = await _taskService.CreateTaskAsync(projectId, request, userId);

            return CreatedAtAction(nameof(GetTask), new { projectId, id = task.Id }, task);
        }
        catch (UnauthorizedAccessException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(Guid projectId, Guid id, [FromBody] UpdateTaskRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetUserId();
        var task = await _taskService.UpdateTaskAsync(id, request, userId);

        if (task == null)
            return NotFound(new { message = "Task not found" });

        return Ok(task);
    }

    [HttpPatch("{id}/complete")]
    public async Task<IActionResult> CompleteTask(Guid projectId, Guid id)
    {
        var userId = GetUserId();
        var result = await _taskService.MarkTaskAsCompletedAsync(id, userId);

        if (!result)
            return NotFound(new { message = "Task not found" });

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(Guid projectId, Guid id)
    {
        var userId = GetUserId();
        var result = await _taskService.DeleteTaskAsync(id, userId);

        if (!result)
            return NotFound(new { message = "Task not found" });

        return NoContent();
    }
}
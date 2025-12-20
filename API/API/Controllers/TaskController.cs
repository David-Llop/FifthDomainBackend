using API.DTO.TodoTask;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/tasks")]
[ApiController]
public class TaskController : ControllerBase
{

    private TodoTaskService _todoService;

    public TaskController(TodoTaskService todoService)
    {
        _todoService = todoService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TodoTaskDto>>> GetAsync([FromHeader] string userId)
    {
        var tasks = (await _todoService.GetUserTasksAsync(userId)).Select(t => new TodoTaskDto
        {
            Id = t.Id,
            Created = t.Created,
            Description = t.Description,
            IsCompleted = t.IsCompleted,
            Title = t.Title
        });
        return Ok(tasks);
    }

    [HttpPost]
    public async Task<ActionResult<string>> CreateAsync([FromHeader] string userId, [FromBody] CreateTodoTaskDto dto)
    {
        return CreatedAtAction(nameof(GetTodoTaskAsync), new { id = "id" }, null);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoTaskDto>> GetTodoTaskAsync([FromHeader] string userId, string id)
    {
        var task = await _todoService.GetTaskAsync(userId, id);
        if (task == null)
        {
            return NotFound();
        }
        return Ok(new TodoTaskDto
        {
            Id = task.Id,
            Title = task.Title,
            IsCompleted = task.IsCompleted,
            Created = task.Created,
            Description = task.Description
        });
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTodoTaskAsync([FromHeader] string userId, string id)
    {
        return Ok();
    }

    [HttpPatch("{id}/completed")]
    public async Task<ActionResult> MarkTodoTaskCompletedAsync([FromHeader] string userId, string id)
    {
        return Ok();
    }


}

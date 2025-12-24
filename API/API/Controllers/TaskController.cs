using API.DTO.TodoTask;
using API.Exceptions;
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<TodoTaskDto>>> Get([FromHeader] string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return BadRequest();
        }
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
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TodoTaskDto>> Create([FromHeader] string userId, [FromBody] CreateTodoTaskDto dto)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return BadRequest();
        }
        try
        {
            var createTask = await _todoService.CreateTaskAsync(dto, userId);
            return CreatedAtAction(nameof(GetTodoTask), new { id = createTask.Id }, new TodoTaskDto
            {
                Id = createTask.Id,
                Created = createTask.Created,
                Description = createTask.Description,
                IsCompleted = createTask.IsCompleted,
                Title = createTask.Title
            });
        }
        // Should not happen, but just in case
        catch (TodoTaskAlreadyExistsException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TodoTaskDto>> GetTodoTask([FromHeader] string userId, string id)
    {
        if (string.IsNullOrWhiteSpace(userId)) { return BadRequest(); }
        var task = await _todoService.GetTaskAsync(userId, id);
        if (task == null)
        {
            return NotFound($"Todo task with id {id} not found");
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
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteTodoTask([FromHeader] string userId, string id)
    {
        if (string.IsNullOrWhiteSpace(userId)) { return BadRequest(); }
        try
        {
            await _todoService.DeleteTaskAsync(id, userId);
            return NoContent();
        }
        catch (TodoTaskNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPatch("{id}/completed")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> MarkTodoTaskCompleted([FromHeader] string userId, string id)
    {
        if (string.IsNullOrWhiteSpace(userId)) { return BadRequest(); }
        try
        {
            await _todoService.UpdateTaskAsCompletedAsync(id, userId);
            return NoContent();
        }
        catch (TodoTaskNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}

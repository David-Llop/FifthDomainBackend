using API.DataBase;
using API.DataBase.Entities;
using API.DTO.TodoTask;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class TodoTaskService
{
    private ApplicationDbContext _dbContext;
    private DbSet<TodoTask> _todoTasks;

    public TodoTaskService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _todoTasks = _dbContext.TodoTasks;
    }

    public async Task<List<TodoTask>> GetUserTasksAsync(string userId)
    {
        return await _todoTasks.AsNoTracking().Where(t => t.UserId == userId).ToListAsync();
    }

    public async Task<TodoTask?> GetTaskAsync(string userId, string taskId)
    {
        return await _todoTasks.AsNoTracking().FirstOrDefaultAsync(t => t.UserId == userId && t.Id == taskId);
    }

    public async Task<TodoTask> CreateTaskAsync(CreateTodoTaskDto taskDto, string userId)
    {
        return Ok(new TodoTaskDto())
    }
}

using API.DataBase;
using API.DataBase.Entities;
using API.DTO.TodoTask;
using API.Exceptions;
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
        return await _todoTasks.AsNoTracking().Where(t => t.UserId == userId).OrderBy(t => t.IsCompleted).ThenBy(t => t.Created).ToListAsync();
    }

    public async Task<TodoTask?> GetTaskAsync(string userId, string taskId)
    {
        return await _todoTasks.AsNoTracking().FirstOrDefaultAsync(t => t.UserId == userId && t.Id == taskId);
    }

    public async Task<TodoTask> CreateTaskAsync(CreateTodoTaskDto taskDto, string userId)
    {
        TodoTask newTask = new TodoTask(taskDto.Title, taskDto.Description, userId);
        _todoTasks.Add(newTask);
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new TodoTaskAlreadyExistsException($"Todo task with id {newTask.Id} already exists", ex);
        }
        return newTask;
    }
    public async Task DeleteTaskAsync(string taskId, string userId)
    {
        TodoTask? taskToDelete = await _todoTasks.FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);
        if (taskToDelete == null)
        {
            throw new TodoTaskNotFoundException($"Todo task with id {taskId} not found");
        }
        _todoTasks.Remove(taskToDelete);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateTaskAsCompletedAsync(string taskId, string userId)
    {
        TodoTask? taskToUpdate = await _todoTasks.FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);
        if (taskToUpdate == null)
        {
            throw new TodoTaskNotFoundException($"Todo task with id {taskId} not found");
        }
        taskToUpdate.IsCompleted = true;
        _todoTasks.Update(taskToUpdate);
        await _dbContext.SaveChangesAsync();
    }
}

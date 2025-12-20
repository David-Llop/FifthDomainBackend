namespace API.DTO.TodoTask;

public class TodoTaskDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Created { get; set; }
    public bool IsCompleted { get; set; }
}

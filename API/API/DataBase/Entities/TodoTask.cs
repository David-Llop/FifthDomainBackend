namespace API.DataBase.Entities;

public class TodoTask
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Created { get; set; }
    public bool IsCompleted { get; set; }

    public TodoTask(string title, string description, string userId)
    {
        Id = Guid.CreateVersion7().ToString();
        UserId = userId;
        Title = title;
        Description = description;
        Created = DateTime.Now;
        IsCompleted = false;
    }
}

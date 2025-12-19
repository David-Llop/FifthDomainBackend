using System.ComponentModel.DataAnnotations.Schema;

namespace API.DataBase.Entities;

public class User
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }

    public User(string Name, string Password)
    {
        Id = Guid.CreateVersion7().ToString();
        this.Name = Name;
        this.Password = Password;
    }
}

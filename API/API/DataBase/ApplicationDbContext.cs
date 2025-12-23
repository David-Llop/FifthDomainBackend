using API.DataBase.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.DataBase;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<TodoTask> TodoTasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(u => u.Id);
        modelBuilder.Entity<User>().HasIndex(u => u.Name).IsUnique();
        modelBuilder.Entity<TodoTask>().HasKey(t => t.Id);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging().UseSqlite("Data Source = demo.db").UseSeeding((context, _) =>
        {
            var user1 = context.Set<User>().FirstOrDefault(u => u.Name == "user1");
            if (user1 == null)
            {
                user1 = new User("user1", PasswordSha256("user1password"));
                context.Set<User>().Add(user1);
            }
            if (!context.Set<TodoTask>().Any(t => t.UserId == user1.Id))
            {
                context.Set<TodoTask>().Add(new TodoTask("Task 1", "Do something", user1.Id));
            }
            var user2 = context.Set<User>().FirstOrDefault(u => u.Name == "user2");
            if (user2 == null)
            {
                user2 = new User("user2", PasswordSha256("user2password"));
                context.Set<User>().Add(user2);
            }
            if (!context.Set<TodoTask>().Any(t => t.UserId == user2.Id))
            {
                context.Set<TodoTask>().Add(new TodoTask("Task 1", "Do something", user2.Id));
            }
            context.SaveChanges();
        });
    }

    private string PasswordSha256(string pasword)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(pasword);
        SHA256 hashstring = SHA256.Create();
        byte[] hash = hashstring.ComputeHash(bytes);
        StringBuilder hexString = new StringBuilder(hash.Length * 2);
        foreach (byte b in hash)
        {
            hexString.AppendFormat("{0:x2}", b);
        }

        return hexString.ToString();
    }
}

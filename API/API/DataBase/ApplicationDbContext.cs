using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Security.Cryptography;
using API.DataBase.Entities;

namespace API.DataBase;

public class ApplicationDbContext: DbContext
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging().UseSqlite("Data Source = demo.db").UseSeeding((context, _) =>
        {
            var user1 = context.Set<User>().FirstOrDefault(u => u.Name == "user1");
            if (user1 == null)
            {
                context.Set<User>().Add(new User("user1", PasswordSha256("user1password")));
            }
            var user2 = context.Set<User>().FirstOrDefault(u => u.Name == "user2");
            if (user2 == null)
            {
                context.Set<User>().Add(new User("user2", PasswordSha256("user2password")));
            }
            context.SaveChanges();
        }).UseAsyncSeeding(async (context, _, cancecllationToken) =>
        {
            var user1 = await context.Set<User>().FirstOrDefaultAsync(u => u.Name == "user1");
            if (user1 == null)
            {
                context.Set<User>().Add(new User("user1", PasswordSha256("user1password")));
            }
            var user2 = await context.Set<User>().FirstOrDefaultAsync(u => u.Name == "user2");
            if (user2 == null)
            {
                context.Set<User>().Add(new User("user2", PasswordSha256("user2password")));
            }
            await context.SaveChangesAsync(cancecllationToken);
        });
    }

    private string PasswordSha256(string pasword)
    {
        byte[] bytes = Encoding.Unicode.GetBytes(pasword);
        SHA256 hashstring = SHA256.Create();
        byte[] hash = hashstring.ComputeHash(bytes);
        string hashString = string.Empty;
        foreach (byte x in hash)
        {
            hashString += String.Format("{0:x2}", x);
        }
        return hashString;
    }
}

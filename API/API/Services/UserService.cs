using API.DataBase;
using API.DataBase.Entities;
using API.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class UserService
{
    private ApplicationDbContext _dbContext;
    private DbSet<User> _users;

    public UserService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _users = _dbContext.Users;
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        return await _users.AsNoTracking().ToListAsync();
    }

    public async Task<User?> GetUserAsync(string username)
    {
        return await _users.AsNoTracking().FirstOrDefaultAsync(u => u.Name == username);
    }

    public async Task<User> CreateUserAsync(string username, string password)
    {
        User newUser = new User(username, password);
        _users.Add(newUser);
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new UserAlreadyExistsException($"User with name {username} already exists", ex);
        }
        return newUser;
    }
}

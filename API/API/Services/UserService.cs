using API.DataBase;
using API.DataBase.Entities;
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
        return await _users.Where(u => u.Name == username).FirstOrDefaultAsync();
    }
}

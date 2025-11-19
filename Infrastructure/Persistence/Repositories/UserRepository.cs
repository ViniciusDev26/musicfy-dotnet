namespace Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Repositories;

public class UserRepository : IUserRepository
{
  private readonly AppDbContext _context;

  public UserRepository(AppDbContext context)
  {
    _context = context;
  }

  public async Task<User?> GetByIdAsync(int id)
  {
    return await _context.Users.FindAsync(id);
  }

  public async Task<User?> GetByEmailAsync(string email)
  {
    return await _context.Users
      .FirstOrDefaultAsync(u => u.Email == email);
  }

  public async Task<IEnumerable<User>> GetAllAsync()
  {
    return await _context.Users.ToListAsync();
  }

  public async Task<IEnumerable<User>> GetPaginatedAsync(int page, int pageSize)
  {
    return await _context.Users
      .Skip((page - 1) * pageSize)
      .Take(pageSize)
      .ToListAsync();
  }

  public async Task AddAsync(User user)
  {
    await _context.Users.AddAsync(user);
    await _context.SaveChangesAsync();
  }

  public async Task UpdateAsync(User user)
  {
    _context.Users.Update(user);
    await _context.SaveChangesAsync();
  }

  public async Task DeleteAsync(int id)
  {
    var user = await GetByIdAsync(id);
    if (user != null)
    {
      _context.Users.Remove(user);
      await _context.SaveChangesAsync();
    }
  }

  public async Task<bool> EmailExistsAsync(string email)
  {
    return await _context.Users
      .AnyAsync(u => u.Email == email);
  }

  public async Task<IEnumerable<User>> GetUsersByAgeRangeAsync(int minAge, int maxAge)
  {
    var today = DateTime.Now;
    var maxBirthDate = today.AddYears(-minAge);
    var minBirthDate = today.AddYears(-maxAge - 1);

    return await _context.Users
      .Where(u => u.BirthDate >= minBirthDate && u.BirthDate <= maxBirthDate)
      .ToListAsync();
  }

  public async Task<int> CountAsync()
  {
    return await _context.Users.CountAsync();
  }
}

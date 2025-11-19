namespace Domain.Repositories;

using Domain.Entities;

public interface IUserRepository
{
  // Operações básicas de CRUD
  Task<User?> GetByIdAsync(int id);
  Task<User?> GetByEmailAsync(string email);
  Task<IEnumerable<User>> GetAllAsync();
  Task<IEnumerable<User>> GetPaginatedAsync(int page, int pageSize);

  Task AddAsync(User user);
  Task UpdateAsync(User user);
  Task DeleteAsync(int id);

  // Operações específicas de negócio
  Task<bool> EmailExistsAsync(string email);
  Task<IEnumerable<User>> GetUsersByAgeRangeAsync(int minAge, int maxAge);
  Task<int> CountAsync();
}

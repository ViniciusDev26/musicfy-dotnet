namespace Domain.Repositories;

using Domain.Entities;

public interface IMusicRepository
{
  // Operações básicas de CRUD
  Task<Music?> GetByIdAsync(int id);
  Task<Music?> GetByNameAsync(string name);
  Task<IEnumerable<Music>> GetAllAsync();
  Task<IEnumerable<Music>> GetPaginatedAsync(int page, int pageSize);

  Task AddAsync(Music music);
  Task UpdateAsync(Music music);
  Task DeleteAsync(int id);

  // Operações específicas de negócio
  Task<bool> MusicExistsAsync(string name, string artist);
  Task<IEnumerable<Music>> GetByArtistAsync(string artist);
  Task<IEnumerable<Music>> SearchByNameAsync(string searchTerm);
  Task<int> CountAsync();
  Task<int> CountByArtistAsync(string artist);
}

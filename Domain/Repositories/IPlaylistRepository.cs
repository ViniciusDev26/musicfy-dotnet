namespace Domain.Repositories;

using Domain.Entities;

public interface IPlaylistRepository
{
  // Operações básicas de CRUD
  Task<Playlist?> GetByIdAsync(int id);
  Task<Playlist?> GetByNameAndUserIdAsync(string name, int userId);
  Task<IEnumerable<Playlist>> GetAllAsync();
  Task<IEnumerable<Playlist>> GetPaginatedAsync(int page, int pageSize);

  Task AddAsync(Playlist playlist);
  Task UpdateAsync(Playlist playlist);
  Task DeleteAsync(int id);

  // Operações específicas de negócio - Playlists de usuário
  Task<IEnumerable<Playlist>> GetByUserIdAsync(int userId);
  Task<IEnumerable<Playlist>> GetUserPlaylistsPaginatedAsync(int userId, int page, int pageSize);
  Task<bool> PlaylistExistsAsync(string name, int userId);
  Task<int> CountByUserIdAsync(int userId);

  // Operações específicas de negócio - Playlists do sistema
  Task<IEnumerable<Playlist>> GetSystemPlaylistsAsync();
  Task<IEnumerable<Playlist>> GetSystemPlaylistsPaginatedAsync(int page, int pageSize);
  Task<int> CountSystemPlaylistsAsync();

  // Operações gerais
  Task<int> CountAsync();
}

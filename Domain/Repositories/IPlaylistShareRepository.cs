namespace Domain.Repositories;

using Domain.Entities;

public interface IPlaylistShareRepository
{
  // Operações básicas de CRUD
  Task<PlaylistShare?> GetByIdAsync(int id);
  Task<IEnumerable<PlaylistShare>> GetAllAsync();
  Task<IEnumerable<PlaylistShare>> GetPaginatedAsync(int page, int pageSize);

  Task AddAsync(PlaylistShare share);
  Task UpdateAsync(PlaylistShare share);
  Task DeleteAsync(int id);

  // Operações específicas - Compartilhamentos de uma playlist
  Task<IEnumerable<PlaylistShare>> GetByPlaylistIdAsync(int playlistId);
  Task<IEnumerable<PlaylistShare>> GetActiveByPlaylistIdAsync(int playlistId);
  Task<int> CountByPlaylistIdAsync(int playlistId);

  // Operações específicas - Compartilhamentos com um usuário
  Task<IEnumerable<PlaylistShare>> GetSharedWithUserAsync(int userId);
  Task<IEnumerable<PlaylistShare>> GetActiveSharedWithUserAsync(int userId);
  Task<int> CountSharedWithUserAsync(int userId);

  // Operações específicas - Compartilhamentos feitos por um usuário
  Task<IEnumerable<PlaylistShare>> GetSharedByUserAsync(int ownerId);
  Task<IEnumerable<PlaylistShare>> GetActiveSharedByUserAsync(int ownerId);
  Task<int> CountSharedByUserAsync(int ownerId);

  // Verificações
  Task<bool> IsPlaylistSharedWithUserAsync(int playlistId, int userId);
  Task<PlaylistShare?> GetShareAsync(int playlistId, int userId);
  Task<bool> ShareExistsAsync(int playlistId, int ownerId, int sharedWithUserId);

  // Operações gerais
  Task<int> CountAsync();
  Task<int> CountActiveAsync();
}

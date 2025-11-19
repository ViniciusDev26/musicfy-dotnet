namespace Domain.Repositories;

using Domain.Entities;

public interface IPlaylistMusicRepository
{
  // Operações básicas de CRUD
  Task<PlaylistMusic?> GetByIdAsync(int id);
  Task<IEnumerable<PlaylistMusic>> GetAllAsync();

  Task AddAsync(PlaylistMusic playlistMusic);
  Task UpdateAsync(PlaylistMusic playlistMusic);
  Task DeleteAsync(int id);

  // Operações específicas - Músicas de uma playlist
  Task<IEnumerable<PlaylistMusic>> GetByPlaylistIdAsync(int playlistId);
  Task<IEnumerable<PlaylistMusic>> GetByPlaylistIdOrderedAsync(int playlistId);
  Task<int> CountByPlaylistIdAsync(int playlistId);

  // Operações específicas - Playlists que contêm uma música
  Task<IEnumerable<PlaylistMusic>> GetByMusicIdAsync(int musicId);
  Task<int> CountByMusicIdAsync(int musicId);

  // Verificações
  Task<bool> MusicExistsInPlaylistAsync(int playlistId, int musicId);
  Task<PlaylistMusic?> GetByPlaylistAndMusicAsync(int playlistId, int musicId);
  Task<int> GetNextOrderAsync(int playlistId);  // Próxima ordem disponível

  // Operações de reordenação
  Task<IEnumerable<PlaylistMusic>> GetByPlaylistIdInOrderRangeAsync(int playlistId, int startOrder, int endOrder);

  // Operações gerais
  Task<int> CountAsync();
}

namespace Application.UseCases.Playlist.ListPlaylistsByMusic;

using Domain.Repositories;
using Application.Exceptions;

public class ListPlaylistsByMusicUseCase
{
  private readonly IMusicRepository _musicRepository;
  private readonly IPlaylistMusicRepository _playlistMusicRepository;

  public ListPlaylistsByMusicUseCase(
    IMusicRepository musicRepository,
    IPlaylistMusicRepository playlistMusicRepository)
  {
    _musicRepository = musicRepository;
    _playlistMusicRepository = playlistMusicRepository;
  }

  public async Task<ListPlaylistsByMusicOutput> ExecuteAsync(ListPlaylistsByMusicInput input)
  {
    // Validar que a música existe
    var music = await _musicRepository.GetByIdAsync(input.MusicId);
    if (music == null)
    {
      throw new NotFoundException("Music", input.MusicId);
    }

    // Buscar todas as playlists que contêm esta música
    var playlistMusics = await _playlistMusicRepository.GetByMusicIdAsync(input.MusicId);

    // Aplicar filtros
    var filteredPlaylists = playlistMusics.AsEnumerable();

    // Filtro: apenas playlists de um usuário específico
    if (input.UserId.HasValue)
    {
      filteredPlaylists = filteredPlaylists.Where(pm =>
        pm.Playlist != null && pm.Playlist.UserId == input.UserId.Value);
    }

    // Filtro: incluir/excluir playlists do sistema
    if (input.IncludeSystemPlaylists.HasValue)
    {
      if (input.IncludeSystemPlaylists.Value)
      {
        // Incluir apenas playlists do sistema
        filteredPlaylists = filteredPlaylists.Where(pm =>
          pm.Playlist != null && pm.Playlist.IsSystemPlaylist());
      }
      else
      {
        // Excluir playlists do sistema
        filteredPlaylists = filteredPlaylists.Where(pm =>
          pm.Playlist != null && !pm.Playlist.IsSystemPlaylist());
      }
    }

    var playlistsList = filteredPlaylists.ToList();

    return new ListPlaylistsByMusicOutput
    {
      MusicId = music.Id,
      MusicName = music.Name,
      MusicArtist = music.Artist,
      Playlists = playlistsList.Select(pm => new PlaylistWithMusicDto
      {
        PlaylistId = pm.Playlist!.Id,
        PlaylistName = pm.Playlist.Name,
        UserId = pm.Playlist.UserId,
        IsSystemPlaylist = pm.Playlist.IsSystemPlaylist(),
        OrderInPlaylist = pm.Order,
        AddedAt = pm.AddedAt,
        OwnerName = pm.Playlist.User?.Name
      }).ToList(),
      TotalCount = playlistsList.Count
    };
  }
}

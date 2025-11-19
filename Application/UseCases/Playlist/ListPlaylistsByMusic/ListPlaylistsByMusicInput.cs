namespace Application.UseCases.Playlist.ListPlaylistsByMusic;

public class ListPlaylistsByMusicInput
{
  public int MusicId { get; set; }
  public int? UserId { get; set; }  // Filtro opcional: apenas playlists deste usuario
  public bool? IncludeSystemPlaylists { get; set; }  // Incluir playlists do sistema
}

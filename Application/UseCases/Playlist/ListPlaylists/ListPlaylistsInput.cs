namespace Application.UseCases.Playlist.ListPlaylists;

public class ListPlaylistsInput
{
  public int? Page { get; set; }
  public int? PageSize { get; set; }
  public int? UserId { get; set; }  // Filtro opcional: playlists de um usuário específico
  public bool? SystemOnly { get; set; }  // Filtro opcional: apenas playlists do sistema
  public bool? UserOnly { get; set; }  // Filtro opcional: apenas playlists de usuários (não do sistema)
}

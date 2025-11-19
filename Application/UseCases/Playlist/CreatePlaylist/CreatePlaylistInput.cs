namespace Application.UseCases.Playlist.CreatePlaylist;

public class CreatePlaylistInput
{
  public string Name { get; set; } = string.Empty;
  public int? UserId { get; set; }  // Null = sistema, valor = usuario
  public List<int> MusicIds { get; set; } = new();  // IDs das musicas para adicionar
}

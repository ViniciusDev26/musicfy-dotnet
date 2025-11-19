namespace Application.UseCases.Playlist.CreatePlaylist;

public class CreatePlaylistOutput
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public int? UserId { get; set; }
  public bool IsSystemPlaylist { get; set; }
  public DateTime CreatedAt { get; set; }
  public int TotalMusics { get; set; }
  public List<MusicInPlaylistDto> Musics { get; set; } = new();
}

public class MusicInPlaylistDto
{
  public int MusicId { get; set; }
  public string Name { get; set; } = string.Empty;
  public string Artist { get; set; } = string.Empty;
  public int Order { get; set; }
}

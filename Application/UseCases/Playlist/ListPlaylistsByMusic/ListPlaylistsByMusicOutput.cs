namespace Application.UseCases.Playlist.ListPlaylistsByMusic;

public class ListPlaylistsByMusicOutput
{
  public int MusicId { get; set; }
  public string MusicName { get; set; } = string.Empty;
  public string MusicArtist { get; set; } = string.Empty;
  public List<PlaylistWithMusicDto> Playlists { get; set; } = new();
  public int TotalCount { get; set; }
}

public class PlaylistWithMusicDto
{
  public int PlaylistId { get; set; }
  public string PlaylistName { get; set; } = string.Empty;
  public int? UserId { get; set; }
  public bool IsSystemPlaylist { get; set; }
  public int OrderInPlaylist { get; set; }
  public DateTime AddedAt { get; set; }
  public string? OwnerName { get; set; }
}

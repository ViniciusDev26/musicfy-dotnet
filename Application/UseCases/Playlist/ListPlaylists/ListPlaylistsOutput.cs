namespace Application.UseCases.Playlist.ListPlaylists;

public class ListPlaylistsOutput
{
  public List<PlaylistDto> Playlists { get; set; } = new();
  public int TotalCount { get; set; }
  public int? Page { get; set; }
  public int? PageSize { get; set; }
  public int? TotalPages { get; set; }
  public int? FilteredByUserId { get; set; }
  public bool? SystemOnly { get; set; }
  public bool? UserOnly { get; set; }
}

public class PlaylistDto
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public int? UserId { get; set; }
  public bool IsSystemPlaylist { get; set; }
  public DateTime CreatedAt { get; set; }
  public string? OwnerName { get; set; }  // Nome do dono (se não for do sistema)
}

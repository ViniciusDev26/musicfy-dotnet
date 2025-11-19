namespace Application.UseCases.Music.ListMusics;

public class ListMusicsOutput
{
  public List<MusicDto> Musics { get; set; } = new();
  public int TotalCount { get; set; }
  public int? Page { get; set; }
  public int? PageSize { get; set; }
  public int? TotalPages { get; set; }
  public string? FilteredByArtist { get; set; }
  public string? SearchedTerm { get; set; }
  public int? FilteredByPlaylistId { get; set; }
}

public class MusicDto
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string Artist { get; set; } = string.Empty;
  public string AudioUrl { get; set; } = string.Empty;
  public int? OrderInPlaylist { get; set; }  // Ordem na playlist (se filtrado por playlist)
  public DateTime? AddedToPlaylistAt { get; set; }  // Data de adição (se filtrado por playlist)
}

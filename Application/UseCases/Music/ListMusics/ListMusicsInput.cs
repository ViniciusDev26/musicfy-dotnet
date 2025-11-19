namespace Application.UseCases.Music.ListMusics;

public class ListMusicsInput
{
  public int? Page { get; set; }
  public int? PageSize { get; set; }
  public string? Artist { get; set; }  // Filtro opcional por artista
  public string? SearchTerm { get; set; }  // Busca opcional por termo no nome
  public int? PlaylistId { get; set; }  // Filtro opcional por playlist
}

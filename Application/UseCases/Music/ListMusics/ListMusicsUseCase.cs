namespace Application.UseCases.Music.ListMusics;

using Domain.Repositories;

public class ListMusicsUseCase
{
  private readonly IMusicRepository _musicRepository;
  private readonly IPlaylistMusicRepository _playlistMusicRepository;

  public ListMusicsUseCase(
    IMusicRepository musicRepository,
    IPlaylistMusicRepository playlistMusicRepository)
  {
    _musicRepository = musicRepository;
    _playlistMusicRepository = playlistMusicRepository;
  }

  public async Task<ListMusicsOutput> ExecuteAsync(ListMusicsInput input)
  {
    IEnumerable<Domain.Entities.Music> musics;
    int totalCount;

    // Filtro por playlist
    if (input.PlaylistId.HasValue)
    {
      var playlistMusics = await _playlistMusicRepository.GetByPlaylistIdOrderedAsync(input.PlaylistId.Value);
      totalCount = playlistMusics.Count();

      return new ListMusicsOutput
      {
        Musics = playlistMusics.Select(pm => new MusicDto
        {
          Id = pm.Music!.Id,
          Name = pm.Music.Name,
          Artist = pm.Music.Artist,
          AudioUrl = pm.Music.AudioUrl,
          OrderInPlaylist = pm.Order,
          AddedToPlaylistAt = pm.AddedAt
        }).ToList(),
        TotalCount = totalCount,
        FilteredByPlaylistId = input.PlaylistId.Value
      };
    }

    // Filtro por artista
    if (!string.IsNullOrWhiteSpace(input.Artist))
    {
      musics = await _musicRepository.GetByArtistAsync(input.Artist);
      totalCount = musics.Count();

      return BuildOutput(musics, totalCount, input, input.Artist, null);
    }

    // Busca por termo no nome
    if (!string.IsNullOrWhiteSpace(input.SearchTerm))
    {
      musics = await _musicRepository.SearchByNameAsync(input.SearchTerm);
      totalCount = musics.Count();

      return BuildOutput(musics, totalCount, input, null, input.SearchTerm);
    }

    // Se informou paginação
    if (input.Page.HasValue && input.PageSize.HasValue)
    {
      musics = await _musicRepository.GetPaginatedAsync(input.Page.Value, input.PageSize.Value);
      totalCount = await _musicRepository.CountAsync();

      return BuildOutputWithPagination(musics, totalCount, input);
    }

    // Caso contrário, retorna todos
    musics = await _musicRepository.GetAllAsync();
    totalCount = musics.Count();

    return BuildOutput(musics, totalCount, input, null, null);
  }

  private ListMusicsOutput BuildOutput(
    IEnumerable<Domain.Entities.Music> musics,
    int totalCount,
    ListMusicsInput input,
    string? filteredByArtist,
    string? searchedTerm)
  {
    return new ListMusicsOutput
    {
      Musics = musics.Select(m => new MusicDto
      {
        Id = m.Id,
        Name = m.Name,
        Artist = m.Artist,
        AudioUrl = m.AudioUrl
      }).ToList(),
      TotalCount = totalCount,
      FilteredByArtist = filteredByArtist,
      SearchedTerm = searchedTerm
    };
  }

  private ListMusicsOutput BuildOutputWithPagination(
    IEnumerable<Domain.Entities.Music> musics,
    int totalCount,
    ListMusicsInput input)
  {
    var totalPages = (int)Math.Ceiling((double)totalCount / input.PageSize!.Value);

    return new ListMusicsOutput
    {
      Musics = musics.Select(m => new MusicDto
      {
        Id = m.Id,
        Name = m.Name,
        Artist = m.Artist,
        AudioUrl = m.AudioUrl
      }).ToList(),
      TotalCount = totalCount,
      Page = input.Page!.Value,
      PageSize = input.PageSize.Value,
      TotalPages = totalPages
    };
  }
}

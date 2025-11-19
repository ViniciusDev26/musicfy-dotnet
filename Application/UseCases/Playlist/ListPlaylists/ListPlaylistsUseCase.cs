namespace Application.UseCases.Playlist.ListPlaylists;

using Domain.Repositories;

public class ListPlaylistsUseCase
{
  private readonly IPlaylistRepository _playlistRepository;

  public ListPlaylistsUseCase(IPlaylistRepository playlistRepository)
  {
    _playlistRepository = playlistRepository;
  }

  public async Task<ListPlaylistsOutput> ExecuteAsync(ListPlaylistsInput input)
  {
    IEnumerable<Domain.Entities.Playlist> playlists;
    int totalCount;

    // Filtro: apenas playlists do sistema
    if (input.SystemOnly == true)
    {
      if (input.Page.HasValue && input.PageSize.HasValue)
      {
        playlists = await _playlistRepository.GetSystemPlaylistsPaginatedAsync(
          input.Page.Value,
          input.PageSize.Value
        );
        totalCount = await _playlistRepository.CountSystemPlaylistsAsync();

        return BuildOutputWithPagination(playlists, totalCount, input, null, true, false);
      }

      playlists = await _playlistRepository.GetSystemPlaylistsAsync();
      totalCount = playlists.Count();

      return BuildOutput(playlists, totalCount, input, null, true, false);
    }

    // Filtro: playlists de um usuário específico
    if (input.UserId.HasValue)
    {
      if (input.Page.HasValue && input.PageSize.HasValue)
      {
        playlists = await _playlistRepository.GetUserPlaylistsPaginatedAsync(
          input.UserId.Value,
          input.Page.Value,
          input.PageSize.Value
        );
        totalCount = await _playlistRepository.CountByUserIdAsync(input.UserId.Value);

        return BuildOutputWithPagination(playlists, totalCount, input, input.UserId.Value, false, false);
      }

      playlists = await _playlistRepository.GetByUserIdAsync(input.UserId.Value);
      totalCount = playlists.Count();

      return BuildOutput(playlists, totalCount, input, input.UserId.Value, false, false);
    }

    // Sem filtros: todas as playlists
    if (input.Page.HasValue && input.PageSize.HasValue)
    {
      playlists = await _playlistRepository.GetPaginatedAsync(
        input.Page.Value,
        input.PageSize.Value
      );
      totalCount = await _playlistRepository.CountAsync();

      return BuildOutputWithPagination(playlists, totalCount, input, null, false, false);
    }

    // Todas as playlists sem paginação
    playlists = await _playlistRepository.GetAllAsync();
    totalCount = playlists.Count();

    return BuildOutput(playlists, totalCount, input, null, false, false);
  }

  private ListPlaylistsOutput BuildOutput(
    IEnumerable<Domain.Entities.Playlist> playlists,
    int totalCount,
    ListPlaylistsInput input,
    int? filteredByUserId,
    bool systemOnly,
    bool userOnly)
  {
    return new ListPlaylistsOutput
    {
      Playlists = playlists.Select(p => new PlaylistDto
      {
        Id = p.Id,
        Name = p.Name,
        UserId = p.UserId,
        IsSystemPlaylist = p.IsSystemPlaylist(),
        CreatedAt = p.CreatedAt,
        OwnerName = p.User?.Name  // Navigation property pode estar null
      }).ToList(),
      TotalCount = totalCount,
      FilteredByUserId = filteredByUserId,
      SystemOnly = systemOnly,
      UserOnly = userOnly
    };
  }

  private ListPlaylistsOutput BuildOutputWithPagination(
    IEnumerable<Domain.Entities.Playlist> playlists,
    int totalCount,
    ListPlaylistsInput input,
    int? filteredByUserId,
    bool systemOnly,
    bool userOnly)
  {
    var totalPages = (int)Math.Ceiling((double)totalCount / input.PageSize!.Value);

    return new ListPlaylistsOutput
    {
      Playlists = playlists.Select(p => new PlaylistDto
      {
        Id = p.Id,
        Name = p.Name,
        UserId = p.UserId,
        IsSystemPlaylist = p.IsSystemPlaylist(),
        CreatedAt = p.CreatedAt,
        OwnerName = p.User?.Name
      }).ToList(),
      TotalCount = totalCount,
      Page = input.Page!.Value,
      PageSize = input.PageSize.Value,
      TotalPages = totalPages,
      FilteredByUserId = filteredByUserId,
      SystemOnly = systemOnly,
      UserOnly = userOnly
    };
  }
}

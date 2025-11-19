using Api.Soap.Contracts;
using Api.Soap.Models;
using Application.UseCases.Playlist.CreatePlaylist;
using Application.UseCases.Playlist.ListPlaylists;
using Application.Exceptions;

namespace Api.Soap.Services;

public class PlaylistSoapService : IPlaylistSoapService
{
  private readonly CreatePlaylistUseCase _createPlaylistUseCase;
  private readonly ListPlaylistsUseCase _listPlaylistsUseCase;

  public PlaylistSoapService(
    CreatePlaylistUseCase createPlaylistUseCase,
    ListPlaylistsUseCase listPlaylistsUseCase)
  {
    _createPlaylistUseCase = createPlaylistUseCase;
    _listPlaylistsUseCase = listPlaylistsUseCase;
  }

  public async Task<GetPlaylistsResponse> GetPlaylists(GetPlaylistsRequest request)
  {
    var input = new ListPlaylistsInput
    {
      UserId = request.UserId,
      SystemOnly = request.SystemOnly,
      Page = request.Page,
      PageSize = request.PageSize
    };

    var result = await _listPlaylistsUseCase.ExecuteAsync(input);

    return new GetPlaylistsResponse
    {
      Playlists = result.Playlists.Select(p => new PlaylistSoapDto
      {
        Id = p.Id,
        Name = p.Name,
        UserId = p.UserId,
        IsSystemPlaylist = p.IsSystemPlaylist,
        CreatedAt = p.CreatedAt,
        OwnerName = p.OwnerName
      }).ToList(),
      TotalCount = result.TotalCount,
      Page = result.Page,
      PageSize = result.PageSize,
      TotalPages = result.TotalPages
    };
  }

  public async Task<PlaylistSoapDto> CreatePlaylist(CreatePlaylistRequest request)
  {
    try
    {
      var input = new CreatePlaylistInput
      {
        Name = request.Name,
        UserId = request.UserId,
        MusicIds = request.MusicIds
      };

      var result = await _createPlaylistUseCase.ExecuteAsync(input);

      return new PlaylistSoapDto
      {
        Id = result.Id,
        Name = result.Name,
        UserId = result.UserId,
        IsSystemPlaylist = result.IsSystemPlaylist,
        CreatedAt = result.CreatedAt
      };
    }
    catch (BusinessException ex)
    {
      throw new Exception($"Business Error: {ex.Message}");
    }
    catch (ArgumentException ex)
    {
      throw new Exception($"Validation Error: {ex.Message}");
    }
    catch (NotFoundException ex)
    {
      throw new Exception($"Not Found Error: {ex.Message}");
    }
  }
}

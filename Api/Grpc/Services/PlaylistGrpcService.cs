using Grpc.Core;
using Google.Protobuf.WellKnownTypes;
using Application.UseCases.Playlist.CreatePlaylist;
using Application.UseCases.Playlist.ListPlaylists;
using Application.Exceptions;

namespace Api.Grpc.Services;

public class PlaylistGrpcService : PlaylistService.PlaylistServiceBase
{
  private readonly CreatePlaylistUseCase _createPlaylistUseCase;
  private readonly ListPlaylistsUseCase _listPlaylistsUseCase;

  public PlaylistGrpcService(
    CreatePlaylistUseCase createPlaylistUseCase,
    ListPlaylistsUseCase listPlaylistsUseCase)
  {
    _createPlaylistUseCase = createPlaylistUseCase;
    _listPlaylistsUseCase = listPlaylistsUseCase;
  }

  public override async Task<GetPlaylistsResponse> GetPlaylists(
    GetPlaylistsRequest request,
    ServerCallContext context)
  {
    var input = new ListPlaylistsInput
    {
      UserId = request.HasUserId ? request.UserId : null,
      SystemOnly = request.HasSystemOnly ? request.SystemOnly : null,
      Page = request.HasPage ? request.Page : null,
      PageSize = request.HasPageSize ? request.PageSize : null
    };

    var result = await _listPlaylistsUseCase.ExecuteAsync(input);

    var response = new GetPlaylistsResponse
    {
      TotalCount = result.TotalCount
    };

    foreach (var playlist in result.Playlists)
    {
      var playlistResponse = new PlaylistResponse
      {
        Id = playlist.Id,
        Name = playlist.Name,
        IsSystemPlaylist = playlist.IsSystemPlaylist,
        CreatedAt = Timestamp.FromDateTime(DateTime.SpecifyKind(playlist.CreatedAt, DateTimeKind.Utc))
      };

      if (playlist.UserId.HasValue)
        playlistResponse.UserId = playlist.UserId.Value;

      if (!string.IsNullOrEmpty(playlist.OwnerName))
        playlistResponse.OwnerName = playlist.OwnerName;

      response.Playlists.Add(playlistResponse);
    }

    if (result.Page.HasValue)
      response.Page = result.Page.Value;

    if (result.PageSize.HasValue)
      response.PageSize = result.PageSize.Value;

    if (result.TotalPages.HasValue)
      response.TotalPages = result.TotalPages.Value;

    return response;
  }

  public override async Task<PlaylistResponse> CreatePlaylist(
    CreatePlaylistRequest request,
    ServerCallContext context)
  {
    try
    {
      var input = new CreatePlaylistInput
      {
        Name = request.Name,
        UserId = request.HasUserId ? request.UserId : null,
        MusicIds = request.MusicIds.ToList()
      };

      var result = await _createPlaylistUseCase.ExecuteAsync(input);

      var response = new PlaylistResponse
      {
        Id = result.Id,
        Name = result.Name,
        IsSystemPlaylist = result.IsSystemPlaylist,
        CreatedAt = Timestamp.FromDateTime(DateTime.SpecifyKind(result.CreatedAt, DateTimeKind.Utc))
      };

      if (result.UserId.HasValue)
        response.UserId = result.UserId.Value;

      return response;
    }
    catch (BusinessException ex)
    {
      throw new RpcException(new Status(StatusCode.InvalidArgument, ex.Message));
    }
    catch (ArgumentException ex)
    {
      throw new RpcException(new Status(StatusCode.InvalidArgument, ex.Message));
    }
    catch (NotFoundException ex)
    {
      throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
    }
  }
}

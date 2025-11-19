using Grpc.Core;
using Application.UseCases.Music.CreateMusic;
using Application.UseCases.Music.ListMusics;
using Application.Exceptions;

namespace Api.Grpc.Services;

public class MusicGrpcService : MusicService.MusicServiceBase
{
  private readonly CreateMusicUseCase _createMusicUseCase;
  private readonly ListMusicsUseCase _listMusicsUseCase;

  public MusicGrpcService(
    CreateMusicUseCase createMusicUseCase,
    ListMusicsUseCase listMusicsUseCase)
  {
    _createMusicUseCase = createMusicUseCase;
    _listMusicsUseCase = listMusicsUseCase;
  }

  public override async Task<GetMusicsResponse> GetMusics(
    GetMusicsRequest request,
    ServerCallContext context)
  {
    var input = new ListMusicsInput
    {
      PlaylistId = request.HasPlaylistId ? request.PlaylistId : null,
      Artist = request.HasArtist ? request.Artist : null,
      SearchTerm = request.HasSearchTerm ? request.SearchTerm : null,
      Page = request.HasPage ? request.Page : null,
      PageSize = request.HasPageSize ? request.PageSize : null
    };

    var result = await _listMusicsUseCase.ExecuteAsync(input);

    var response = new GetMusicsResponse
    {
      TotalCount = result.TotalCount
    };

    foreach (var music in result.Musics)
    {
      response.Musics.Add(new MusicResponse
      {
        Id = music.Id,
        Name = music.Name,
        Artist = music.Artist,
        AudioUrl = music.AudioUrl
      });
    }

    if (result.Page.HasValue)
      response.Page = result.Page.Value;

    if (result.PageSize.HasValue)
      response.PageSize = result.PageSize.Value;

    if (result.TotalPages.HasValue)
      response.TotalPages = result.TotalPages.Value;

    return response;
  }

  public override async Task<MusicResponse> CreateMusic(
    CreateMusicRequest request,
    ServerCallContext context)
  {
    try
    {
      var input = new CreateMusicInput
      {
        Name = request.Name,
        Artist = request.Artist,
        AudioUrl = request.AudioUrl
      };

      var result = await _createMusicUseCase.ExecuteAsync(input);

      return new MusicResponse
      {
        Id = result.Id,
        Name = result.Name,
        Artist = result.Artist,
        AudioUrl = result.AudioUrl
      };
    }
    catch (BusinessException ex)
    {
      throw new RpcException(new Status(StatusCode.InvalidArgument, ex.Message));
    }
    catch (ArgumentException ex)
    {
      throw new RpcException(new Status(StatusCode.InvalidArgument, ex.Message));
    }
  }
}

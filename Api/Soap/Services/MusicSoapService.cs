using Api.Soap.Contracts;
using Api.Soap.Models;
using Application.UseCases.Music.CreateMusic;
using Application.UseCases.Music.ListMusics;
using Application.Exceptions;

namespace Api.Soap.Services;

public class MusicSoapService : IMusicSoapService
{
  private readonly CreateMusicUseCase _createMusicUseCase;
  private readonly ListMusicsUseCase _listMusicsUseCase;

  public MusicSoapService(
    CreateMusicUseCase createMusicUseCase,
    ListMusicsUseCase listMusicsUseCase)
  {
    _createMusicUseCase = createMusicUseCase;
    _listMusicsUseCase = listMusicsUseCase;
  }

  public async Task<GetMusicsResponse> GetMusics(GetMusicsRequest request)
  {
    var input = new ListMusicsInput
    {
      PlaylistId = request.PlaylistId,
      Artist = request.Artist,
      SearchTerm = request.SearchTerm,
      Page = request.Page,
      PageSize = request.PageSize
    };

    var result = await _listMusicsUseCase.ExecuteAsync(input);

    return new GetMusicsResponse
    {
      Musics = result.Musics.Select(m => new MusicSoapDto
      {
        Id = m.Id,
        Name = m.Name,
        Artist = m.Artist,
        AudioUrl = m.AudioUrl
      }).ToList(),
      TotalCount = result.TotalCount,
      Page = result.Page,
      PageSize = result.PageSize,
      TotalPages = result.TotalPages
    };
  }

  public async Task<MusicSoapDto> CreateMusic(CreateMusicRequest request)
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

      return new MusicSoapDto
      {
        Id = result.Id,
        Name = result.Name,
        Artist = result.Artist,
        AudioUrl = result.AudioUrl
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
  }
}

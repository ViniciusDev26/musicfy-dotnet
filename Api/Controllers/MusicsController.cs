using Microsoft.AspNetCore.Mvc;
using Application.UseCases.Music.CreateMusic;
using Application.UseCases.Music.ListMusics;
using Application.Exceptions;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MusicsController : ControllerBase
{
  private readonly CreateMusicUseCase _createMusicUseCase;
  private readonly ListMusicsUseCase _listMusicsUseCase;

  public MusicsController(
    CreateMusicUseCase createMusicUseCase,
    ListMusicsUseCase listMusicsUseCase)
  {
    _createMusicUseCase = createMusicUseCase;
    _listMusicsUseCase = listMusicsUseCase;
  }

  /// <summary>
  /// Get all musics with optional filters and pagination
  /// </summary>
  /// <param name="playlistId">Filter by playlist ID (optional)</param>
  /// <param name="artist">Filter by artist name (optional)</param>
  /// <param name="searchTerm">Search term for music name (optional)</param>
  /// <param name="page">Page number (optional)</param>
  /// <param name="pageSize">Page size (optional)</param>
  /// <returns>List of musics</returns>
  [HttpGet]
  [ProducesResponseType(typeof(ListMusicsOutput), StatusCodes.Status200OK)]
  public async Task<ActionResult<ListMusicsOutput>> GetMusics(
    [FromQuery] int? playlistId = null,
    [FromQuery] string? artist = null,
    [FromQuery] string? searchTerm = null,
    [FromQuery] int? page = null,
    [FromQuery] int? pageSize = null)
  {
    var input = new ListMusicsInput
    {
      PlaylistId = playlistId,
      Artist = artist,
      SearchTerm = searchTerm,
      Page = page,
      PageSize = pageSize
    };

    var result = await _listMusicsUseCase.ExecuteAsync(input);
    return Ok(result);
  }

  /// <summary>
  /// Create a new music
  /// </summary>
  /// <param name="input">Music data</param>
  /// <returns>Created music</returns>
  [HttpPost]
  [ProducesResponseType(typeof(CreateMusicOutput), StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<ActionResult<CreateMusicOutput>> CreateMusic([FromBody] CreateMusicInput input)
  {
    try
    {
      var result = await _createMusicUseCase.ExecuteAsync(input);
      return CreatedAtAction(
        nameof(GetMusics),
        new { id = result.Id },
        result);
    }
    catch (BusinessException ex)
    {
      return BadRequest(new { error = ex.Message });
    }
    catch (ArgumentException ex)
    {
      return BadRequest(new { error = ex.Message });
    }
  }
}

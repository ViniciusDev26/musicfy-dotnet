using Microsoft.AspNetCore.Mvc;
using Application.UseCases.Playlist.CreatePlaylist;
using Application.UseCases.Playlist.ListPlaylists;
using Application.UseCases.Playlist.ListPlaylistsByMusic;
using Application.Exceptions;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlaylistsController : ControllerBase
{
  private readonly CreatePlaylistUseCase _createPlaylistUseCase;
  private readonly ListPlaylistsUseCase _listPlaylistsUseCase;
  private readonly ListPlaylistsByMusicUseCase _listPlaylistsByMusicUseCase;

  public PlaylistsController(
    CreatePlaylistUseCase createPlaylistUseCase,
    ListPlaylistsUseCase listPlaylistsUseCase,
    ListPlaylistsByMusicUseCase listPlaylistsByMusicUseCase)
  {
    _createPlaylistUseCase = createPlaylistUseCase;
    _listPlaylistsUseCase = listPlaylistsUseCase;
    _listPlaylistsByMusicUseCase = listPlaylistsByMusicUseCase;
  }

  /// <summary>
  /// Get all playlists with optional filters and pagination
  /// </summary>
  /// <param name="userId">Filter by user ID (optional)</param>
  /// <param name="systemOnly">Show only system playlists (optional)</param>
  /// <param name="page">Page number (optional)</param>
  /// <param name="pageSize">Page size (optional)</param>
  /// <returns>List of playlists</returns>
  [HttpGet]
  [ProducesResponseType(typeof(ListPlaylistsOutput), StatusCodes.Status200OK)]
  public async Task<ActionResult<ListPlaylistsOutput>> GetPlaylists(
    [FromQuery] int? userId = null,
    [FromQuery] bool? systemOnly = null,
    [FromQuery] int? page = null,
    [FromQuery] int? pageSize = null)
  {
    var input = new ListPlaylistsInput
    {
      UserId = userId,
      SystemOnly = systemOnly,
      Page = page,
      PageSize = pageSize
    };

    var result = await _listPlaylistsUseCase.ExecuteAsync(input);
    return Ok(result);
  }

  /// <summary>
  /// Get all playlists that contain a specific music
  /// </summary>
  /// <param name="musicId">Music ID</param>
  /// <returns>List of playlists containing the music</returns>
  [HttpGet("by-music/{musicId}")]
  [ProducesResponseType(typeof(ListPlaylistsByMusicOutput), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<ListPlaylistsByMusicOutput>> GetPlaylistsByMusic(int musicId)
  {
    try
    {
      var input = new ListPlaylistsByMusicInput { MusicId = musicId };
      var result = await _listPlaylistsByMusicUseCase.ExecuteAsync(input);
      return Ok(result);
    }
    catch (NotFoundException ex)
    {
      return NotFound(new { error = ex.Message });
    }
  }

  /// <summary>
  /// Create a new playlist (user or system)
  /// </summary>
  /// <param name="input">Playlist data</param>
  /// <returns>Created playlist</returns>
  [HttpPost]
  [ProducesResponseType(typeof(CreatePlaylistOutput), StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<CreatePlaylistOutput>> CreatePlaylist([FromBody] CreatePlaylistInput input)
  {
    try
    {
      var result = await _createPlaylistUseCase.ExecuteAsync(input);
      return CreatedAtAction(
        nameof(GetPlaylists),
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
    catch (NotFoundException ex)
    {
      return NotFound(new { error = ex.Message });
    }
  }
}

using Microsoft.AspNetCore.Mvc;
using Application.UseCases.User.CreateUser;
using Application.UseCases.User.ListUsers;
using Application.Exceptions;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
  private readonly CreateUserUseCase _createUserUseCase;
  private readonly ListUsersUseCase _listUsersUseCase;

  public UsersController(
    CreateUserUseCase createUserUseCase,
    ListUsersUseCase listUsersUseCase)
  {
    _createUserUseCase = createUserUseCase;
    _listUsersUseCase = listUsersUseCase;
  }

  /// <summary>
  /// Get all users with optional pagination
  /// </summary>
  /// <param name="page">Page number (optional)</param>
  /// <param name="pageSize">Page size (optional)</param>
  /// <returns>List of users</returns>
  [HttpGet]
  [ProducesResponseType(typeof(ListUsersOutput), StatusCodes.Status200OK)]
  public async Task<ActionResult<ListUsersOutput>> GetUsers(
    [FromQuery] int? page = null,
    [FromQuery] int? pageSize = null)
  {
    var input = new ListUsersInput
    {
      Page = page,
      PageSize = pageSize
    };

    var result = await _listUsersUseCase.ExecuteAsync(input);
    return Ok(result);
  }

  /// <summary>
  /// Create a new user
  /// </summary>
  /// <param name="input">User data</param>
  /// <returns>Created user</returns>
  [HttpPost]
  [ProducesResponseType(typeof(CreateUserOutput), StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<ActionResult<CreateUserOutput>> CreateUser([FromBody] CreateUserInput input)
  {
    try
    {
      var result = await _createUserUseCase.ExecuteAsync(input);
      return CreatedAtAction(
        nameof(GetUsers),
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

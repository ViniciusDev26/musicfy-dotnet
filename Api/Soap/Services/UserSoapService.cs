using Api.Soap.Contracts;
using Api.Soap.Models;
using Application.UseCases.User.CreateUser;
using Application.UseCases.User.ListUsers;
using Application.Exceptions;

namespace Api.Soap.Services;

public class UserSoapService : IUserSoapService
{
  private readonly CreateUserUseCase _createUserUseCase;
  private readonly ListUsersUseCase _listUsersUseCase;

  public UserSoapService(
    CreateUserUseCase createUserUseCase,
    ListUsersUseCase listUsersUseCase)
  {
    _createUserUseCase = createUserUseCase;
    _listUsersUseCase = listUsersUseCase;
  }

  public async Task<GetUsersResponse> GetUsers(GetUsersRequest request)
  {
    var input = new ListUsersInput
    {
      Page = request.Page,
      PageSize = request.PageSize
    };

    var result = await _listUsersUseCase.ExecuteAsync(input);

    return new GetUsersResponse
    {
      Users = result.Users.Select(u => new UserSoapDto
      {
        Id = u.Id,
        Name = u.Name,
        BirthDate = u.BirthDate,
        Email = u.Email,
        Age = u.Age
      }).ToList(),
      TotalCount = result.TotalCount,
      Page = result.Page,
      PageSize = result.PageSize,
      TotalPages = result.TotalPages
    };
  }

  public async Task<UserSoapDto> CreateUser(CreateUserRequest request)
  {
    try
    {
      var input = new CreateUserInput
      {
        Name = request.Name,
        BirthDate = request.BirthDate,
        Email = request.Email
      };

      var result = await _createUserUseCase.ExecuteAsync(input);

      return new UserSoapDto
      {
        Id = result.Id,
        Name = result.Name,
        BirthDate = result.BirthDate,
        Email = result.Email,
        Age = result.Age
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

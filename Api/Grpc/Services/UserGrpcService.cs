using Grpc.Core;
using Google.Protobuf.WellKnownTypes;
using Application.UseCases.User.CreateUser;
using Application.UseCases.User.ListUsers;
using Application.Exceptions;

namespace Api.Grpc.Services;

public class UserGrpcService : UserService.UserServiceBase
{
  private readonly CreateUserUseCase _createUserUseCase;
  private readonly ListUsersUseCase _listUsersUseCase;

  public UserGrpcService(
    CreateUserUseCase createUserUseCase,
    ListUsersUseCase listUsersUseCase)
  {
    _createUserUseCase = createUserUseCase;
    _listUsersUseCase = listUsersUseCase;
  }

  public override async Task<GetUsersResponse> GetUsers(
    GetUsersRequest request,
    ServerCallContext context)
  {
    var input = new ListUsersInput
    {
      Page = request.HasPage ? request.Page : null,
      PageSize = request.HasPageSize ? request.PageSize : null
    };

    var result = await _listUsersUseCase.ExecuteAsync(input);

    var response = new GetUsersResponse
    {
      TotalCount = result.TotalCount
    };

    foreach (var user in result.Users)
    {
      response.Users.Add(new UserResponse
      {
        Id = user.Id,
        Name = user.Name,
        BirthDate = Timestamp.FromDateTime(DateTime.SpecifyKind(user.BirthDate, DateTimeKind.Utc)),
        Email = user.Email,
        Age = user.Age
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

  public override async Task<UserResponse> CreateUser(
    CreateUserRequest request,
    ServerCallContext context)
  {
    try
    {
      var input = new CreateUserInput
      {
        Name = request.Name,
        BirthDate = request.BirthDate.ToDateTime(),
        Email = request.Email
      };

      var result = await _createUserUseCase.ExecuteAsync(input);

      return new UserResponse
      {
        Id = result.Id,
        Name = result.Name,
        BirthDate = Timestamp.FromDateTime(DateTime.SpecifyKind(result.BirthDate, DateTimeKind.Utc)),
        Email = result.Email,
        Age = result.Age
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

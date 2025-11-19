using HotChocolate.Types;
using Application.UseCases.User.ListUsers;

namespace Api.GraphQL.Queries;

[ExtendObjectType("Query")]
public class UserQueries
{
  public async Task<ListUsersOutput> Users(
    ListUsersInput input,
    [Service] ListUsersUseCase useCase)
  {
    return await useCase.ExecuteAsync(input);
  }
}

using HotChocolate.Types;
using Application.UseCases.User.CreateUser;

namespace Api.GraphQL.Mutations;

[ExtendObjectType("Mutation")]
public class UserMutations
{
  public async Task<CreateUserOutput> CreateUser(
    CreateUserInput input,
    [Service] CreateUserUseCase useCase)
  {
    return await useCase.ExecuteAsync(input);
  }
}

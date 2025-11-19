using HotChocolate.Types;
using Application.UseCases.Music.CreateMusic;

namespace Api.GraphQL.Mutations;

[ExtendObjectType("Mutation")]
public class MusicMutations
{
  public async Task<CreateMusicOutput> CreateMusic(
    CreateMusicInput input,
    [Service] CreateMusicUseCase useCase)
  {
    return await useCase.ExecuteAsync(input);
  }
}

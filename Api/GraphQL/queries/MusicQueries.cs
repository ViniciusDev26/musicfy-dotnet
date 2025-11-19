using HotChocolate.Types;
using Application.UseCases.Music.ListMusics;

namespace Api.GraphQL.Queries;

[ExtendObjectType("Query")]
public class MusicQueries
{
  public async Task<ListMusicsOutput> Musics(
    ListMusicsInput input,
    [Service] ListMusicsUseCase useCase)
  {
    return await useCase.ExecuteAsync(input);
  }
}
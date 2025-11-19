using HotChocolate.Types;
using Application.UseCases.Playlist.CreatePlaylist;

namespace Api.GraphQL.Mutations;

[ExtendObjectType("Mutation")]
public class PlaylistMutations
{
  public async Task<CreatePlaylistOutput> CreatePlaylist(
    CreatePlaylistInput input,
    [Service] CreatePlaylistUseCase useCase)
  {
    return await useCase.ExecuteAsync(input);
  }
}

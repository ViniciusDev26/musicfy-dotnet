using HotChocolate.Types;
using Application.UseCases.Playlist.ListPlaylists;
using Application.UseCases.Playlist.ListPlaylistsByMusic;

namespace Api.GraphQL.Queries;

[ExtendObjectType("Query")]
public class PlaylistQueries
{
  public async Task<ListPlaylistsOutput> Playlists(
    ListPlaylistsInput input,
    [Service] ListPlaylistsUseCase useCase)
  {
    return await useCase.ExecuteAsync(input);
  }

  public async Task<ListPlaylistsByMusicOutput> PlaylistsByMusic(
    ListPlaylistsByMusicInput input,
    [Service] ListPlaylistsByMusicUseCase useCase)
  {
    return await useCase.ExecuteAsync(input);
  }
}

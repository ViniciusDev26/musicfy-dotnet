namespace Application;

using Microsoft.Extensions.DependencyInjection;
using Application.UseCases.User.CreateUser;
using Application.UseCases.User.ListUsers;
using Application.UseCases.Music.CreateMusic;
using Application.UseCases.Music.ListMusics;
using Application.UseCases.Playlist.CreatePlaylist;
using Application.UseCases.Playlist.ListPlaylists;
using Application.UseCases.Playlist.ListPlaylistsByMusic;

public static class DependencyInjection
{
  public static IServiceCollection AddApplication(this IServiceCollection services)
  {
    // User use cases
    services.AddScoped<CreateUserUseCase>();
    services.AddScoped<ListUsersUseCase>();

    // Music use cases
    services.AddScoped<CreateMusicUseCase>();
    services.AddScoped<ListMusicsUseCase>();

    // Playlist use cases
    services.AddScoped<CreatePlaylistUseCase>();
    services.AddScoped<ListPlaylistsUseCase>();
    services.AddScoped<ListPlaylistsByMusicUseCase>();

    return services;
  }
}

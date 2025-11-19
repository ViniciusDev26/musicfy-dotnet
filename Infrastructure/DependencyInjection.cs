namespace Infrastructure;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Domain.Repositories;

public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructure(
    this IServiceCollection services,
    string connectionString)
  {
    // DbContext
    services.AddDbContext<AppDbContext>(options =>
      options.UseSqlite(connectionString));

    // Repositórios
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IMusicRepository, MusicRepository>();
    services.AddScoped<IPlaylistRepository, PlaylistRepository>();
    services.AddScoped<IPlaylistMusicRepository, PlaylistMusicRepository>();
    services.AddScoped<IPlaylistShareRepository, PlaylistShareRepository>();

    return services;
  }
}

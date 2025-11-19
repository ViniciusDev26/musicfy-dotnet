namespace Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Domain.Entities;

public class AppDbContext : DbContext
{
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
  {
  }

  // DbSets
  public DbSet<User> Users => Set<User>();
  public DbSet<Music> Musics => Set<Music>();
  public DbSet<Playlist> Playlists => Set<Playlist>();
  public DbSet<PlaylistMusic> PlaylistMusics => Set<PlaylistMusic>();
  public DbSet<PlaylistShare> PlaylistShares => Set<PlaylistShare>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    // Aplicar todas as configurações do assembly
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
  }
}

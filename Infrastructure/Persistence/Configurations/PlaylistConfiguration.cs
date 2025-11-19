namespace Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

public class PlaylistConfiguration : IEntityTypeConfiguration<Playlist>
{
  public void Configure(EntityTypeBuilder<Playlist> builder)
  {
    builder.ToTable("Playlists");

    builder.HasKey(p => p.Id);

    builder.Property(p => p.Name)
      .IsRequired()
      .HasMaxLength(100);

    builder.Property(p => p.UserId)
      .IsRequired(false); // Nullable para playlists do sistema

    builder.Property(p => p.CreatedAt)
      .IsRequired();

    // Relacionamento com User (opcional)
    builder.HasOne(p => p.User)
      .WithMany()
      .HasForeignKey(p => p.UserId)
      .OnDelete(DeleteBehavior.Cascade);

    builder.HasIndex(p => new { p.Name, p.UserId });
  }
}

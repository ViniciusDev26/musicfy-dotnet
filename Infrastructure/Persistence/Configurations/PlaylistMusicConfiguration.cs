namespace Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

public class PlaylistMusicConfiguration : IEntityTypeConfiguration<PlaylistMusic>
{
  public void Configure(EntityTypeBuilder<PlaylistMusic> builder)
  {
    builder.ToTable("PlaylistMusics");

    builder.HasKey(pm => pm.Id);

    builder.Property(pm => pm.PlaylistId)
      .IsRequired();

    builder.Property(pm => pm.MusicId)
      .IsRequired();

    builder.Property(pm => pm.Order)
      .IsRequired();

    builder.Property(pm => pm.AddedAt)
      .IsRequired();

    builder.Property(pm => pm.AddedByUserId)
      .IsRequired(false); // Nullable

    // Relacionamento com Playlist
    builder.HasOne(pm => pm.Playlist)
      .WithMany()
      .HasForeignKey(pm => pm.PlaylistId)
      .OnDelete(DeleteBehavior.Cascade);

    // Relacionamento com Music
    builder.HasOne(pm => pm.Music)
      .WithMany()
      .HasForeignKey(pm => pm.MusicId)
      .OnDelete(DeleteBehavior.Cascade);

    // Relacionamento com User (quem adicionou)
    builder.HasOne(pm => pm.AddedByUser)
      .WithMany()
      .HasForeignKey(pm => pm.AddedByUserId)
      .OnDelete(DeleteBehavior.SetNull);

    // Índice único: uma música não pode aparecer duas vezes na mesma playlist
    builder.HasIndex(pm => new { pm.PlaylistId, pm.MusicId })
      .IsUnique();

    // Índice para ordenação
    builder.HasIndex(pm => new { pm.PlaylistId, pm.Order });
  }
}

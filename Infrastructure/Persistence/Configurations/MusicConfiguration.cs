namespace Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

public class MusicConfiguration : IEntityTypeConfiguration<Music>
{
  public void Configure(EntityTypeBuilder<Music> builder)
  {
    builder.ToTable("Musics");

    builder.HasKey(m => m.Id);

    builder.Property(m => m.Name)
      .IsRequired()
      .HasMaxLength(200);

    builder.Property(m => m.Artist)
      .IsRequired()
      .HasMaxLength(200);

    builder.Property(m => m.AudioUrl)
      .IsRequired()
      .HasMaxLength(500);

    builder.HasIndex(m => new { m.Name, m.Artist });
  }
}

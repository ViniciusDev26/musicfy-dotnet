namespace Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

public class PlaylistShareConfiguration : IEntityTypeConfiguration<PlaylistShare>
{
  public void Configure(EntityTypeBuilder<PlaylistShare> builder)
  {
    builder.ToTable("PlaylistShares");

    builder.HasKey(ps => ps.Id);

    builder.Property(ps => ps.PlaylistId)
      .IsRequired();

    builder.Property(ps => ps.OwnerId)
      .IsRequired();

    builder.Property(ps => ps.SharedWithUserId)
      .IsRequired();

    builder.Property(ps => ps.Permission)
      .IsRequired()
      .HasConversion<int>(); // Salva enum como int

    builder.Property(ps => ps.SharedAt)
      .IsRequired();

    builder.Property(ps => ps.IsActive)
      .IsRequired();

    // Relacionamento com Playlist
    builder.HasOne(ps => ps.Playlist)
      .WithMany()
      .HasForeignKey(ps => ps.PlaylistId)
      .OnDelete(DeleteBehavior.Cascade);

    // Relacionamento com Owner (User)
    builder.HasOne(ps => ps.Owner)
      .WithMany()
      .HasForeignKey(ps => ps.OwnerId)
      .OnDelete(DeleteBehavior.Restrict); // Não deletar shares se owner for deletado

    // Relacionamento com SharedWithUser (User)
    builder.HasOne(ps => ps.SharedWithUser)
      .WithMany()
      .HasForeignKey(ps => ps.SharedWithUserId)
      .OnDelete(DeleteBehavior.Cascade);

    // Índice único: mesma playlist não pode ser compartilhada duas vezes com o mesmo usuário
    builder.HasIndex(ps => new { ps.PlaylistId, ps.OwnerId, ps.SharedWithUserId })
      .IsUnique();
  }
}

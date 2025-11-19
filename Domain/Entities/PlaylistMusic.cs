namespace Domain.Entities;

public class PlaylistMusic
{
  public int Id { get; private set; }
  public int PlaylistId { get; private set; }
  public int MusicId { get; private set; }
  public int Order { get; private set; }  // Ordem da música na playlist (permite reordenar)
  public DateTime AddedAt { get; private set; }
  public int? AddedByUserId { get; private set; }  // Quem adicionou (útil em playlists compartilhadas)

  // Navigation properties (para o EF Core carregar relacionamentos)
  public Playlist? Playlist { get; private set; }
  public Music? Music { get; private set; }
  public User? AddedByUser { get; private set; }

  // Construtor vazio para o EF Core
  private PlaylistMusic() { }

  // Construtor para adicionar música à playlist
  public PlaylistMusic(int playlistId, int musicId, int order, int? addedByUserId = null)
  {
    ValidatePlaylistId(playlistId);
    ValidateMusicId(musicId);
    ValidateOrder(order);

    PlaylistId = playlistId;
    MusicId = musicId;
    Order = order;
    AddedAt = DateTime.UtcNow;
    AddedByUserId = addedByUserId;
  }

  // Métodos de negócio
  public void ChangeOrder(int newOrder)
  {
    ValidateOrder(newOrder);
    Order = newOrder;
  }

  public bool WasAddedBy(int userId)
  {
    return AddedByUserId.HasValue && AddedByUserId.Value == userId;
  }

  // Validações
  private void ValidatePlaylistId(int playlistId)
  {
    if (playlistId <= 0)
      throw new ArgumentException("Playlist ID must be greater than 0");
  }

  private void ValidateMusicId(int musicId)
  {
    if (musicId <= 0)
      throw new ArgumentException("Music ID must be greater than 0");
  }

  private void ValidateOrder(int order)
  {
    if (order < 0)
      throw new ArgumentException("Order must be greater than or equal to 0");
  }
}

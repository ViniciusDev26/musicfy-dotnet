namespace Domain.Entities;

using Domain.Enums;

public class PlaylistShare
{
  public int Id { get; private set; }
  public int PlaylistId { get; private set; }
  public int OwnerId { get; private set; } // Quem compartilhou
  public int SharedWithUserId { get; private set; } // Com quem foi compartilhado
  public SharePermission Permission { get; private set; }
  public DateTime SharedAt { get; private set; }
  public bool IsActive { get; private set; } // Pode ser revogado

  // Navigation properties (para o EF Core carregar relacionamentos)
  public Playlist? Playlist { get; private set; }
  public User? Owner { get; private set; }
  public User? SharedWithUser { get; private set; }

  // Construtor vazio para EF Core
  private PlaylistShare() { }

  // Construtor para criar compartilhamento
  public PlaylistShare(int playlistId, int ownerId, int sharedWithUserId, SharePermission permission)
  {
    ValidatePlaylistId(playlistId);
    ValidateOwnerId(ownerId);
    ValidateSharedWithUserId(sharedWithUserId, ownerId);

    PlaylistId = playlistId;
    OwnerId = ownerId;
    SharedWithUserId = sharedWithUserId;
    Permission = permission;
    SharedAt = DateTime.UtcNow;
    IsActive = true;
  }

  // Métodos de negócio
  public void Revoke()
  {
    if (!IsActive)
      throw new InvalidOperationException("Share is already revoked");

    IsActive = false;
  }

  public void Reactivate()
  {
    if (IsActive)
      throw new InvalidOperationException("Share is already active");

    IsActive = true;
  }

  public void ChangePermission(SharePermission newPermission)
  {
    if (!IsActive)
      throw new InvalidOperationException("Cannot change permission of revoked share");

    Permission = newPermission;
  }

  public bool CanUserEdit()
  {
    return IsActive && Permission == SharePermission.Edit;
  }

  public bool CanUserView()
  {
    return IsActive && (Permission == SharePermission.View || Permission == SharePermission.Edit);
  }

  public bool IsSharedWith(int userId)
  {
    return IsActive && SharedWithUserId == userId;
  }

  public bool IsOwnedBy(int userId)
  {
    return OwnerId == userId;
  }

  // Validações
  private void ValidatePlaylistId(int playlistId)
  {
    if (playlistId <= 0)
      throw new ArgumentException("Playlist ID must be greater than 0");
  }

  private void ValidateOwnerId(int ownerId)
  {
    if (ownerId <= 0)
      throw new ArgumentException("Owner ID must be greater than 0");
  }

  private void ValidateSharedWithUserId(int sharedWithUserId, int ownerId)
  {
    if (sharedWithUserId <= 0)
      throw new ArgumentException("Shared with user ID must be greater than 0");

    if (sharedWithUserId == ownerId)
      throw new ArgumentException("Cannot share playlist with yourself");
  }
}

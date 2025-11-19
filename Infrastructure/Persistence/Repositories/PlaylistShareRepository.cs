namespace Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Repositories;

public class PlaylistShareRepository : IPlaylistShareRepository
{
  private readonly AppDbContext _context;

  public PlaylistShareRepository(AppDbContext context)
  {
    _context = context;
  }

  public async Task<PlaylistShare?> GetByIdAsync(int id)
  {
    return await _context.PlaylistShares
      .Include(ps => ps.Playlist)
      .Include(ps => ps.Owner)
      .Include(ps => ps.SharedWithUser)
      .FirstOrDefaultAsync(ps => ps.Id == id);
  }

  public async Task<IEnumerable<PlaylistShare>> GetAllAsync()
  {
    return await _context.PlaylistShares
      .Include(ps => ps.Playlist)
      .Include(ps => ps.Owner)
      .Include(ps => ps.SharedWithUser)
      .ToListAsync();
  }

  public async Task<IEnumerable<PlaylistShare>> GetPaginatedAsync(int page, int pageSize)
  {
    return await _context.PlaylistShares
      .Include(ps => ps.Playlist)
      .Include(ps => ps.Owner)
      .Include(ps => ps.SharedWithUser)
      .Skip((page - 1) * pageSize)
      .Take(pageSize)
      .ToListAsync();
  }

  public async Task AddAsync(PlaylistShare share)
  {
    await _context.PlaylistShares.AddAsync(share);
    await _context.SaveChangesAsync();
  }

  public async Task UpdateAsync(PlaylistShare share)
  {
    _context.PlaylistShares.Update(share);
    await _context.SaveChangesAsync();
  }

  public async Task DeleteAsync(int id)
  {
    var share = await _context.PlaylistShares.FindAsync(id);
    if (share != null)
    {
      _context.PlaylistShares.Remove(share);
      await _context.SaveChangesAsync();
    }
  }

  public async Task<IEnumerable<PlaylistShare>> GetByPlaylistIdAsync(int playlistId)
  {
    return await _context.PlaylistShares
      .Include(ps => ps.Owner)
      .Include(ps => ps.SharedWithUser)
      .Where(ps => ps.PlaylistId == playlistId)
      .ToListAsync();
  }

  public async Task<IEnumerable<PlaylistShare>> GetActiveByPlaylistIdAsync(int playlistId)
  {
    return await _context.PlaylistShares
      .Include(ps => ps.Owner)
      .Include(ps => ps.SharedWithUser)
      .Where(ps => ps.PlaylistId == playlistId && ps.IsActive)
      .ToListAsync();
  }

  public async Task<int> CountByPlaylistIdAsync(int playlistId)
  {
    return await _context.PlaylistShares
      .CountAsync(ps => ps.PlaylistId == playlistId);
  }

  public async Task<IEnumerable<PlaylistShare>> GetSharedWithUserAsync(int userId)
  {
    return await _context.PlaylistShares
      .Include(ps => ps.Playlist)
        .ThenInclude(p => p!.User)
      .Include(ps => ps.Owner)
      .Where(ps => ps.SharedWithUserId == userId)
      .ToListAsync();
  }

  public async Task<IEnumerable<PlaylistShare>> GetActiveSharedWithUserAsync(int userId)
  {
    return await _context.PlaylistShares
      .Include(ps => ps.Playlist)
        .ThenInclude(p => p!.User)
      .Include(ps => ps.Owner)
      .Where(ps => ps.SharedWithUserId == userId && ps.IsActive)
      .ToListAsync();
  }

  public async Task<int> CountSharedWithUserAsync(int userId)
  {
    return await _context.PlaylistShares
      .CountAsync(ps => ps.SharedWithUserId == userId);
  }

  public async Task<IEnumerable<PlaylistShare>> GetSharedByUserAsync(int ownerId)
  {
    return await _context.PlaylistShares
      .Include(ps => ps.Playlist)
      .Include(ps => ps.SharedWithUser)
      .Where(ps => ps.OwnerId == ownerId)
      .ToListAsync();
  }

  public async Task<IEnumerable<PlaylistShare>> GetActiveSharedByUserAsync(int ownerId)
  {
    return await _context.PlaylistShares
      .Include(ps => ps.Playlist)
      .Include(ps => ps.SharedWithUser)
      .Where(ps => ps.OwnerId == ownerId && ps.IsActive)
      .ToListAsync();
  }

  public async Task<int> CountSharedByUserAsync(int ownerId)
  {
    return await _context.PlaylistShares
      .CountAsync(ps => ps.OwnerId == ownerId);
  }

  public async Task<bool> IsPlaylistSharedWithUserAsync(int playlistId, int userId)
  {
    return await _context.PlaylistShares
      .AnyAsync(ps => ps.PlaylistId == playlistId && ps.SharedWithUserId == userId && ps.IsActive);
  }

  public async Task<PlaylistShare?> GetShareAsync(int playlistId, int userId)
  {
    return await _context.PlaylistShares
      .Include(ps => ps.Playlist)
      .Include(ps => ps.Owner)
      .Include(ps => ps.SharedWithUser)
      .FirstOrDefaultAsync(ps => ps.PlaylistId == playlistId && ps.SharedWithUserId == userId);
  }

  public async Task<bool> ShareExistsAsync(int playlistId, int ownerId, int sharedWithUserId)
  {
    return await _context.PlaylistShares
      .AnyAsync(ps => ps.PlaylistId == playlistId && ps.OwnerId == ownerId && ps.SharedWithUserId == sharedWithUserId);
  }

  public async Task<int> CountAsync()
  {
    return await _context.PlaylistShares.CountAsync();
  }

  public async Task<int> CountActiveAsync()
  {
    return await _context.PlaylistShares
      .CountAsync(ps => ps.IsActive);
  }
}

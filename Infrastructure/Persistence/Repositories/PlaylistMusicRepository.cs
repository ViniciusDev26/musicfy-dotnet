namespace Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Repositories;

public class PlaylistMusicRepository : IPlaylistMusicRepository
{
  private readonly AppDbContext _context;

  public PlaylistMusicRepository(AppDbContext context)
  {
    _context = context;
  }

  public async Task<PlaylistMusic?> GetByIdAsync(int id)
  {
    return await _context.PlaylistMusics
      .Include(pm => pm.Playlist)
      .Include(pm => pm.Music)
      .Include(pm => pm.AddedByUser)
      .FirstOrDefaultAsync(pm => pm.Id == id);
  }

  public async Task<IEnumerable<PlaylistMusic>> GetAllAsync()
  {
    return await _context.PlaylistMusics
      .Include(pm => pm.Playlist)
      .Include(pm => pm.Music)
      .Include(pm => pm.AddedByUser)
      .ToListAsync();
  }

  public async Task AddAsync(PlaylistMusic playlistMusic)
  {
    await _context.PlaylistMusics.AddAsync(playlistMusic);
    await _context.SaveChangesAsync();
  }

  public async Task UpdateAsync(PlaylistMusic playlistMusic)
  {
    _context.PlaylistMusics.Update(playlistMusic);
    await _context.SaveChangesAsync();
  }

  public async Task DeleteAsync(int id)
  {
    var playlistMusic = await _context.PlaylistMusics.FindAsync(id);
    if (playlistMusic != null)
    {
      _context.PlaylistMusics.Remove(playlistMusic);
      await _context.SaveChangesAsync();
    }
  }

  public async Task<IEnumerable<PlaylistMusic>> GetByPlaylistIdAsync(int playlistId)
  {
    return await _context.PlaylistMusics
      .Include(pm => pm.Music)
      .Include(pm => pm.AddedByUser)
      .Where(pm => pm.PlaylistId == playlistId)
      .ToListAsync();
  }

  public async Task<IEnumerable<PlaylistMusic>> GetByPlaylistIdOrderedAsync(int playlistId)
  {
    return await _context.PlaylistMusics
      .Include(pm => pm.Music)
      .Include(pm => pm.AddedByUser)
      .Where(pm => pm.PlaylistId == playlistId)
      .OrderBy(pm => pm.Order)
      .ToListAsync();
  }

  public async Task<int> CountByPlaylistIdAsync(int playlistId)
  {
    return await _context.PlaylistMusics
      .CountAsync(pm => pm.PlaylistId == playlistId);
  }

  public async Task<IEnumerable<PlaylistMusic>> GetByMusicIdAsync(int musicId)
  {
    return await _context.PlaylistMusics
      .Include(pm => pm.Playlist)
        .ThenInclude(p => p!.User)
      .Include(pm => pm.AddedByUser)
      .Where(pm => pm.MusicId == musicId)
      .ToListAsync();
  }

  public async Task<int> CountByMusicIdAsync(int musicId)
  {
    return await _context.PlaylistMusics
      .CountAsync(pm => pm.MusicId == musicId);
  }

  public async Task<bool> MusicExistsInPlaylistAsync(int playlistId, int musicId)
  {
    return await _context.PlaylistMusics
      .AnyAsync(pm => pm.PlaylistId == playlistId && pm.MusicId == musicId);
  }

  public async Task<PlaylistMusic?> GetByPlaylistAndMusicAsync(int playlistId, int musicId)
  {
    return await _context.PlaylistMusics
      .FirstOrDefaultAsync(pm => pm.PlaylistId == playlistId && pm.MusicId == musicId);
  }

  public async Task<int> GetNextOrderAsync(int playlistId)
  {
    var maxOrder = await _context.PlaylistMusics
      .Where(pm => pm.PlaylistId == playlistId)
      .MaxAsync(pm => (int?)pm.Order);

    return (maxOrder ?? -1) + 1;
  }

  public async Task<IEnumerable<PlaylistMusic>> GetByPlaylistIdInOrderRangeAsync(int playlistId, int startOrder, int endOrder)
  {
    return await _context.PlaylistMusics
      .Where(pm => pm.PlaylistId == playlistId && pm.Order >= startOrder && pm.Order <= endOrder)
      .OrderBy(pm => pm.Order)
      .ToListAsync();
  }

  public async Task<int> CountAsync()
  {
    return await _context.PlaylistMusics.CountAsync();
  }
}

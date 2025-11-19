namespace Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Repositories;

public class PlaylistRepository : IPlaylistRepository
{
  private readonly AppDbContext _context;

  public PlaylistRepository(AppDbContext context)
  {
    _context = context;
  }

  public async Task<Playlist?> GetByIdAsync(int id)
  {
    return await _context.Playlists
      .Include(p => p.User)
      .FirstOrDefaultAsync(p => p.Id == id);
  }

  public async Task<Playlist?> GetByNameAndUserIdAsync(string name, int userId)
  {
    return await _context.Playlists
      .FirstOrDefaultAsync(p => p.Name == name && p.UserId == userId);
  }

  public async Task<IEnumerable<Playlist>> GetAllAsync()
  {
    return await _context.Playlists
      .Include(p => p.User)
      .ToListAsync();
  }

  public async Task<IEnumerable<Playlist>> GetPaginatedAsync(int page, int pageSize)
  {
    return await _context.Playlists
      .Include(p => p.User)
      .Skip((page - 1) * pageSize)
      .Take(pageSize)
      .ToListAsync();
  }

  public async Task AddAsync(Playlist playlist)
  {
    await _context.Playlists.AddAsync(playlist);
    await _context.SaveChangesAsync();
  }

  public async Task UpdateAsync(Playlist playlist)
  {
    _context.Playlists.Update(playlist);
    await _context.SaveChangesAsync();
  }

  public async Task DeleteAsync(int id)
  {
    var playlist = await _context.Playlists.FindAsync(id);
    if (playlist != null)
    {
      _context.Playlists.Remove(playlist);
      await _context.SaveChangesAsync();
    }
  }

  public async Task<IEnumerable<Playlist>> GetByUserIdAsync(int userId)
  {
    return await _context.Playlists
      .Include(p => p.User)
      .Where(p => p.UserId == userId)
      .ToListAsync();
  }

  public async Task<IEnumerable<Playlist>> GetUserPlaylistsPaginatedAsync(int userId, int page, int pageSize)
  {
    return await _context.Playlists
      .Include(p => p.User)
      .Where(p => p.UserId == userId)
      .Skip((page - 1) * pageSize)
      .Take(pageSize)
      .ToListAsync();
  }

  public async Task<bool> PlaylistExistsAsync(string name, int userId)
  {
    return await _context.Playlists
      .AnyAsync(p => p.Name == name && p.UserId == userId);
  }

  public async Task<int> CountByUserIdAsync(int userId)
  {
    return await _context.Playlists
      .CountAsync(p => p.UserId == userId);
  }

  public async Task<IEnumerable<Playlist>> GetSystemPlaylistsAsync()
  {
    return await _context.Playlists
      .Where(p => p.UserId == null)
      .ToListAsync();
  }

  public async Task<IEnumerable<Playlist>> GetSystemPlaylistsPaginatedAsync(int page, int pageSize)
  {
    return await _context.Playlists
      .Where(p => p.UserId == null)
      .Skip((page - 1) * pageSize)
      .Take(pageSize)
      .ToListAsync();
  }

  public async Task<int> CountSystemPlaylistsAsync()
  {
    return await _context.Playlists
      .CountAsync(p => p.UserId == null);
  }

  public async Task<int> CountAsync()
  {
    return await _context.Playlists.CountAsync();
  }
}

namespace Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Repositories;

public class MusicRepository : IMusicRepository
{
  private readonly AppDbContext _context;

  public MusicRepository(AppDbContext context)
  {
    _context = context;
  }

  public async Task<Music?> GetByIdAsync(int id)
  {
    return await _context.Musics.FindAsync(id);
  }

  public async Task<Music?> GetByNameAsync(string name)
  {
    return await _context.Musics
      .FirstOrDefaultAsync(m => m.Name == name);
  }

  public async Task<IEnumerable<Music>> GetAllAsync()
  {
    return await _context.Musics.ToListAsync();
  }

  public async Task<IEnumerable<Music>> GetPaginatedAsync(int page, int pageSize)
  {
    return await _context.Musics
      .Skip((page - 1) * pageSize)
      .Take(pageSize)
      .ToListAsync();
  }

  public async Task AddAsync(Music music)
  {
    await _context.Musics.AddAsync(music);
    await _context.SaveChangesAsync();
  }

  public async Task UpdateAsync(Music music)
  {
    _context.Musics.Update(music);
    await _context.SaveChangesAsync();
  }

  public async Task DeleteAsync(int id)
  {
    var music = await GetByIdAsync(id);
    if (music != null)
    {
      _context.Musics.Remove(music);
      await _context.SaveChangesAsync();
    }
  }

  public async Task<bool> MusicExistsAsync(string name, string artist)
  {
    return await _context.Musics
      .AnyAsync(m => m.Name == name && m.Artist == artist);
  }

  public async Task<IEnumerable<Music>> GetByArtistAsync(string artist)
  {
    return await _context.Musics
      .Where(m => m.Artist == artist)
      .ToListAsync();
  }

  public async Task<IEnumerable<Music>> SearchByNameAsync(string searchTerm)
  {
    return await _context.Musics
      .Where(m => m.Name.Contains(searchTerm))
      .ToListAsync();
  }

  public async Task<int> CountAsync()
  {
    return await _context.Musics.CountAsync();
  }

  public async Task<int> CountByArtistAsync(string artist)
  {
    return await _context.Musics
      .CountAsync(m => m.Artist == artist);
  }
}

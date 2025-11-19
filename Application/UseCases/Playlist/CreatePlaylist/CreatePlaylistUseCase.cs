namespace Application.UseCases.Playlist.CreatePlaylist;

using Domain.Entities;
using Domain.Repositories;
using Application.Exceptions;

public class CreatePlaylistUseCase
{
  private readonly IPlaylistRepository _playlistRepository;
  private readonly IUserRepository _userRepository;
  private readonly IMusicRepository _musicRepository;
  private readonly IPlaylistMusicRepository _playlistMusicRepository;

  public CreatePlaylistUseCase(
    IPlaylistRepository playlistRepository,
    IUserRepository userRepository,
    IMusicRepository musicRepository,
    IPlaylistMusicRepository playlistMusicRepository)
  {
    _playlistRepository = playlistRepository;
    _userRepository = userRepository;
    _musicRepository = musicRepository;
    _playlistMusicRepository = playlistMusicRepository;
  }

  public async Task<CreatePlaylistOutput> ExecuteAsync(CreatePlaylistInput input)
  {
    Domain.Entities.Playlist playlist;

    // Criar playlist do sistema
    if (!input.UserId.HasValue)
    {
      // Validação: verificar se já existe playlist do sistema com mesmo nome
      var systemPlaylists = await _playlistRepository.GetSystemPlaylistsAsync();
      var nameExists = systemPlaylists.Any(p =>
        p.Name.Equals(input.Name, StringComparison.OrdinalIgnoreCase));

      if (nameExists)
      {
        throw new BusinessException($"System playlist with name '{input.Name}' already exists");
      }

      playlist = Domain.Entities.Playlist.CreateSystemPlaylist(input.Name);
    }
    // Criar playlist de usuário
    else
    {
      // Validação: verificar se usuário existe
      var user = await _userRepository.GetByIdAsync(input.UserId.Value);
      if (user == null)
      {
        throw new NotFoundException("User", input.UserId.Value);
      }

      // Validação: verificar se usuário já tem playlist com esse nome
      var playlistExists = await _playlistRepository.PlaylistExistsAsync(
        input.Name,
        input.UserId.Value);

      if (playlistExists)
      {
        throw new BusinessException($"User already has a playlist named '{input.Name}'");
      }

      playlist = new Domain.Entities.Playlist(input.Name, input.UserId.Value);
    }

    // Persistir playlist
    await _playlistRepository.AddAsync(playlist);

    // Adicionar músicas à playlist (se fornecidas)
    var addedMusics = new List<MusicInPlaylistDto>();

    if (input.MusicIds != null && input.MusicIds.Any())
    {
      // Validar que todas as músicas existem
      var order = 0;
      foreach (var musicId in input.MusicIds)
      {
        var music = await _musicRepository.GetByIdAsync(musicId);
        if (music == null)
        {
          throw new NotFoundException("Music", musicId);
        }

        // Verificar se já não foi adicionada (evita duplicatas no mesmo request)
        var alreadyAdded = addedMusics.Any(m => m.MusicId == musicId);
        if (alreadyAdded)
        {
          continue; // Pula duplicatas
        }

        // Criar relacionamento PlaylistMusic
        var playlistMusic = new PlaylistMusic(
          playlist.Id,
          musicId,
          order,
          input.UserId // Quem adicionou (null se for playlist do sistema)
        );

        await _playlistMusicRepository.AddAsync(playlistMusic);

        addedMusics.Add(new MusicInPlaylistDto
        {
          MusicId = music.Id,
          Name = music.Name,
          Artist = music.Artist,
          Order = order
        });

        order++;
      }
    }

    // Retornar output
    return new CreatePlaylistOutput
    {
      Id = playlist.Id,
      Name = playlist.Name,
      UserId = playlist.UserId,
      IsSystemPlaylist = playlist.IsSystemPlaylist(),
      CreatedAt = playlist.CreatedAt,
      TotalMusics = addedMusics.Count,
      Musics = addedMusics
    };
  }
}

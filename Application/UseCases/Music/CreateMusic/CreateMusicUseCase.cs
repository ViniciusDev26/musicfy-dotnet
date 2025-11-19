namespace Application.UseCases.Music.CreateMusic;

using Domain.Entities;
using Domain.Repositories;
using Application.Exceptions;

public class CreateMusicUseCase
{
  private readonly IMusicRepository _musicRepository;

  public CreateMusicUseCase(IMusicRepository musicRepository)
  {
    _musicRepository = musicRepository;
  }

  public async Task<CreateMusicOutput> ExecuteAsync(CreateMusicInput input)
  {
    // Validação de negócio: verificar se música já existe (mesmo nome + mesmo artista)
    var musicExists = await _musicRepository.MusicExistsAsync(input.Name, input.Artist);
    if (musicExists)
    {
      throw new BusinessException($"Music '{input.Name}' by '{input.Artist}' already exists");
    }

    // Criar entidade (validações de domínio acontecem no construtor)
    var music = new Domain.Entities.Music(
      input.Name,
      input.Artist,
      input.AudioUrl
    );

    // Persistir
    await _musicRepository.AddAsync(music);

    // Retornar output
    return new CreateMusicOutput
    {
      Id = music.Id,
      Name = music.Name,
      Artist = music.Artist,
      AudioUrl = music.AudioUrl
    };
  }
}

namespace Domain.Entities;

public class Music
{
  public int Id { get; private set; }
  public string Name { get; private set; }
  public string Artist { get; private set; }
  public string AudioUrl { get; private set; }

  // Construtor vazio para o EF Core
  private Music() { }

  // Construtor para criar nova música
  public Music(string name, string artist, string audioUrl)
  {
    ValidateName(name);
    ValidateArtist(artist);
    ValidateAudioUrl(audioUrl);

    Name = name;
    Artist = artist;
    AudioUrl = audioUrl;
  }

  // Métodos para alterar propriedades (mantém encapsulamento)
  public void ChangeName(string name)
  {
    ValidateName(name);
    Name = name;
  }

  public void ChangeArtist(string artist)
  {
    ValidateArtist(artist);
    Artist = artist;
  }

  public void ChangeAudioUrl(string audioUrl)
  {
    ValidateAudioUrl(audioUrl);
    AudioUrl = audioUrl;
  }

  // Validações (regras de negócio no domínio)
  private void ValidateName(string name)
  {
    if (string.IsNullOrWhiteSpace(name))
      throw new ArgumentException("Music name cannot be empty");

    if (name.Length < 1)
      throw new ArgumentException("Music name must have at least 1 character");

    if (name.Length > 200)
      throw new ArgumentException("Music name cannot exceed 200 characters");
  }

  private void ValidateArtist(string artist)
  {
    if (string.IsNullOrWhiteSpace(artist))
      throw new ArgumentException("Artist name cannot be empty");

    if (artist.Length < 1)
      throw new ArgumentException("Artist name must have at least 1 character");

    if (artist.Length > 200)
      throw new ArgumentException("Artist name cannot exceed 200 characters");
  }

  private void ValidateAudioUrl(string audioUrl)
  {
    if (string.IsNullOrWhiteSpace(audioUrl))
      throw new ArgumentException("Audio URL cannot be empty");

    // Validação básica de URL
    if (!Uri.TryCreate(audioUrl, UriKind.Absolute, out var uri))
      throw new ArgumentException("Invalid audio URL format");

    // Verifica se é HTTP ou HTTPS
    if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
      throw new ArgumentException("Audio URL must be HTTP or HTTPS");
  }
}

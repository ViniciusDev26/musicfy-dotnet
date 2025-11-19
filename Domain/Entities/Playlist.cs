namespace Domain.Entities;

public class Playlist
{
  public int Id { get; private set; }
  public string Name { get; private set; }
  public int? UserId { get; private set; } // Nullable - se null, é playlist do sistema
  public DateTime CreatedAt { get; private set; }

  // Navigation property (opcional, para o EF Core carregar o User)
  public User? User { get; private set; }

  // Construtor vazio para o EF Core
  private Playlist() { }

  // Construtor para criar playlist de usuário
  public Playlist(string name, int userId)
  {
    ValidateName(name);
    ValidateUserId(userId);

    Name = name;
    UserId = userId;
    CreatedAt = DateTime.UtcNow;
  }

  // Construtor para criar playlist do sistema
  public static Playlist CreateSystemPlaylist(string name)
  {
    if (string.IsNullOrWhiteSpace(name))
      throw new ArgumentException("System playlist name cannot be empty");

    if (name.Length < 3)
      throw new ArgumentException("System playlist name must have at least 3 characters");

    return new Playlist
    {
      Name = name,
      UserId = null,
      CreatedAt = DateTime.UtcNow
    };
  }

  // Métodos para alterar propriedades
  public void ChangeName(string name)
  {
    ValidateName(name);
    Name = name;
  }

  // Validações (regras de negócio no domínio)
  private void ValidateName(string name)
  {
    if (string.IsNullOrWhiteSpace(name))
      throw new ArgumentException("Playlist name cannot be empty");

    if (name.Length < 3)
      throw new ArgumentException("Playlist name must have at least 3 characters");

    if (name.Length > 100)
      throw new ArgumentException("Playlist name cannot exceed 100 characters");
  }

  private void ValidateUserId(int userId)
  {
    if (userId <= 0)
      throw new ArgumentException("User ID must be greater than 0");
  }

  // Métodos de negócio
  public bool IsSystemPlaylist() => UserId == null;

  public bool BelongsToUser(int userId)
  {
    return !IsSystemPlaylist() && UserId == userId;
  }

  public bool CanBeEditedBy(int userId)
  {
    // Playlist do sistema não pode ser editada por usuários
    if (IsSystemPlaylist())
      return false;

    // Playlist só pode ser editada pelo dono
    return UserId == userId;
  }

  public bool CanBeDeletedBy(int userId)
  {
    // Playlist do sistema não pode ser deletada
    if (IsSystemPlaylist())
      return false;

    // Playlist só pode ser deletada pelo dono
    return UserId == userId;
  }
}

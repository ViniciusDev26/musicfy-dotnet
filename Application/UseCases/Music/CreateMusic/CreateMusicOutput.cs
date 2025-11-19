namespace Application.UseCases.Music.CreateMusic;

public class CreateMusicOutput
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string Artist { get; set; } = string.Empty;
  public string AudioUrl { get; set; } = string.Empty;
}

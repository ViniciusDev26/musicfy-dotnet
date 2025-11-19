namespace Application.UseCases.User.CreateUser;

public class CreateUserOutput
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public DateTime BirthDate { get; set; }
  public string Email { get; set; } = string.Empty;
  public int Age { get; set; }
}

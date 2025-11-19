namespace Application.UseCases.User.CreateUser;

public class CreateUserInput
{
  public string Name { get; set; } = string.Empty;
  public DateTime BirthDate { get; set; }
  public string Email { get; set; } = string.Empty;
}

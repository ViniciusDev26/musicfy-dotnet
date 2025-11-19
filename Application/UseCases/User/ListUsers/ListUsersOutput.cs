namespace Application.UseCases.User.ListUsers;

public class ListUsersOutput
{
  public List<UserDto> Users { get; set; } = new();
  public int TotalCount { get; set; }
  public int? Page { get; set; }
  public int? PageSize { get; set; }
  public int? TotalPages { get; set; }
}

public class UserDto
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public DateTime BirthDate { get; set; }
  public string Email { get; set; } = string.Empty;
  public int Age { get; set; }
}

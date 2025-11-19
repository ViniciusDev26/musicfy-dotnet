namespace Application.UseCases.User.ListUsers;

using Domain.Repositories;

public class ListUsersUseCase
{
  private readonly IUserRepository _userRepository;

  public ListUsersUseCase(IUserRepository userRepository)
  {
    _userRepository = userRepository;
  }

  public async Task<ListUsersOutput> ExecuteAsync(ListUsersInput input)
  {
    IEnumerable<Domain.Entities.User> users;
    int totalCount;

    // Se informou página e tamanho, usa paginação
    if (input.Page.HasValue && input.PageSize.HasValue)
    {
      users = await _userRepository.GetPaginatedAsync(input.Page.Value, input.PageSize.Value);
      totalCount = await _userRepository.CountAsync();

      var totalPages = (int)Math.Ceiling((double)totalCount / input.PageSize.Value);

      return new ListUsersOutput
      {
        Users = users.Select(u => new UserDto
        {
          Id = u.Id,
          Name = u.Name,
          BirthDate = u.BirthDate,
          Email = u.Email,
          Age = u.GetAge()
        }).ToList(),
        TotalCount = totalCount,
        Page = input.Page.Value,
        PageSize = input.PageSize.Value,
        TotalPages = totalPages
      };
    }

    // Caso contrário, retorna todos
    users = await _userRepository.GetAllAsync();
    totalCount = users.Count();

    return new ListUsersOutput
    {
      Users = users.Select(u => new UserDto
      {
        Id = u.Id,
        Name = u.Name,
        BirthDate = u.BirthDate,
        Email = u.Email,
        Age = u.GetAge()
      }).ToList(),
      TotalCount = totalCount
    };
  }
}

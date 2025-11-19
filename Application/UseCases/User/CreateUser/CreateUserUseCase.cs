namespace Application.UseCases.User.CreateUser;

using Domain.Entities;
using Domain.Repositories;
using Application.Exceptions;

public class CreateUserUseCase
{
  private readonly IUserRepository _userRepository;

  public CreateUserUseCase(IUserRepository userRepository)
  {
    _userRepository = userRepository;
  }

  public async Task<CreateUserOutput> ExecuteAsync(CreateUserInput input)
  {
    // Validação de negócio: verificar se email já existe
    var emailExists = await _userRepository.EmailExistsAsync(input.Email);
    if (emailExists)
    {
      throw new BusinessException($"Email '{input.Email}' is already in use");
    }

    // Criar entidade (validações de domínio acontecem no construtor)
    var user = new Domain.Entities.User(
      input.Name,
      input.BirthDate,
      input.Email
    );

    // Persistir
    await _userRepository.AddAsync(user);

    // Retornar output
    return new CreateUserOutput
    {
      Id = user.Id,
      Name = user.Name,
      BirthDate = user.BirthDate,
      Email = user.Email,
      Age = user.GetAge()
    };
  }
}

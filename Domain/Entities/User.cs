namespace Domain.Entities;

public class User
{
  public int Id { get; private set; }
  public string Name { get; private set; }
  public DateTime BirthDate { get; private set; }
  public string Email { get; private set; }

  // Construtor vazio para o EF Core
  private User() { }

  // Construtor para criar novo usuário
  public User(string name, DateTime birthDate, string email)
  {
    ValidateName(name);
    ValidateEmail(email);
    ValidateBirthDate(birthDate);

    Name = name;
    BirthDate = birthDate;
    Email = email;
  }

  // Métodos para alterar propriedades (mantém encapsulamento)
  public void ChangeName(string name)
  {
    ValidateName(name);
    Name = name;
  }

  public void ChangeEmail(string email)
  {
    ValidateEmail(email);
    Email = email;
  }

  // Validações (regras de negócio no domínio)
  private void ValidateName(string name)
  {
    if (string.IsNullOrWhiteSpace(name))
      throw new ArgumentException("Name cannot be empty");

    if (name.Length < 3)
      throw new ArgumentException("Name must have at least 3 characters");
  }

  private void ValidateEmail(string email)
  {
    if (string.IsNullOrWhiteSpace(email))
      throw new ArgumentException("Email cannot be empty");

    if (!email.Contains("@"))
      throw new ArgumentException("Invalid email format");
  }

  private void ValidateBirthDate(DateTime birthDate)
  {
    if (birthDate >= DateTime.Now)
      throw new ArgumentException("Birth date must be in the past");

    var age = DateTime.Now.Year - birthDate.Year;
    if (age < 13)
      throw new ArgumentException("User must be at least 13 years old");
  }

  // Método útil (regra de negócio)
  public int GetAge()
  {
    var today = DateTime.Now;
    var age = today.Year - BirthDate.Year;

    if (BirthDate.Date > today.AddYears(-age))
      age--;

    return age;
  }
}
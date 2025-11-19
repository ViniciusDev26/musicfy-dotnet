using System.ServiceModel;
using Api.Soap.Models;

namespace Api.Soap.Contracts;

[ServiceContract]
public interface IUserSoapService
{
  [OperationContract]
  Task<GetUsersResponse> GetUsers(GetUsersRequest request);

  [OperationContract]
  Task<UserSoapDto> CreateUser(CreateUserRequest request);
}

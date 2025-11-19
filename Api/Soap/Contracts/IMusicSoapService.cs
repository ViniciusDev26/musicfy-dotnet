using System.ServiceModel;
using Api.Soap.Models;

namespace Api.Soap.Contracts;

[ServiceContract]
public interface IMusicSoapService
{
  [OperationContract]
  Task<GetMusicsResponse> GetMusics(GetMusicsRequest request);

  [OperationContract]
  Task<MusicSoapDto> CreateMusic(CreateMusicRequest request);
}

using System.ServiceModel;
using Api.Soap.Models;

namespace Api.Soap.Contracts;

[ServiceContract]
public interface IPlaylistSoapService
{
  [OperationContract]
  Task<GetPlaylistsResponse> GetPlaylists(GetPlaylistsRequest request);

  [OperationContract]
  Task<PlaylistSoapDto> CreatePlaylist(CreatePlaylistRequest request);
}
